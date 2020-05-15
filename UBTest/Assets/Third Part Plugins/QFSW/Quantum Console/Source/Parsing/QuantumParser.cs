using QFSW.QC.Utilities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEngine;

namespace QFSW.QC
{
    /// <summary>
    /// Handles parsing values to use as console inputs.
    /// </summary>
    public class QuantumParser
    {
        private readonly IQcParser[] _parsers;
        private readonly IQcGrammarConstruct[] _grammarConstructs;
        private readonly ConcurrentDictionary<Type, IQcParser> _parserLookup = new ConcurrentDictionary<Type, IQcParser>();
        private readonly HashSet<Type> _unparseableLookup = new HashSet<Type>();

        private readonly Func<string, Type, object> _recursiveParser;

        /// <summary>
        /// Creates a Parser Serializer with a custom set of parsers.
        /// </summary>
        /// <param name="parsers">The IQcParsers to use in this Quantum Parser.</param>
        /// <param name="grammarConstructs">The IQcGrammarConstructs to use in this Quantum Parser</param>
        public QuantumParser(IEnumerable<IQcParser> parsers, IEnumerable<IQcGrammarConstruct> grammarConstructs)
        {
            _recursiveParser = Parse;

            _parsers = parsers.OrderByDescending(x => x.Priority)
                              .ToArray();

            _grammarConstructs = grammarConstructs.OrderBy(x => x.Precedence)
                                                  .ToArray();
        }

        /// <summary>
        /// Creates a Quantum Parser with the default injected parsers.
        /// </summary>
        public QuantumParser() : this(new InjectionLoader<IQcParser>().GetInjectedInstances(), new InjectionLoader<IQcGrammarConstruct>().GetInjectedInstances())
        {

        }

