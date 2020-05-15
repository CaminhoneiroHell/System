using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace QFSW.QC.Utilities
{
    public static class ReflectionExtensions
    {
        #region Lookup Tables
        private static readonly Dictionary<Type, string> _typeDisplayNames = new Dictionary<Type, string>
        {
            { typeof(int), "int" },
            { typeof(float), "float" },
            { typeof(decimal), "decimal" },
            { typeof(double), "double" },
            { typeof(string), "string" },
            { typeof(bool), "bool" },
            { typeof(byte), "byte" },
            { typeof(sbyte), "sbyte" },
            { typeof(uint), "uint" },
            { typeof(short), "short" },
            { typeof(ushort), "ushort" },
            { typeof(long), "decimal" },
            { typeof(ulong), "ulong" },
            { typeof(char), "char" },
            { typeof(object), "object" }
        };

        private static readonly Type[][] _primitiveTypeCastHierarchy =
        {
            new[] { typeof(byte),  typeof(sbyte), typeof(char) },
            new[] { typeof(short), typeof(ushort) },
            new[] { typeof(int), typeof(uint) },
            new[] { typeof(long), typeof(ulong) },
            new[] { typeof(float) },
            new[] { typeof(double) }
        };
        #endregion

        /// <summary>Determines if a type is a delegate.</summary>
        /// <returns>If the type is a delegate.</returns>
        public static bool IsDelegate(this Type type)
        {
            if (!typeof(Delegate).IsAssignableFrom(type)) { return false; }
            return true;
        }

        /// <summary>Determines if a type is a strongly typed delegate.</summary>
        /// <returns>If the type is a strongly typed delegate.</returns>
        public static bool IsStrongDelegate(this Type type)
        {
            if (!type.IsDelegate()) { return false; }
            if (type.IsAbstract) { return false; }
            return true;
        }

        /// <summary>Determines if a field is a delegate.</summary>
        /// <returns>If the field is a delegate.</returns>
        public static bool IsDelegate(this FieldInfo fieldInfo)
        {
            return fieldInfo.FieldType.IsDelegate();
        }

        /// <summary>Determines if a field is a strongly typed delegate.</summary>
        /// <param name="fieldInfo">The field to query.</param>
        /// <returns>If the field is a strongly typed delegate.</returns>
        public static bool IsStrongDelegate(this FieldInfo fieldInfo)
        {
            return fieldInfo.FieldType.IsStrongDelegate();
        }

        /// <summary>
        /// Determines if the field is a generic type of the given non-generic type.
        /// </summary>
        /// <param name="nonGenericType">The non-generic type to test against.</param>
        /// <returns>If the type is a generic type of the non-generic type.</returns>
        public static bool IsGenericTypeOf(this Type genericType, Type nonGenericType)
        {
            if (!genericType.IsGenericType) { return false; }
            return genericType.GetGenericTypeDefinition() == nonGenericType;
        }

        /// <summary>
        /// Determines if an object the given type can be casted to the specified type.
        /// </summary>
        /// <param name="to">The destination type of the cast.</param>
        /// <param name="implicitly">If only implicit casts should be considered.</param>
        /// <returns>If the cast can be performed.</returns>
        public static bool IsCastableTo(this Type from, Type to, bool implicitly = false)
        {
            return to.IsAssignableFrom(from) || from.HasCastDefined(to, implicitly);
        }

        private static bool HasCastDefined(this Type from, Type to, bool implicitly)
        {
            if ((from.IsPrimitive || from.IsEnum) && (to.IsPrimitive || to.IsEnum))
            {
                if (!implicitly)
                {
                    return from == to || (from != typeof(bool) && to != typeof(bool));
                }

                IEnumerable<Type> lowerTypes = Enumerable.Empty<Type>();
                foreach (Type[] types in _primitiveTypeCastHierarchy)
                {
                    if (types.Any(t => t == to))
                    {
                        return lowerTypes.Any(t => t == from);
                    }

                    lowerTypes = lowerTypes.Concat(types);
                }

                return false;   // IntPtr, UIntPtr, Enum, Boolean
            }

            return IsCastDefined(to, m => m.GetParameters()[0].ParameterType, _ => from, implicitly, false)
                || IsCastDefined(from, _ => to, m => m.ReturnType, implicitly, true);
        }

        private static bool IsCastDefined(Type type, Func<MethodInfo, Type> baseType, Func<MethodInfo, Type> derivedType, bool implicitly, bool lookInBase)
        {
            BindingFlags flags = BindingFlags.Public | BindingFlags.Static | (lookInBase ? BindingFlags.FlattenHierarchy : BindingFlags.DeclaredOnly);
            MethodInfo[] methods = type.GetMethods(flags);

            return methods.Where(m => m.Name == "op_Implicit" || (!implicitly && m.Name == "op_Explicit"))
                          .Any(m => baseType(m).IsAssignableFrom(derivedType(m)));
        }

        /// <summary>
        /// Dynamically casts an object to the specified type.
        /// </summary>
        /// <param name="type">The destination type of the cast.</param>
        /// <param name="data">The object to cast.</param>
        /// <returns>The dynamically casted object.</returns>
        public static object Cast(this Type type, object data)
        {
            if (type.IsInstanceOfType(data))
            {
                return data;
            }

            try
            {
                return Convert.ChangeType(data, type);
            }
            catch (InvalidCastException)
            {
                Type srcType = data.GetType();
                ParameterExpression dataParam = Expression.Parameter(srcType, "data");
                Expression body = Expression.Convert(Expression.Convert(dataParam, srcType), type);

                Delegate run = Expression.Lambda(body, dataParam).Compile();
                return run.DynamicInvoke(data);
            }
        }

        /// <summary>Determines if the given method is an override.</summary>
        /// <returns>If the method is an override.</returns>
        public static bool IsOverride(this MethodInfo methodInfo)
        {
            return methodInfo.GetBaseDefinition().DeclaringType != methodInfo.DeclaringType;
        }

        /// <summary>
        /// Gets if the provider has the specified attribute.
        /// </summary>
        /// <typeparam name="T">The attribute to test.</typeparam>
        /// <param name="provider">The attribute provider.</param>
        /// <param name="searchInherited">If base declarations should be searched.</param>
        /// <returns>If the attribute is present.</returns>
        public static bool HasAttribute<T>(this ICustomAttributeProvider provider, bool searchInherited = true) where T : Attribute
        {
            try
            {
                return provider.IsDefined(typeof(T), searchInherited);
            }
            catch (MissingMethodException)
            {
                return false;
            }
        }

        /// <summary>
        /// Gets a formatted display name for a given type.
        /// </summary>
        /// <param name="type">The type to generate a display name for.</param>
        /// <returns>The generated display name.</returns>
        public static string GetDisplayName(this Type type)
        {
            if (type.IsArray)
            {
                int rank = type.GetArrayRank();
                return $"{GetDisplayName(type.GetElementType())}[{new string(',', rank - 1)}]";
            }

            if (_typeDisplayNames.ContainsKey(type))
            {
                string baseName = _typeDisplayNames[type];
                if (type.IsGenericType && !type.IsConstructedGenericType)
                {
                    Type[] genericArgs = type.GetGenericArguments();
                    return $"{baseName}<{new string(',', genericArgs.Length - 1)}>";
                }
                else
                {
                    return baseName;
                }
            }

            if (type.IsGenericType)
            {
                Type baseType = type.GetGenericTypeDefinition();
                Type[] genericArgs = type.GetGenericArguments();

                if (type.IsConstructedGenericType)
                {
                    string[] genericNames = new string[genericArgs.Length];
                    for (int i = 0; i < genericArgs.Length; i++)
                    {
                        genericNames[i] = GetDisplayName(genericArgs[i]);
                    }

                    return $"{GetDisplayName(baseType).Split('<')[0]}<{string.Join(", ", genericNames)}>";
                }
                else
                {
                    return $"{type.Name.Split('`')[0]}<{new string(',', genericArgs.Length - 1)}>";
                }
            }

            return type.Name;
        }
    }
}