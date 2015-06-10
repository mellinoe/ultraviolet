﻿using System;
using System.ComponentModel;
using System.Globalization;

namespace TwistedLogik.Nucleus.Data
{
    /// <summary>
    /// Represents a type converter which makes use of the Nucleus Object Resolver.
    /// </summary>
    /// <typeparam name="T">The type of object to convert.</typeparam>
    public class ObjectResolverTypeConverter<T> : TypeConverter
    {
        /// <summary>
        /// Returns whether this converter can convert an object of the given type to the type of this converter, using the specified context.
        /// </summary>
        /// <param name="context">An <see cref="ITypeDescriptorContext"/> that provides a format context.</param>
        /// <param name="sourceType">A <see cref="Type"/> that represents the type you want to convert from.</param>
        /// <returns><c>true</c> if this converter can perform the conversion; otherwise, <c>false</c>.</returns>
        public override Boolean CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(String))
                return true;
            return base.CanConvertFrom(context, sourceType);
        }

        /// <summary>
        /// Converts the given object to the type of this converter, using the specified context and culture information.
        /// </summary>
        /// <param name="context">An <see cref="ITypeDescriptorContext"/> that provides a format context.</param>
        /// <param name="culture">The <see cref="CultureInfo"/> to use as the current culture.</param>
        /// <param name="value">The <see cref="Object"/> to convert.</param>
        /// <returns>An <see cref="Object"/> that represents the converted value.</returns>
        public override Object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, Object value)
        {
            if (value is String)
            {
                if (String.IsNullOrEmpty((String)value))
                    return default(T);
#if NETCORE
                Type type;
                if (context == null)
                {
                    type = typeof(T);
                }
                else
                {
                    throw new NotImplementedException("PropertyDescriptor is stubbed out.");
                }
#else
                var type = (context == null) ? typeof(T) : context.PropertyDescriptor.PropertyType;
#endif
                return ObjectResolver.FromString((String)value, type);
            }
            return base.ConvertFrom(context, culture, value);
        }
    }
}