        public IQcParser GetParser(Type type)
        {
            if (_parserLookup.ContainsKey(type))
            {
                return _parserLookup[type];
            }
            else if (!_unparseableLookup.Contains(type))
            {
                foreach (IQcParser parser in _parsers)
                {
                    try
                    {
                        if (parser.CanParse(type))
                        {
                            return _parserLookup[type] = parser;
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.LogError($"{parser.GetType().GetDisplayName()}.CanParse is malformed and throws");
                        Debug.LogException(e);
                    }
                }

                _unparseableLookup.Add(type);
            }

            return null;
        }

        public bool CanParse(Type type)
        {
            return GetParser(type) != null;
        }

        private IQcGrammarConstruct GetMatchingGrammar(string value, Type type)
        {
            foreach (IQcGrammarConstruct grammar in _grammarConstructs)
            {
                try
                {
                    if (grammar.Match(value, type))
                    {
                        return grammar;
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError($"{grammar.GetType().GetDisplayName()}.Match is malformed and throws");
                    Debug.LogException(e);
                }
            }

            return null;
        }

        /// <summary>
        /// Parses a serialized string of data.
        /// </summary>
        /// <typeparam name="T">The type of the value to parse.</typeparam>
        /// <param name="value">The string to parse.</param>
        /// <returns>The parsed value.</returns>
        public T Parse<T>(string value)
        {
            return (T)Parse(value, typeof(T));
        }

        /// <summary>
        /// Parses a serialized string of data.
        /// </summary>
        /// <param name="value">The string to parse.</param>
        /// <param name="type">The type of the value to parse.</param>
        /// <returns>The parsed value.</returns>
        public object Parse(string value, Type type)
        {
            value = value.ReduceScope('(', ')');

            if (type.IsClass && value == "null")
            {
                return null;
            }

            value = value.ReduceScope('"', '"');

            IQcGrammarConstruct grammar = GetMatchingGrammar(value, type);
            if (grammar != null)
            {
                try
                {
                    return grammar.Parse(value, type, _recursiveParser);
                }
                catch (ParserException) { throw; }
                catch (Exception e)
                {
                    throw new Exception($"Parsing of {type.GetDisplayName()} via {grammar} failed:\n{e.Message}", e);
                }
            }

            IQcParser parser = GetParser(type);
            if (parser == null)
            {
                throw new ArgumentException($"Cannot parse object of type '{type.GetDisplayName()}'");
            }

            try
            {
                return parser.Parse(value, type, _recursiveParser);
            }
            catch (ParserException) { throw; }
            catch (Exception e)
            {
                throw new Exception($"Parsing of {type.GetDisplayName()} via {parser} failed:\n{e.Message}", e);
            }
        }


        #region Type Parser
        private static readonly Dictionary<Type, string> _typeDisplayNames = new Dictionary<Type, string> { { typeof(int), "int" }, { typeof(float), "float" }, { typeof(decimal), "decimal" },
                                                                                                           { typeof(double), "double" }, { typeof(string), "string" }, { typeof(bool), "bool" },
                                                                                                           { typeof(byte), "byte" }, { typeof(sbyte), "sbyte" }, { typeof(uint), "uint" },
                                                                                                           { typeof(short), "short" }, { typeof(ushort), "ushort" }, { typeof(long), "decimal" },
                                                                                                           { typeof(ulong), "ulong" }, { typeof(char), "char" }, { typeof(object), "object" } };

        private static readonly Dictionary<string, Type> _reverseTypeDisplayNames = _typeDisplayNames.Invert();
        private static readonly Assembly[] _loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies();
        private static readonly string[] _defaultNamespaces = new string[] { "System", "System.Collections", "System.Collections.Generic", "UnityEngine", "UnityEngine.UI", "QFSW.QC" };
        private static readonly List<string> _namespaceTable = new List<string>(_defaultNamespaces);
        private static readonly Regex _arrayTypeRegex = new Regex(@"^.*\[,*\]$");

        /// <summary>
        /// Resets the namespace table to its initial state.
        /// </summary>
        [Command("reset-namespaces", "Resets the namespace table to its initial state")]
        public static void ResetNamespaceTable()
        {
            _namespaceTable.Clear();
            _namespaceTable.AddRange(_defaultNamespaces);
        }

        /// <summary>
        /// Adds a namespace to the table so that it can be used to type resolution.
        /// </summary>
        [Command("use-namespace", "Adds a namespace to the table so that it can be used to type resolution")]
        public static void AddNamespace(string namespaceName)
        {
            if (!_namespaceTable.Contains(namespaceName))
            {
                _namespaceTable.Add(namespaceName);
            }
        }

        /// <summary>
        /// Removes a namespace to the table so that it is no longer used to type resolution.
        /// </summary>
        [Command("remove-namespace", "Removes a namespace from the table")]
        public static void RemoveNamespace(string namespaceName)
        {
            if (_namespaceTable.Contains(namespaceName))
            {
                _namespaceTable.Remove(namespaceName);
            }
            else
            {
                throw new ArgumentException($"No namespace named {namespaceName} was present in the table");
            }
        }

        [Command("all-namespaces", "Displays all of the namespaces currently in use by the namespace table")]
        private static string ShowNamespaces()
        {
            _namespaceTable.Sort();
            if (_namespaceTable.Count == 0) { return "Namespace table is empty"; }
            else { return string.Join("\n", _namespaceTable); }
        }

        /// <summary>
        /// Returns a copy of the namespace table.
        /// </summary>
        public static IEnumerable<string> GetAllNamespaces() { return _namespaceTable; }

        /// <summary>
        /// Parses and infers the type specified by the string.
        /// </summary>
        /// <returns>The parsed type.</returns>
        /// <param name="typeName">The type to parse.</param>
        public static Type ParseType(string typeName)
        {
            typeName = typeName.Trim();

            if (_reverseTypeDisplayNames.ContainsKey(typeName))
            {
                return _reverseTypeDisplayNames[typeName];
            }

            if (_arrayTypeRegex.IsMatch(typeName))
            {
                int arrayPos = typeName.LastIndexOf('[');
                int arrayRank = typeName.CountFromIndex(',', arrayPos) + 1;
                Type elementType = ParseType(typeName.Substring(0, arrayPos));

                if (arrayRank > 1)
                {
                    return elementType.MakeArrayType(arrayRank);
                }
                else
                {
                    return elementType.MakeArrayType();
                }
            }

            if (typeName.EndsWith(">") && typeName.Contains("<"))
            {
                string[] parts = typeName.Split(new char[] { '<' }, 2);
                string[] genericArgNames = $"<{parts[1]}".ReduceScope('<', '>').SplitScoped(',');
                string incompleteGenericName = $"{parts[0]}`{Math.Max(1, genericArgNames.Length)}";

                Type incompleteGenericType = ParseType(incompleteGenericName);
                if (genericArgNames.All(string.IsNullOrWhiteSpace)) { return incompleteGenericType; }
                Type[] genericArgs = new Type[genericArgNames.Length];
                for (int i = 0; i < genericArgs.Length; i++) { genericArgs[i] = ParseType(genericArgNames[i]); }

                return incompleteGenericType.MakeGenericType(genericArgs);
            }

            if (typeName.Contains('`'))
            {
                string genericName = typeName.Split('`')[0];
                if (_reverseTypeDisplayNames.ContainsKey(genericName))
                {
                    return _reverseTypeDisplayNames[genericName];
                }
            }

            return ParseTypeBaseCase(typeName);
        }

        private static Type ParseTypeBaseCase(string typeName)
        {
            Type loadedType = null;
            loadedType = loadedType ?? GetTypeFromAssemblies(typeName, _loadedAssemblies, false, false);
            loadedType = loadedType ?? GetTypeFromAssemblies(typeName, _namespaceTable, _loadedAssemblies, false, false);
            loadedType = loadedType ?? GetTypeFromAssemblies(typeName, _loadedAssemblies, false, true);
            loadedType = loadedType ?? GetTypeFromAssemblies(typeName, _namespaceTable, _loadedAssemblies, true, true);
            return loadedType;
        }

        private static Type GetTypeFromAssemblies(string typeName, IEnumerable<string> namespaces, IEnumerable<Assembly> assemblies, bool throwOnError, bool ignoreCase)
        {
            foreach (string namespaceName in namespaces)
            {
                Type type = GetTypeFromAssemblies($"{namespaceName}.{typeName}", assemblies, false, ignoreCase);
                if (type != null) { return type; }
            }

            if (throwOnError) { throw new TypeLoadException($"No type of name '{typeName}' could be found in the specified assemblies and namespaces."); }
            else { return null; }
        }

        private static Type GetTypeFromAssemblies(string typeName, IEnumerable<Assembly> assemblies, bool throwOnError, bool ignoreCase)
        {
            foreach (Assembly assembly in assemblies)
            {
                Type type = Type.GetType($"{typeName}, {assembly.FullName}", false, ignoreCase);
                if (type != null) { return type; }
            }

            if (throwOnError) { throw new TypeLoadException($"No type of name '{typeName}' could be found in the specified assemblies."); }
            else { return null; }
        }
        #endregion

        #region Legacy
        private static QuantumParser _parserObsolete;

        [Obsolete("Use an instance of QuantumParser instead")]
        public static object Parse(Type type, string data)
        {
            if (_parserObsolete == null) { _parserObsolete = new QuantumParser(); }
            return _parserObsolete.Parse(data, type);
        }

        [Obsolete("Use the extension method GetDisplayName from ReflectionExtensions in QFSW.QC.Utilities instead")]
        public static string GetDisplayName(Type type)
        {
            return type.GetDisplayName();
        }

        private static QuantumSerializer _serializerObsolete;

        [Obsolete("Use QuantumSerializer.SerializeFormatted instead")]
        public static string SerializeFormatted(object data, QuantumTheme theme = null)
        {
            if (_serializerObsolete == null) { _serializerObsolete = new QuantumSerializer(); }
            return _serializerObsolete.SerializeFormatted(data, theme);
        }
        #endregion
    }
}
