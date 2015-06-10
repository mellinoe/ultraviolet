﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Xml.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TwistedLogik.Nucleus.Data
{
    /// <summary>
    /// Represents a method which can be passed to the <see cref="ObjectLoader"/> to allow external code
    /// to resolve property values.
    /// </summary>
    /// <param name="obj">The object which is being populated.</param>
    /// <param name="name">The name of the property which is being populated.</param>
    /// <param name="value">The value which is being resolved.</param>
    /// <returns><c>true</c> if the property was resolved; otherwise, <c>false</c>.</returns>
    public delegate Boolean ObjectLoaderMemberResolutionHandler(Object obj, String name, String value);

    /// <summary>
    /// Contains methods for loading object definitions from data files.
    /// </summary>
    [CLSCompliant(false)]
    public static class ObjectLoader
    {
        /// <summary>
        /// Gets a value indicating whether the specified name is a reserved keyword used by the object loading system.
        /// </summary>
        /// <param name="name">The name to evaluate.</param>
        /// <returns><c>true</c> if the name is reserved; otherwise, <c>false</c>.</returns>
        public static Boolean IsReservedKeyword(String name)
        {
            switch (name)
            {
                case "Class":
                case "Key":
                case "ID":
                case "Type":
                case "Constructor":
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the specified name is a forbidden keyword which cannot be used in an object
        /// definition under any circumstance.
        /// </summary>
        /// <param name="name">The name to evaluate.</param>
        /// <returns><c>true</c> if the name is forbidden; otherwise, <c>false</c>.</returns>
        public static Boolean IsForbiddenKeyword(String name)
        {
            switch(name)
            {
                case "Key":
                case "Constructor":
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Loads the objects defined in the specified file.
        /// </summary>
        /// <typeparam name="T">The type of object to load.</typeparam>
        /// <param name="datatype">The type of data contained in the specified file.</param>
        /// <param name="path">The path of the file that contains the data definitions to load.</param>
        /// <param name="name">The name of the type of object to load.  Corresponds to the names of the elements in the XML file.</param>
        /// <param name="defaultClass">The default class if no default is specified in the file.</param>
        /// <returns>A collection containing the objects that were loaded.</returns>
        public static IEnumerable<T> LoadDefinitions<T>(ObjectLoaderDataType datatype, String path, String name, Type defaultClass = null) where T : DataObject
        {
            Contract.Require(path, "path");
            Contract.Require(name, "name");

            // If requested, determine the data type based on the file extension.
            if (datatype == ObjectLoaderDataType.Detect)
            {
                var extension = Path.GetExtension(path).ToUpper();
                switch (extension)
                {
                    case ".XML":
                        datatype = ObjectLoaderDataType.Xml;
                        break;

                    case ".JSON":
                    case ".JS":
                        datatype = ObjectLoaderDataType.Json;
                        break;

                    default:
                        throw new InvalidOperationException(NucleusStrings.UnableToDetectDataTypeFromExt);
                }
            }

            // Load the item definitions.
            using (var stream = File.OpenRead(path))
            {
                switch (datatype)
                {
                    case ObjectLoaderDataType.Xml:
                        return LoadDefinitions<T>(XDocument.Load(stream), name, defaultClass);

                    case ObjectLoaderDataType.Json:
                        using (var sreader = new StreamReader(stream))
                        using (var jreader = new JsonTextReader(sreader))
                        {
                            return LoadDefinitions<T>(JObject.Load(jreader), name, defaultClass);
                        }
                }
            }
            throw new InvalidOperationException(NucleusStrings.UnrecognizedDataType);
        }

        /// <summary>
        /// Loads object definitions from the specified XML file and adds them to the specified object registry.
        /// </summary>
        /// <typeparam name="T">The type of object to load.</typeparam>
        /// <param name="xml">The XML document that contains the object definitions to load.</param>
        /// <param name="name">The name of the type of object to load.  Corresponds to the names of the elements in the data file.</param>
        /// <param name="defaultClass">The default class if no default is specified in the file.</param>
        /// <returns>A collection containing the objects that were loaded.</returns>
        public static IEnumerable<T> LoadDefinitions<T>(XDocument xml, String name, Type defaultClass = null) where T : DataObject
        {
            Contract.Require(xml, "xml");
            Contract.Require(name, "name");

            var root = DataElement.CreateFromXml(xml);
            var aliases = root.Element("Aliases");
            var defaults = root.Element("Defaults");
            return LoadDefinitions<T>(root, 
                (aliases == null) ? Enumerable.Empty<DataElement>() : aliases.Elements("Alias"),
                (defaults == null) ? Enumerable.Empty<DataElement>() : defaults.Elements("Default"), name, defaultClass);
        }

        /// <summary>
        /// Loads object definitions from the specified JSON file and adds them to the specified object registry.
        /// </summary>
        /// <typeparam name="T">The type of object to load.</typeparam>
        /// <param name="json">The JSON document that contains the object definitions to load.</param>
        /// <param name="name">The name of the type of object to load.  Corresponds to the names of the elements in the data file.</param>
        /// <param name="defaultClass">The default class if no default is specified in the file.</param>
        /// <returns>A collection containing the objects that were loaded.</returns>
        public static IEnumerable<T> LoadDefinitions<T>(JObject json, String name, Type defaultClass = null) where T : DataObject
        {
            Contract.Require(json, "json");
            Contract.Require(name, "name");

            var root = DataElement.CreateFromJson(json);
            var aliases = root.Elements("Aliases");
            var defaults = root.Elements("Defaults");
            return LoadDefinitions<T>(root, aliases, defaults, name, defaultClass);
        }

        /// <summary>
        /// Loads an object from the specified XML element.
        /// </summary>
        /// <typeparam name="T">The type of object to load.</typeparam>
        /// <param name="xml">The XML element that contains the object data.</param>
        /// <param name="ignoreMissingMembers">A value indicating whether the object loader 
        /// should ignore members which do not exist on the type.</param>
        /// <returns>The object that was loaded.</returns>
        public static T LoadObject<T>(XElement xml, Boolean ignoreMissingMembers = false)
        {
            return (T)LoadObject(null, typeof(T), xml, CultureInfo.InvariantCulture, ignoreMissingMembers);
        }

        /// <summary>
        /// Loads an object from the specified XML element.
        /// </summary>
        /// <typeparam name="T">The type of object to load.</typeparam>
        /// <param name="resolver">A custom <see cref="ObjectLoaderMemberResolutionHandler"/> which allows external
        /// code to optionally resolve deserialized member values.</param>
        /// <param name="xml">The XML element that contains the object data.</param>
        /// <param name="ignoreMissingMembers">A value indicating whether the object loader 
        /// should ignore members which do not exist on the type.</param>
        /// <returns>The object that was loaded.</returns>
        public static T LoadObject<T>(ObjectLoaderMemberResolutionHandler resolver, XElement xml, Boolean ignoreMissingMembers = false)
        {
            return (T)LoadObject(resolver, typeof(T), xml, CultureInfo.InvariantCulture, ignoreMissingMembers);
        }

        /// <summary>
        /// Loads an object from the specified XML element.
        /// </summary>
        /// <param name="type">The type of object to load.</param>
        /// <param name="xml">The XML element that contains the object data.</param>
        /// <param name="ignoreMissingMembers">A value indicating whether the object loader 
        /// should ignore members which do not exist on the type.</param>
        /// <returns>The object that was loaded.</returns>
        public static Object LoadObject(Type type, XElement xml, Boolean ignoreMissingMembers = false)
        {
            return LoadObject(null, type, xml, CultureInfo.InvariantCulture, ignoreMissingMembers);
        }

        /// <summary>
        /// Loads an object from the specified XML element.
        /// </summary>
        /// <param name="resolver">A custom <see cref="ObjectLoaderMemberResolutionHandler"/> which allows external
        /// code to optionally resolve deserialized member values.</param>
        /// <param name="type">The type of object to load.</param>
        /// <param name="xml">The XML element that contains the object data.</param>
        /// <param name="ignoreMissingMembers">A value indicating whether the object loader 
        /// should ignore members which do not exist on the type.</param>
        /// <returns>The object that was loaded.</returns>
        public static Object LoadObject(ObjectLoaderMemberResolutionHandler resolver, Type type, XElement xml, Boolean ignoreMissingMembers = false)
        {
            return LoadObject(resolver, type, xml, CultureInfo.InvariantCulture, ignoreMissingMembers);
        }

        /// <summary>
        /// Loads an object from the specified XML element.
        /// </summary>
        /// <typeparam name="T">The type of object to load.</typeparam>
        /// <param name="xml">The XML element that contains the object data.</param>
        /// <param name="culture">The culture information to use when parsing values.</param>
        /// <param name="ignoreMissingMembers">A value indicating whether the object loader 
        /// should ignore members which do not exist on the type.</param>
        /// <returns>The object that was loaded.</returns>
        public static T LoadObject<T>(XElement xml, CultureInfo culture, Boolean ignoreMissingMembers = false)
        {
            return (T)LoadObject(null, typeof(T), xml, culture, ignoreMissingMembers);
        }

        /// <summary>
        /// Loads an object from the specified XML element.
        /// </summary>
        /// <typeparam name="T">The type of object to load.</typeparam>
        /// <param name="resolver">A custom <see cref="ObjectLoaderMemberResolutionHandler"/> which allows external
        /// code to optionally resolve deserialized member values.</param>
        /// <param name="xml">The XML element that contains the object data.</param>
        /// <param name="culture">The culture information to use when parsing values.</param>
        /// <param name="ignoreMissingMembers">A value indicating whether the object loader 
        /// should ignore members which do not exist on the type.</param>
        /// <returns>The object that was loaded.</returns>
        public static T LoadObject<T>(ObjectLoaderMemberResolutionHandler resolver, XElement xml, CultureInfo culture, Boolean ignoreMissingMembers = false)
        {
            return (T)LoadObject(resolver, typeof(T), xml, culture, ignoreMissingMembers);
        }

        /// <summary>
        /// Loads an object from the specified XML element.
        /// </summary>
        /// <param name="type">The type of object to load.</param>
        /// <param name="xml">The XML element that contains the object data.</param>
        /// <param name="culture">The culture information to use when parsing values.</param>
        /// <param name="ignoreMissingMembers">A value indicating whether the object loader 
        /// should ignore members which do not exist on the type.</param>
        /// <returns>The object that was loaded.</returns>
        public static Object LoadObject(Type type, XElement xml, CultureInfo culture, Boolean ignoreMissingMembers = false)
        {
            return LoadObject(null, type, xml, culture, ignoreMissingMembers);
        }

        /// <summary>
        /// Loads an object from the specified XML element.
        /// </summary>
        /// <param name="resolver">A custom <see cref="ObjectLoaderMemberResolutionHandler"/> which allows external
        /// code to optionally resolve deserialized member values.</param>
        /// <param name="type">The type of object to load.</param>
        /// <param name="xml">The XML element that contains the object data.</param>
        /// <param name="culture">The culture information to use when parsing values.</param>
        /// <param name="ignoreMissingMembers">A value indicating whether the object loader 
        /// should ignore members which do not exist on the type.</param>
        /// <returns>The object that was loaded.</returns>
        public static Object LoadObject(ObjectLoaderMemberResolutionHandler resolver, Type type, XElement xml, CultureInfo culture, Boolean ignoreMissingMembers = false)
        {
            Contract.Require(type, "type");
            Contract.Require(xml, "xml");
            Contract.Require(culture, "culture");

            var state = new ObjectLoaderState(globalAliases, culture, resolver);
            state.IgnoreMissingMembers = ignoreMissingMembers;
            state.ParseClassAliases(null, type);

            var objectElement = DataElement.CreateFromXml(xml);
            var objectInstance = CreateObjectFromRootElement(state, type, objectElement);

            return PopulateObject(state, objectInstance, objectElement);
        }

        /// <summary>
        /// Loads an object from the specified JSON object.
        /// </summary>
        /// <typeparam name="T">The type of object to load.</typeparam>
        /// <param name="json">The JSON object that contains the object data.</param>
        /// <param name="ignoreMissingMembers">A value indicating whether the object loader 
        /// should ignore members which do not exist on the type.</param>
        /// <returns>The object that was loaded.</returns>
        public static T LoadObject<T>(JObject json, Boolean ignoreMissingMembers = false)
        {
            return (T)LoadObject(null, typeof(T), json, CultureInfo.InvariantCulture, ignoreMissingMembers);
        }

        /// <summary>
        /// Loads an object from the specified JSON object.
        /// </summary>
        /// <typeparam name="T">The type of object to load.</typeparam>
        /// <param name="resolver">A custom <see cref="ObjectLoaderMemberResolutionHandler"/> which allows external
        /// code to optionally resolve deserialized member values.</param>
        /// <param name="json">The JSON object that contains the object data.</param>
        /// <param name="ignoreMissingMembers">A value indicating whether the object loader 
        /// should ignore members which do not exist on the type.</param>
        /// <returns>The object that was loaded.</returns>
        public static T LoadObject<T>(ObjectLoaderMemberResolutionHandler resolver, JObject json, Boolean ignoreMissingMembers = false)
        {
            return (T)LoadObject(resolver, typeof(T), json, CultureInfo.InvariantCulture, ignoreMissingMembers);
        }

        /// <summary>
        /// Loads an object from the specified JSON object.
        /// </summary>
        /// <param name="type">The type of object to load.</param>
        /// <param name="json">The JSON object that contains the object data.</param>
        /// <param name="ignoreMissingMembers">A value indicating whether the object loader 
        /// should ignore members which do not exist on the type.</param>
        /// <returns>The object that was loaded.</returns>
        public static Object LoadObject(Type type, JObject json, Boolean ignoreMissingMembers = false)
        {
            return LoadObject(null, type, json, CultureInfo.InvariantCulture, ignoreMissingMembers);
        }

        /// <summary>
        /// Loads an object from the specified JSON object.
        /// </summary>
        /// <param name="resolver">A custom <see cref="ObjectLoaderMemberResolutionHandler"/> which allows external
        /// code to optionally resolve deserialized member values.</param>
        /// <param name="type">The type of object to load.</param>
        /// <param name="json">The JSON object that contains the object data.</param>
        /// <param name="ignoreMissingMembers">A value indicating whether the object loader 
        /// should ignore members which do not exist on the type.</param>
        /// <returns>The object that was loaded.</returns>
        public static Object LoadObject(ObjectLoaderMemberResolutionHandler resolver, Type type, JObject json, Boolean ignoreMissingMembers = false)
        {
            return LoadObject(resolver, type, json, CultureInfo.InvariantCulture, ignoreMissingMembers);
        }

        /// <summary>
        /// Loads an object from the specified JSON object.
        /// </summary>
        /// <typeparam name="T">The type of object to load.</typeparam>
        /// <param name="json">The JSON object that contains the object data.</param>
        /// <param name="culture">The culture information to use when parsing values.</param>
        /// <param name="ignoreMissingMembers">A value indicating whether the object loader 
        /// should ignore members which do not exist on the type.</param>
        /// <returns>The object that was loaded.</returns>
        public static T LoadObject<T>(JObject json, CultureInfo culture, Boolean ignoreMissingMembers = false)
        {
            return (T)LoadObject(null, typeof(T), json, culture, ignoreMissingMembers);
        }

        /// <summary>
        /// Loads an object from the specified JSON object.
        /// </summary>
        /// <typeparam name="T">The type of object to load.</typeparam>
        /// <param name="resolver">A custom <see cref="ObjectLoaderMemberResolutionHandler"/> which allows external
        /// code to optionally resolve deserialized member values.</param>
        /// <param name="json">The JSON object that contains the object data.</param>
        /// <param name="culture">The culture information to use when parsing values.</param>
        /// <param name="ignoreMissingMembers">A value indicating whether the object loader 
        /// should ignore members which do not exist on the type.</param>
        /// <returns>The object that was loaded.</returns>
        public static T LoadObject<T>(ObjectLoaderMemberResolutionHandler resolver, JObject json, CultureInfo culture, Boolean ignoreMissingMembers = false)
        {
            return (T)LoadObject(resolver, typeof(T), json, culture, ignoreMissingMembers);
        }

        /// <summary>
        /// Loads an object from the specified JSON object.
        /// </summary>
        /// <param name="type">The type of object to load.</param>
        /// <param name="json">The JSON object that contains the object data.</param>
        /// <param name="culture">The culture information to use when parsing values.</param>
        /// <param name="ignoreMissingMembers">A value indicating whether the object loader 
        /// should ignore members which do not exist on the type.</param>
        /// <returns>The object that was loaded.</returns>
        public static Object LoadObject(Type type, JObject json, CultureInfo culture, Boolean ignoreMissingMembers = false)
        {
            return LoadObject(null, type, json, culture, ignoreMissingMembers);
        }

        /// <summary>
        /// Loads an object from the specified JSON object.
        /// </summary>
        /// <param name="resolver">A custom <see cref="ObjectLoaderMemberResolutionHandler"/> which allows external
        /// code to optionally resolve deserialized member values.</param>
        /// <param name="type">The type of object to load.</param>
        /// <param name="json">The JSON object that contains the object data.</param>
        /// <param name="culture">The culture information to use when parsing values.</param>
        /// <param name="ignoreMissingMembers">A value indicating whether the object loader 
        /// should ignore members which do not exist on the type.</param>
        /// <returns>The object that was loaded.</returns>
        public static Object LoadObject(ObjectLoaderMemberResolutionHandler resolver, Type type, JObject json, CultureInfo culture, Boolean ignoreMissingMembers = false)
        {
            Contract.Require(type, "type");
            Contract.Require(json, "json");
            Contract.Require(culture, "culture");

            var state = new ObjectLoaderState(globalAliases, culture, resolver);
            state.IgnoreMissingMembers = ignoreMissingMembers;
            state.ParseClassAliases(null, type);

            var objectElement = DataElement.CreateFromJson(json);
            var objectInstance = CreateObjectFromRootElement(state, type, objectElement);

            return PopulateObject(state, objectInstance, objectElement);
        }

        /// <summary>
        /// Registers a globally-available type alias.
        /// </summary>
        /// <param name="alias">The alias to assign to the type.</param>
        /// <param name="type">The type for which to create an alias.</param>
        public static void RegisterGlobalAlias(String alias, Type type)
        {
            RegisterGlobalAlias(alias, type.AssemblyQualifiedName);
        }

        /// <summary>
        /// Registers a globally-available type alias.
        /// </summary>
        /// <param name="alias">The alias to assign to the type.</param>
        /// <param name="type">The assembly-qualified name of the type for which to create an alias.</param>
        public static void RegisterGlobalAlias(String alias, String type)
        {
            try
            {
                lockobj.EnterWriteLock();
                if (globalAliases.ContainsKey(alias))
                {
                    throw new InvalidOperationException(NucleusStrings.TypeAliasAlreadyRegistered.Format(alias));
                }
                globalAliases[alias] = type;
            }
            finally
            {
                if (lockobj.IsWriteLockHeld)
                {
                    lockobj.ExitWriteLock();
                }
            }
        }

        /// <summary>
        /// Unregisters a globally-available type alias.
        /// </summary>
        /// <param name="alias">The alias to unregister.</param>
        /// <returns><c>true</c> if the alias was unregistered; otherwise, <c>false</c>.</returns>
        public static bool UnregisterGlobalAlias(String alias)
        {
            try
            {
                lockobj.EnterWriteLock();
                return globalAliases.Remove(alias);
            }
            finally
            {
                if (lockobj.IsWriteLockHeld)
                {
                    lockobj.ExitWriteLock();
                }
            }
        }

        /// <summary>
        /// Loads object definitions from the specified data hierarchy and adds them to the specified object registry.
        /// </summary>
        /// <typeparam name="T">The type of object to load.</typeparam>
        /// <param name="root">The root data element of the document that contains the object definitions to load.</param>
        /// <param name="aliases">The elements defining class aliases.</param>
        /// <param name="defaults">The elements defining class defaults.</param>
        /// <param name="defaultClass">The name of the default class to apply to loaded objects.</param>
        /// <param name="name">The name of the type of object to load.  Corresponds to the names of the elements in the data file.</param>
        private static IEnumerable<T> LoadDefinitions<T>(DataElement root, IEnumerable<DataElement> aliases, IEnumerable<DataElement> defaults, String name, Type defaultClass) where T : DataObject
        {
            Contract.Require(root, "root");
            Contract.Require(name, "name");

            var cultureString = root.AttributeValue<String>("Culture");
            var culture       = String.IsNullOrWhiteSpace(cultureString) ? CultureInfo.InvariantCulture : new CultureInfo(cultureString);
            
            try
            {
                lockobj.EnterReadLock();

                var state = new ObjectLoaderState(globalAliases, culture);
                state.ParseClassAliases(aliases, defaultClass);
                state.ParseClassDefaults(defaults);

                var objectElements = root.Elements(name);
                var objectList = new List<T>();
                foreach (var objectElement in objectElements)
                {
                    var objectInstance = (T)CreateObjectFromRootElement(state, typeof(T), objectElement);
                    PopulateObject(state, objectInstance, objectElement);
                    objectList.Add(objectInstance);
                }
                return objectList;
            }
            finally
            {
                if (lockobj.IsReadLockHeld)
                {
                    lockobj.ExitReadLock();
                }
            }
        }

        /// <summary>
        /// Creates an object from the specified root element.
        /// </summary>
        /// <param name="state">The current loader state.</param>
        /// <param name="type">The type of object to create.</param>
        /// <param name="element">The element from which to create an object.</param>
        /// <returns>The object that was created.</returns>
        private static Object CreateObjectFromRootElement(ObjectLoaderState state, Type type, DataElement element)
        {
            // First, ensure that we have a class, key, and identifier.
            var objClassName = state.ResolveClass(element.AttributeValue<String>("Class"));
            if (String.IsNullOrEmpty(objClassName))
                throw new InvalidOperationException(NucleusStrings.DataObjectMissingClass);
            
            // If we're loading a Nucleus data object, parse its unique key and ID.
            var argsBase = default(Object[]);
            if (typeof(DataObject).IsAssignableFrom(type))
            {
                var objKey = element.AttributeValue<String>("Key");
                if (String.IsNullOrEmpty(objKey))
                    throw new InvalidOperationException(NucleusStrings.DataObjectMissingKey);
                
                var objID = element.AttributeValue<String>("ID");
                if (String.IsNullOrEmpty(objID))
                    throw new InvalidOperationException(NucleusStrings.DataObjectMissingID);
                
                Guid objIDValue;
                if (!Guid.TryParse(objID, out objIDValue))
                    throw new InvalidOperationException(NucleusStrings.DataObjectInvalidID.Format(objID));
                
                 argsBase = new Object[] { objKey, objIDValue };
            }

            // Attempt to find the object class and make sure it's of the correct type.
            var objClass = Type.GetType(objClassName, false);
            if (objClass == null || !type.IsAssignableFrom(objClass))
                throw new InvalidOperationException(NucleusStrings.DataObjectInvalidClass.Format(objClassName ?? "(null)", argsBase[0]));
            
            // Attempt to instantiate the object.
            return CreateObject(state, objClass, argsBase, GetSpecifiedConstructorArguments(element));
        }

        /// <summary>
        /// Creates an object from the specified root element.
        /// </summary>
        /// <param name="state">The loader state.</param>
        /// <param name="type">The type of object to create.</param>
        /// <param name="argsBase">The base set of arguments for this object's constructor.</param>
        /// <param name="argsSpecified">The specified set of arguments for this object's constructor.</param>
        /// <returns>The object that was created.</returns>
        private static Object CreateObject(ObjectLoaderState state, Type type, Object[] argsBase, DataElement[] argsSpecified = null)
        {
            var argsBaseLength = (argsBase == null) ? 0 : argsBase.Length;
            var argsSpecifiedLength = (argsSpecified == null) ? 0 : argsSpecified.Length;

            // Try to find a constructor that matches our argument list.
            var ctorArgs = new Object[argsBaseLength + argsSpecifiedLength];
            var ctors = type.GetConstructors();
            if (!ctors.Any() && argsBaseLength == 0 && argsSpecifiedLength == 0)
            {
                return Activator.CreateInstance(type);
            }

            var ctorMatches = ctors.Where(x => x.GetParameters().Count() == ctorArgs.Length).ToList();
            if (!ctorMatches.Any())
                throw new InvalidOperationException(NucleusStrings.CtorMatchNotFound);
            if (ctorMatches.Count() > 1)
                throw new InvalidOperationException(NucleusStrings.CtorMatchIsAmbiguous);
            
            var ctorMatch = ctorMatches.Single();
            var ctorParams = ctorMatch.GetParameters();

            // Build the complete list of constructor arguments.
            for (int i = 0; i < argsBaseLength; i++)
            {
                ctorArgs[i] = argsBase[i];
            }
            for (int i = 0; i < argsSpecifiedLength; i++)
            {
                var ctorArgElement = argsSpecified[i];
                var ctorArgType = GetTypeFromElement(state, ctorParams[i + argsBase.Length].ParameterType, ctorArgElement);
                var ctorArgValue = default(Object);
                if (ctorArgElement.Elements().Any())
                {
                    ctorArgValue = CreateObject(state, ctorArgType, null, GetSpecifiedConstructorArguments(ctorArgElement));
                    ctorArgValue = PopulateObjectFromElements(state, ctorArgValue, ctorArgElement);
                }
                else
                {
                    ctorArgValue = ParseValue(ctorArgElement.Value, ctorArgType, state.Culture);
                }
                ctorArgs[i + argsBase.Length] = ctorArgValue;
            }

            // Attempt to instantiate the object.
            return ctorMatch.Invoke(ctorArgs);
        }

        /// <summary>
        /// Gets the specified constructor arguments for an element.
        /// </summary>
        /// <param name="element">The element to evaluate.</param>
        /// <returns>The specified constructor arguments for the element.</returns>
        private static DataElement[] GetSpecifiedConstructorArguments(DataElement element)
        {
            var ctorElement = element.Element("Constructor");
            if (ctorElement == null)
                return null;
            return ctorElement.Elements("Argument").ToArray();
        }

        /// <summary>
        /// Gets the type defined by the specified element.
        /// </summary>
        /// <param name="state">The loader state.</param>
        /// <param name="baseType">The base type.</param>
        /// <param name="element">The element to evaluate.</param>
        /// <returns>The type defined by the specified element.</returns>
        private static Type GetTypeFromElement(ObjectLoaderState state, Type baseType, DataElement element)
        {
            var complexTypeAttr = element.Attribute("Type");
            if (complexTypeAttr != null && String.IsNullOrEmpty(complexTypeAttr.Value))
                throw new InvalidOperationException(NucleusStrings.DataObjectInvalidType.Format(element.Name));

            var complexType = (complexTypeAttr == null) ? baseType : Type.GetType(state.ResolveClass(complexTypeAttr.Value), false);
            if (complexType == null)
                throw new InvalidOperationException(NucleusStrings.DataObjectInvalidType.Format(element.Name));
            
            if (!baseType.IsAssignableFrom(complexType))
                throw new InvalidOperationException(NucleusStrings.DataObjectIncompatibleType.Format(element.Name));
            
            return complexType;
        }

        /// <summary>
        /// Populates an object with values.
        /// </summary>
        /// <param name="state">The loader state.</param>
        /// <param name="objectInstance">The object instance.</param>
        /// <param name="objectElement">The object element.</param>
        /// <returns>The object instance.</returns>
        private static Object PopulateObject(ObjectLoaderState state, Object objectInstance, DataElement objectElement)
        {
            objectInstance = PopulateObjectFromDefaults(state, objectInstance, objectElement);
            objectInstance = PopulateObjectFromAttributes(state, objectInstance, objectElement);
            objectInstance = PopulateObjectFromElements(state, objectInstance, objectElement);
            return objectInstance;
        }

        /// <summary>
        /// Populates an object from the currently loaded default values.
        /// </summary>
        /// <param name="state">The loader state.</param>
        /// <param name="objectInstance">The object instance.</param>
        /// <param name="objectElement">The object element.</param>
        /// <returns>The object instance.</returns>
        private static Object PopulateObjectFromDefaults(ObjectLoaderState state, Object objectInstance, DataElement objectElement)
        {
            // Find our inheritance chain.
            var typeStack = new Stack<Type>();
            var typeCurrent = objectInstance.GetType();
            while (typeCurrent != null)
            {
                typeStack.Push(typeCurrent);
                typeCurrent = typeCurrent.BaseType;
            }

            // Populate the default values for the entire inheritance heirarchy.
            while (typeStack.Count > 0)
            {
                // Retrieve the defaults for the current type.
                var typeBeingPopulated = typeStack.Pop();
                var typeDefaults = state.GetDefaultValues(typeBeingPopulated);
                if (typeDefaults == null)
                    continue;

                // Populate the default values.
                foreach (var typeDefault in typeDefaults)
                {
                    objectInstance = PopulateMemberFromElement(state, objectInstance, typeDefault.Value);
                }
            }

            return objectInstance;
        }

        /// <summary>
        /// Populates an object from the attributes defined on the specified data element.
        /// </summary>
        /// <param name="state">The loader state.</param>
        /// <param name="objectInstance">The object instance.</param>
        /// <param name="objectElement">The object element.</param>
        /// <returns>The object instance.</returns>
        private static Object PopulateObjectFromAttributes(ObjectLoaderState state, Object objectInstance, DataElement objectElement)
        {
            foreach (var attr in objectElement.Attributes())
            {
                if (IsReservedKeyword(attr.Name))
                    continue;

                var attrMember = ObjectLoaderMember.Find(objectInstance, attr.Name, state.IgnoreMissingMembers);
                if (attrMember != null)
                {
                    if (state.Resolver != null && state.Resolver(objectInstance, attr.Name, attr.Value))
                        continue;

                    var attrValue = ObjectResolver.FromString(attr.Value, attrMember.MemberType);
                    attrMember.SetValue(attrValue, null);
                }
            }

            return objectInstance;
        }

        /// <summary>
        /// Populates an object from the elements descending from the specified data element.
        /// </summary>
        /// <param name="state">The loader state.</param>
        /// <param name="objectInstance">The object instance.</param>
        /// <param name="objectElement">The object element.</param>
        /// <param name="filter">A filter which determines which elements are used to populate the object.</param>
        /// <returns>The object instance.</returns>
        private static Object PopulateObjectFromElements(ObjectLoaderState state, Object objectInstance, DataElement objectElement, Func<DataElement, Boolean> filter = null)
        {
            var children = (filter == null) ? objectElement.Elements() : objectElement.Elements().Where(filter);
            foreach (var element in children)
            {
                objectInstance = PopulateMemberFromElement(state, objectInstance, element, false);
            }

            return objectInstance;
        }

        /// <summary>
        /// Populates an object member from the specified data element.
        /// </summary>
        /// <param name="state">The loader state.</param>
        /// <param name="objectInstance">The object instance.</param>
        /// <param name="memberElement">The element that defines the member.</param>
        /// <param name="skipReservedKeywords">A value indicating whether to skip elements with the same names as reserved keywords.</param>
        /// <returns>The object instance.</returns>
        private static Object PopulateMemberFromElement(ObjectLoaderState state, Object objectInstance, DataElement memberElement, Boolean skipReservedKeywords = true)
        {
            if (IsReservedKeyword(memberElement.Name))
            {
                if (skipReservedKeywords || IsForbiddenKeyword(memberElement.Name))
                {
                    return objectInstance;
                }
            }

            if (state.Resolver != null && state.Resolver(objectInstance, memberElement.Name, memberElement.Value))
                return objectInstance;

            var member = ObjectLoaderMember.Find(objectInstance, memberElement.Name, state.IgnoreMissingMembers);
            if (member == null)
                return objectInstance;

            // Handle array values.
            if (member.MemberType.IsArray)
            {
                PopulateArray(state, member, memberElement);
                return objectInstance;
            }

            // Handle list values.
            if (IsListType(member.MemberType))
            {
                PopulateList(state, member, memberElement);
                return objectInstance;
            }

            // Handle generic enumerables.
            if (IsEnumerableType(member.MemberType))
            {
                PopulateEnumerable(state, member, memberElement);
                return objectInstance;
            }

            // Handle complex and simple objects.
            if (memberElement.Elements().Any())
            {
                var complexType = GetTypeFromElement(state, member.MemberType, memberElement);
                var complexTypeValue = member.GetValueFromData(memberElement);
                if (complexTypeValue == null)
                {
                    complexTypeValue = CreateObject(state, complexType, null, GetSpecifiedConstructorArguments(memberElement));
                    if (!complexType.GetTypeInfo().IsValueType)
                        member.SetValueFromData(complexTypeValue, memberElement);
                }

                if (!member.IsIndexer)
                {
                    complexTypeValue = PopulateObjectFromAttributes(state, complexTypeValue, memberElement);
                }
                complexTypeValue = PopulateObjectFromElements(state, complexTypeValue, memberElement);

                if (complexType.GetTypeInfo().IsValueType)
                    member.SetValueFromData(complexTypeValue, memberElement);
            }
            else
            {
                if (String.IsNullOrEmpty(memberElement.Value))
                    return objectInstance;

                var memberValue = ParseValue(memberElement.Value, member.MemberType, state.Culture);
                member.SetValueFromData(memberValue, memberElement);
            }

            return objectInstance;
        }

        /// <summary>
        /// Gets a value indicating whether the specified type is a list.
        /// </summary>
        /// <param name="type">The type to evaluate.</param>
        /// <returns><c>true</c> if the specified type is a list; otherwise, <c>false</c>.</returns>
        private static Boolean IsListType(Type type)
        {
            if (type == typeof(IList))
                return true;

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IList<>))
                return true;

            var ifaces = type.GetInterfaces();

            if (ifaces.Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IList<>)))
                return true;

            if (ifaces.Any(x => x == typeof(IList)))
                return true;

            return false;
        }

        /// <summary>
        /// Gets a value indicating whether the specified type is an enumerable.
        /// </summary>
        /// <param name="type">The type to evaluate.</param>
        /// <returns><c>true</c> if the specified type is an enumerable; otherwise, <c>false</c>.</returns>
        private static Boolean IsEnumerableType(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEnumerable<>);
        }

        /// <summary>
        /// Gets the implementation type for the specified list type.
        /// </summary>
        /// <param name="listType">The list type to evaluate.</param>
        /// <returns>The implementation type for the specified list type.</returns>
        private static Type GetListImplementationType(Type listType)
        {
            if (listType == typeof(IList))
                return typeof(ArrayList);

            if (listType.IsGenericType && listType.GetGenericTypeDefinition() == typeof(IList<>))
                return typeof(List<>).MakeGenericType(listType.GetGenericArguments()[0]);

            return listType;
        }

        /// <summary>
        /// Gets the element type for the specified list type.
        /// </summary>
        /// <param name="name">The name of the element that defines the list.</param>
        /// <param name="listType">The list type to evaluate.</param>
        /// <returns>The element type for the specified list type.</returns>
        private static Type GetListElementType(String name, Type listType)
        {
            if (listType == typeof(IList))
                return typeof(Object);

            if (listType.IsGenericType && listType.GetGenericTypeDefinition() == typeof(IList<>))
                return listType.GetGenericArguments()[0];

            var ifaces = listType.GetInterfaces();

            var listImpls = ifaces.Where(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IList<>));
            if (listImpls.Count() > 1)
            {
                throw new InvalidOperationException(NucleusStrings.MemberImplementsMultipleListImpls.Format(name));
            }

            if (listImpls.Any())
                return listImpls.Single().GetGenericArguments()[0];

            if (ifaces.Any(x => x == typeof(IList)))
                return typeof(Object);

            return null;
        }

        /// <summary>
        /// Gets the element type for the specified enumerable type.
        /// </summary>
        /// <param name="name">The name of the element that defines the enumerable.</param>
        /// <param name="enumerableType">The enumerable type to evaluate.</param>
        /// <returns>The element type for the specified enumerable type.</returns>
        private static Type GetEnumerableElementType(String name, Type enumerableType)
        {
            return enumerableType.GetGenericArguments()[0];
        }

        /// <summary>
        /// Populates an array value.
        /// </summary>
        /// <param name="state">The loader state.</param>
        /// <param name="member">The member to populate.</param>
        /// <param name="element">The element that defines the member's value.</param>
        private static void PopulateArray(ObjectLoaderState state, ObjectLoaderMember member, DataElement element)
        {
            // Make sure that the array is properly defined.
            var itemsRoot = element.Element("Items");
            if (itemsRoot != null && itemsRoot.Elements().Where(x => x.Name != "Item").Any())
                throw new InvalidOperationException(NucleusStrings.NonItemElementsInArrayDef);
            
            // Create the array.
            var items            = (itemsRoot == null) ? new List<DataElement>() : itemsRoot.Elements("Item").ToList();
            var arrayElementType = member.MemberType.GetElementType();
            var array            = Array.CreateInstance(arrayElementType, items.Count);
            member.SetValueFromData(array, element);

            // Populate the array's items.
            for (int i = 0; i < items.Count; i++)
            {
                var value = default(Object);
                var type = GetTypeFromElement(state, arrayElementType, items[i]);
                if (items[i].Elements().Any())
                {
                    value = CreateObject(state, type, null, GetSpecifiedConstructorArguments(items[i]));
                    value = PopulateObjectFromElements(state, value, items[i]);
                }
                else
                {
                    value = ObjectResolver.FromString(items[i].Value, type);
                }
                array.SetValue(value, i);
            }
        }

        /// <summary>
        /// Populates a list value.
        /// </summary>
        /// <param name="state">The loader state.</param>
        /// <param name="member">The member to populate.</param>
        /// <param name="element">The element that defines the member's value.</param>
        private static void PopulateList(ObjectLoaderState state, ObjectLoaderMember member, DataElement element)
        {
            // Create the list.
            var listImplType  = GetListImplementationType(member.MemberType);
            var listElemType  = GetListElementType(element.Name, listImplType);
            var list          = Activator.CreateInstance(listImplType);

            // Populate the list's members.
            if (!member.IsIndexer)
            {
                PopulateObjectFromAttributes(state, list, element);
            }
            PopulateObjectFromElements(state, list, element, x => x.Name != "Items");
            PopulateListItems(state, list, listElemType, element);

            // Set the list on the object.
            member.SetValueFromData(list, element);
        }

        /// <summary>
        /// Populates an enumerable value.
        /// </summary>
        /// <param name="state">The loader state.</param>
        /// <param name="member">The member to populate.</param>
        /// <param name="element">The element that defines the member's value.</param>
        private static void PopulateEnumerable(ObjectLoaderState state, ObjectLoaderMember member, DataElement element)
        {
            var listElemType = GetEnumerableElementType(element.Name, member.MemberType);
            var listType     = typeof(List<>).MakeGenericType(listElemType);
            var listInstance = Activator.CreateInstance(listType);

            PopulateListItems(state, listInstance, listElemType, element);

            member.SetValueFromData(listInstance, element);
        }

        /// <summary>
        /// Populates a list with items.
        /// </summary>
        /// <param name="state">The loader state.</param>
        /// <param name="list">The list to populate.</param>
        /// <param name="listElemType">The type of elements in the list.</param>
        /// <param name="element">The element that defines the list.</param>
        private static void PopulateListItems(ObjectLoaderState state, Object list, Type listElemType, DataElement element)
        {
            // Make sure that the list is properly defined.
            var itemsRoot = element.Element("Items");
            if (itemsRoot != null && itemsRoot.Elements().Where(x => x.Name != "Item").Any())
                throw new InvalidOperationException(NucleusStrings.NonItemElementsInListDef);

            // Populate the list's items.
            if (itemsRoot != null)
            {
                var add   = list.GetType().GetMethod("Add", new[] { listElemType });
                var items = itemsRoot.Elements("Item").ToList();

                for (int i = 0; i < items.Count; i++)
                {
                    var value = default(Object);
                    var type = GetTypeFromElement(state, listElemType, items[i]);
                    if (items[i].Elements().Any())
                    {
                        value = CreateObject(state, type, null, GetSpecifiedConstructorArguments(items[i]));
                        value = PopulateObjectFromElements(state, value, items[i]);
                    }
                    else
                    {
                        value = ObjectResolver.FromString(items[i].Value, type);
                    }
                    add.Invoke(list, new[] { value });
                }
            }
        }

        /// <summary>
        /// Attempts to parse a string into a simple value.
        /// </summary>
        /// <param name="str">The string to parse.</param>
        /// <param name="type">The type into which to parse the string.</param>
        /// <param name="provider">An object that provides culture-specific formatting information.</param>
        /// <returns>The value that was parsed.</returns>
        private static Object ParseValue(String str, Type type, IFormatProvider provider)
        {
            return ObjectResolver.FromString(str, type, provider);
        }

        // The global alias registry.
        private static readonly ReaderWriterLockSlim lockobj = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);
        private static readonly Dictionary<String, String> globalAliases =
            new Dictionary<String, String>();
    }
}
