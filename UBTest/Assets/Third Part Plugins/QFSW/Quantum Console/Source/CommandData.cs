using QFSW.QC.Comparators;
using QFSW.QC.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace QFSW.QC
{
    /// <summary>
    /// Contains the full data about a command and provides an execution point for invoking the command.
    /// </summary>
    public class CommandData
    {
        public readonly string CommandName;
        public readonly string CommandDescription;
        public readonly string CommandSignature;
        public readonly string ParamaterSignature;
        public readonly string GenericSignature;
        public readonly ParameterInfo[] MethodParamData;
        public readonly Type[] ParamTypes;
        public readonly Type[] GenericParamTypes;
        public readonly MethodInfo MethodData;
        public readonly MonoTargetType MonoTarget;

        private readonly object[] _defaultParameters;
        private readonly bool _isMono;

        public bool IsGeneric => GenericParamTypes.Length > 0;
        public bool IsStatic => MethodData.IsStatic;
        public bool HasDescription => !string.IsNullOrWhiteSpace(CommandDescription);
        public int ParamCount => ParamTypes.Length - _defaultParameters.Length;

        public Type[] MakeGenericArguments(params Type[] genericTypeArguments)
        {
            if (genericTypeArguments.Length != GenericParamTypes.Length)
            {
                throw new ArgumentException("Incorrect number of generic subsitution types were supplied.");
            }

            Dictionary<string, Type> subsitutionTable = new Dictionary<string, Type>();
            for (int i = 0; i < genericTypeArguments.Length; i++)
            {
                subsitutionTable.Add(GenericParamTypes[i].Name, genericTypeArguments[i]);
            }

            Type[] types = new Type[ParamTypes.Length];
            for (int i = 0; i < types.Length; i++)
            {
                if (ParamTypes[i].ContainsGenericParameters)
                {
                    Type subsitution = ConstructGenericType(ParamTypes[i], subsitutionTable);
                    types[i] = subsitution;
                }
                else
                {
                    types[i] = ParamTypes[i];
                }
            }

            return types;
        }

        private Type ConstructGenericType(Type genericType, Dictionary<string, Type> subsitutionTable)
        {
            if (!genericType.ContainsGenericParameters) { return genericType; }
            if (subsitutionTable.ContainsKey(genericType.Name)) { return subsitutionTable[genericType.Name]; }
            if (genericType.IsArray) { return ConstructGenericType(genericType.GetElementType(), subsitutionTable).MakeArrayType(); }
            if (genericType.IsGenericType)
            {
                Type baseType = genericType.GetGenericTypeDefinition();
                Type[] typeArguments = genericType.GetGenericArguments();
                for (int i = 0; i < typeArguments.Length; i++)
                {
                    typeArguments[i] = ConstructGenericType(typeArguments[i], subsitutionTable);
                }

                return baseType.MakeGenericType(typeArguments);
            }

            throw new ArgumentException($"Could not construct the generic type {genericType}");
        }

        public object Invoke(object[] paramData, Type[] genericTypeArguments)
        {
            object[] data = new object[paramData.Length + _defaultParameters.Length];
            Array.Copy(paramData, 0, data, 0, paramData.Length);
            Array.Copy(_defaultParameters, 0, data, paramData.Length, _defaultParameters.Length);

            MethodInfo invokingMethod = MethodData;
            if (IsGeneric)
            {
                try { invokingMethod = MethodData.MakeGenericMethod(genericTypeArguments); }
                catch (ArgumentException) { throw new ArgumentException($"Supplied generic parameters did not satisfy the generic constraints imposed by '{CommandName}'"); }
            }

            if (IsStatic)
            {
                return invokingMethod.Invoke(null, data);
            }
            else
            {
                Type declaringType = invokingMethod.DeclaringType;
                int registrySize = QuantumRegistry.GetRegistrySize(declaringType);
                if (MonoTarget == MonoTargetType.Single || (registrySize == 1 && MonoTarget == MonoTargetType.Registry))
                {
                    object target;
                    if (MonoTarget == MonoTargetType.Single)
                    {
                        target = GameObject.FindObjectOfType(invokingMethod.DeclaringType);
                        if (target == null)
                        {
                            throw new Exception($"No objects of type {declaringType.GetDisplayName()} could not be found so the command could not be invoked.");
                        }
                    }
                    else
                    {
                        target = QuantumRegistry.GetRegistryContents(declaringType).First();
                    }

                    return invokingMethod.Invoke(target, data);
                }
                else if (MonoTarget == MonoTargetType.Singleton)
                {
                    object target;
                    if (registrySize > 0)
                    {
                        target = QuantumRegistry.GetRegistryContents(declaringType).First();
                    }
                    else
                    {
                        target = CreateCommandSingletonInstance(declaringType);
                        QuantumRegistry.RegisterObject(declaringType, target);
                    }

                    return invokingMethod.Invoke(target, data);
                }
                else if (MonoTarget == MonoTargetType.All || (registrySize > 1 && MonoTarget == MonoTargetType.Registry))
                {
                    IEnumerable<object> targets;
                    if (MonoTarget == MonoTargetType.All)
                    {
                        UnityEngine.Object[] targetsRaw = GameObject.FindObjectsOfType(invokingMethod.DeclaringType);
                        if (targetsRaw.Length == 0)
                        {
                            throw new Exception($"No objects of type {declaringType.GetDisplayName()} could not be found so the command could not be invoked.");
                        }

                        targets = targetsRaw.OrderBy(x => x.name, new AlphanumComparator());
                    }
                    else
                    {
                        targets = QuantumRegistry.GetRegistryContents(declaringType);
                    }

                    int returnCount = 0;
                    Dictionary<object, object> resultsParts = new Dictionary<object, object>();
                    foreach (object target in targets)
                    {
                        object result = invokingMethod.Invoke(target, data);
                        if (result != null)
                        {
                            resultsParts.Add(target, result);
                            returnCount++;
                        }
                    }

                    if (returnCount > 1)
                    {
                        return resultsParts;
                    }
                    else if (returnCount == 1)
                    {
                        return resultsParts.Values.First();
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    throw new Exception($"No objects of type {declaringType.GetDisplayName()} could be found in the registry so the command could not be invoked. " +
                        $"Please add objects to the registry by using QFSW.QC.QuantumRegistry.Register");
                }
            }
        }

        private Component CreateCommandSingletonInstance(Type classType)
        {
            GameObject obj = new GameObject($"{classType}Singleton");
            GameObject.DontDestroyOnLoad(obj);
            return obj.AddComponent(classType);
        }

        public CommandData(MethodInfo methodData, int defaultParameterCount = 0) : this(methodData, methodData.Name, defaultParameterCount) { }
        public CommandData(MethodInfo methodData, string commandName, int defaultParameterCount = 0)
        {
            CommandName = commandName;
            MethodData = methodData;

            if (string.IsNullOrWhiteSpace(commandName))
            {
                CommandName = methodData.Name;
            }

            Type declaringType = methodData.DeclaringType;
            _isMono = typeof(MonoBehaviour).IsAssignableFrom(declaringType);
            if (!_isMono)
            {
                MonoTarget = MonoTargetType.Registry;
            }

            while (declaringType != null)
            {
                IEnumerable<CommandPrefixAttribute> prefixAttributes = declaringType.GetCustomAttributes<CommandPrefixAttribute>();
                foreach (CommandPrefixAttribute prefixAttribute in prefixAttributes.Reverse())
                {
                    if (prefixAttribute.Valid)
                    {
                        string prefix = prefixAttribute.Prefix;
                        if (string.IsNullOrWhiteSpace(prefix)) { prefix = declaringType.Name; }
                        CommandName = $"{prefix}{CommandName}";
                    }
                }

                declaringType = declaringType.DeclaringType;
            }

            MethodParamData = methodData.GetParameters();
            ParamTypes = new Type[MethodParamData.Length];
            for (int i = 0; i < ParamTypes.Length; i++) { ParamTypes[i] = MethodParamData[i].ParameterType; }

            _defaultParameters = new object[defaultParameterCount];
            for (int i = 0; i < defaultParameterCount; i++)
            {
                int j = MethodParamData.Length - defaultParameterCount + i;
                _defaultParameters[i] = MethodParamData[j].DefaultValue;
            }

            if (methodData.IsGenericMethodDefinition)
            {
                GenericParamTypes = methodData.GetGenericArguments();
                GenericSignature = $"<{string.Join(", ", GenericParamTypes.Select(x => x.Name))}>";
            }
            else { GenericParamTypes = Array.Empty<Type>(); }

            ParamaterSignature = string.Empty;
            for (int i = 0; i < MethodParamData.Length - defaultParameterCount; i++) { ParamaterSignature += $"{(i == 0 ? string.Empty : " ")}{MethodParamData[i].Name}"; }
            if (ParamCount > 0) { CommandSignature += $"{commandName}{GenericSignature} {ParamaterSignature}"; }
            else { CommandSignature = $"{commandName}{GenericSignature}"; }
        }

        public CommandData(MethodInfo methodData, CommandAttribute commandAttribute, int defaultParameterCount = 0) : this(methodData, commandAttribute.Alias, defaultParameterCount)
        {
            CommandDescription = commandAttribute.Description;
            if (_isMono) { MonoTarget = commandAttribute.MonoTarget; }
        }

        public CommandData(MethodInfo methodData, CommandAttribute commandAttribute, CommandDescriptionAttribute descriptionAttribute, int defaultParameterCount = 0)
            : this(methodData, commandAttribute, defaultParameterCount)
        {
            if ((descriptionAttribute?.Valid ?? false) && string.IsNullOrWhiteSpace(commandAttribute.Description))
            {
                CommandDescription = descriptionAttribute.Description;
            }
        }
    }
}
