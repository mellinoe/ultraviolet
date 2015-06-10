﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace TwistedLogik.Nucleus.Data
{
    /// <summary>
    /// Maintains the application's collection of data object registries.
    /// </summary>
    [CLSCompliant(false)]
    public static class DataObjectRegistries
    {
        /// <summary>
        /// Gets the data object registry for the specified object type.
        /// </summary>
        /// <typeparam name="T">The type of object for which to retrieve a registry.</typeparam>
        /// <returns>The <see cref="DataObjectRegistry{T}"/> which contains objects of type <typeparamref name="T"/>.</returns>
        public static DataObjectRegistry<T> Get<T>() where T : DataObject
        {
            IDataObjectRegistry registry;
            if (!registries.TryGetValue(typeof(T), out registry))
            {
                throw new InvalidOperationException(NucleusStrings.DataObjectRegistryDoesNotExist.Format(typeof(T).FullName));
            }
            return (DataObjectRegistry<T>)registry;
        }

        /// <summary>
        /// Registers the data object registries in the specified assembly.
        /// </summary>
        /// <param name="asm">The assembly that contains the registries to register.</param>
        public static void Register(Assembly asm)
        {
            Contract.Require(asm, "asm");

            var types = asm.GetTypes();
            var typesOfRegistry = from t in types
                                  where
                                   t.GetInterface(typeof(IDataObjectRegistry).FullName) != null && !t.GetTypeInfo().IsAbstract
                                  select t;

            foreach (var type in typesOfRegistry)
            {
                var instance = (IDataObjectRegistry)Activator.CreateInstance(type);
                var elementType = GetRegistryElementType(instance);
                if (elementType == null)
                    continue;

                if (registries.ContainsKey(elementType))
                {
                    throw new InvalidOperationException(NucleusStrings.DataObjectRegistryAlreadyExists.Format(elementType.Name));
                }
                registries[elementType] = instance;

                var rrname = instance.ReferenceResolutionName;
                if (!String.IsNullOrEmpty(rrname))
                {
                    registriesByName[rrname.ToLower()] = instance;
                }

                instance.Register();
            }
        }

        /// <summary>
        /// Removes the data object registry for the specified type from the collection of registries.
        /// </summary>
        /// <typeparam name="T">The type of data object for which to unregister a registry.</typeparam>
        public static void Unregister<T>() where T : DataObject
        {
            IDataObjectRegistry registry;
            if (registries.TryGetValue(typeof(T), out registry))
            {
                registry.Unregister();

                registries.Remove(typeof(T));
                registriesByName.Remove(registry.ReferenceResolutionName.ToLower());
            }
        }

        /// <summary>
        /// Removes all objects from all of the application's data object registries 
        /// and returns them to their default states.
        /// </summary>
        public static void Clear()
        {
            foreach (var registry in registries)
            {
                registry.Value.Clear();
            }
        }

        /// <summary>
        /// Loads all of the application's registered data object registries.
        /// </summary>
        public static void Load()
        {
            LoadKeys();
            LoadObjects();
        }

        /// <summary>
        /// Loads object keys for all of the application's registered data object registries.
        /// </summary>
        public static void LoadKeys()
        {
            foreach (var registry in registries)
            {
                registry.Value.LoadKeys();
            }
        }

        /// <summary>
        /// Loads objects for all of the application's registered data object registries.
        /// </summary>
        public static void LoadObjects()
        {
            foreach (var registry in registries)
            {
                registry.Value.LoadObjects();
            }
        }

        /// <summary>
        /// Resolves an object reference. If the reference cannot be resolved, an exception is thrown.
        /// </summary>
        /// <param name="reference">The object reference to resolve.</param>
        /// <returns>The resolved object reference.</returns>
        public static ResolvedDataObjectReference ResolveReference(String reference)
        {
            if (reference == null)
                throw new ArgumentNullException("reference");

            if (reference.StartsWith("@"))
            {
                if (reference == "@INVALID")
                {
                    return ResolvedDataObjectReference.Invalid;
                }

                var ix = reference.IndexOf(":");
                if (ix > 0)
                {
                    var type = reference.Substring(1, ix - 1).ToLower();
                    var key = reference.Substring(ix + 1);

                    IDataObjectRegistry registry;
                    if (!registriesByName.TryGetValue(type, out registry))
                    {
                        throw new InvalidDataException(NucleusStrings.InvalidDataObjectReference.Format(reference));
                    }

                    var globalID = registry.ResolveObjectKey(key);
                    if (globalID == null)
                    {
                        throw new InvalidDataException(NucleusStrings.InvalidDataObjectReference.Format(reference));
                    }
                    return new ResolvedDataObjectReference(globalID.Value, reference.Substring(0, ix + 1) + key);
                }
            }
            return new ResolvedDataObjectReference(new Guid(reference), null);
        }

        /// <summary>
        /// Gets the element type of the specified registry.
        /// </summary>
        /// <param name="registry">The registry to evaluate.</param>
        /// <returns>The element type of the specified registry.</returns>
        private static Type GetRegistryElementType(IDataObjectRegistry registry)
        {
            var ancestor = registry.GetType().GetTypeInfo().BaseType;
            while (ancestor != null)
            {
                if (ancestor.GetTypeInfo().IsGenericType && ancestor.GetGenericTypeDefinition() == typeof(DataObjectRegistry<>))
                {
                    return ancestor.GetGenericArguments()[0];
                }
                ancestor = ancestor.GetTypeInfo().BaseType;
            }
            return null;
        }

        // The game's list of object registries.
        private static readonly Dictionary<Type, IDataObjectRegistry> registries = new Dictionary<Type, IDataObjectRegistry>();
        private static readonly Dictionary<String, IDataObjectRegistry> registriesByName = new Dictionary<String, IDataObjectRegistry>();
    }
}
