using QFSW.QC.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QFSW.QC
{
    public static class QuantumRegistry
    {
        private static readonly Dictionary<Type, List<object>> _objectRegistry = new Dictionary<Type, List<object>>();

        /// <summary>Adds the object to the registry.</summary>
        /// <param name="obj">The object to add to the registry.</param>
        /// <typeparam name="T">The type of the object to add to the registry.</typeparam>
        [Command("register-object", "Adds the object to the registry to be used by commands with MonoTargetType = Registry")]
        public static void RegisterObject<T>(T obj) where T : class { RegisterObject(typeof(T), obj); }

        /// <summary>Adds the object to the registry.</summary>
        /// <param name="type">The type of the object to add to the registry.</param>
        /// <param name="obj">The object to add to the registry.</param>
        public static void RegisterObject(Type type, object obj)
        {
            if (!type.IsClass) { throw new Exception("Registry may only contain class types"); }
            lock (_objectRegistry)
            {
                if (_objectRegistry.ContainsKey(type))
                {
                    if (_objectRegistry[type].Contains(obj)) { throw new ArgumentException($"Could not register object '{obj}' of type {type.GetDisplayName()} as it was already registered."); }
                    else { _objectRegistry[type].Add(obj); }
                }
                else { _objectRegistry.Add(type, new List<object>() { obj }); }
            }
        }

        [Obsolete("Use DeregisterObject instead")]
        public static void DeRegisterObject<T>(T obj) where T : class { DeregisterObject(obj); }

        [Obsolete("Use DeregisterObject instead")]
        public static void DeRegisterObject(Type type, object obj) { DeregisterObject(type, obj); }

        /// <summary>Removes the object from the registry.</summary>
        /// <param name="obj">The object to remove from the registry.</param>
        /// <typeparam name="T">The type of the object to remove from the registry.</typeparam>
        [Command("deregister-object", "Removes the object to the registry to be used by commands with MonoTargetType = Registry")]
        public static void DeregisterObject<T>(T obj) where T : class { DeregisterObject(typeof(T), obj); }

        /// <summary>Removes the object to the registry.</summary>
        /// <param name="type">The type of the object to remove from the registry.</param>
        /// <param name="obj">The object to remove from the registry.</param>
        public static void DeregisterObject(Type type, object obj)
        {
            if (!type.IsClass) { throw new Exception("Registry may only contain class types"); }
            lock (_objectRegistry)
            {
                if (_objectRegistry.ContainsKey(type) && _objectRegistry[type].Contains(obj)) { _objectRegistry[type].Remove(obj); }
                else { throw new ArgumentException($"Could not deregister object '{obj}' of type {type.GetDisplayName()} as it was not found in the registry."); }
            }
        }

        /// <summary>Gets the size of the specified registry.</summary>
        /// <returns>The registry size.</returns>
        /// <typeparam name="T">The registry to query.</typeparam>
        public static int GetRegistrySize<T>() where T : class { return GetRegistrySize(typeof(T)); }

        /// <summary>Gets the size of the specified registry.</summary>
        /// <returns>The registry size.</returns>
        /// <param name="type">The registry to query.</param>
        public static int GetRegistrySize(Type type)
        {
            if (!type.IsClass) { throw new Exception("Registry may only contain class types"); }
            lock (_objectRegistry)
            {
                if (_objectRegistry.ContainsKey(type))
                {
                    _objectRegistry[type].RemoveAll(x => x == null);
                    return _objectRegistry[type].Count;
                }
                else { return 0; }
            }
        }

        /// <summary>Gets the contents of the specified registry.</summary>
        /// <returns>The registry contents.</returns>
        /// <typeparam name="T">The registry to query.</typeparam>
        public static IEnumerable<T> GetRegistryContents<T>() where T : class
        {
            foreach (object obj in GetRegistryContents(typeof(T)))
            {
                yield return (T)obj;
            }
        }

        /// <summary>Gets the contents of the specified registry.</summary>
        /// <returns>The registry contents.</returns>
        /// <param name="type">The registry to query.</param>
        public static IEnumerable<object> GetRegistryContents(Type type)
        {
            if (!type.IsClass) { throw new Exception("Registry may only contain class types"); }
            lock (_objectRegistry)
            {
                if (_objectRegistry.ContainsKey(type)) { return _objectRegistry[type]; }
                else { return Enumerable.Empty<object>(); }
            }
        }

        [Command("display-registry", "Displays the contents of the specified registry")]
        private static string DisplayRegistry([CommandParameterDescription("Full namespace qualified name of the type of the registry.")]Type type)
        {
            int registrySize = GetRegistrySize(type);
            if (registrySize < 1) { return $"registry '{type.GetDisplayName()}' was empty"; }
            else { return string.Join("\n", GetRegistryContents(type).Select(x => x.ToString())); }
        }
    }
}