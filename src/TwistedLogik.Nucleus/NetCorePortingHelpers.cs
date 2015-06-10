using System.Globalization;
using System.Linq;

// Small porting helpers to help my laziness

namespace System.Security
{
    public class SuppressUnmanagedCodeSecurityAttribute : Attribute
    {
    }
}

namespace System.Runtime.Serialization
{
    public class SerializationInfo
    {
    }
}

namespace System
{
    public class SerializableAttribute : Attribute
    {
    }

    public class EnvironmentEx
    {
        public unsafe static bool Is64BitProcess
        {
            get
            {
                return sizeof(IntPtr) == 8;
            }
        }
    }
}

namespace System.Reflection
{
    [Flags]
    public enum MemberTypes
    {
        // The following are the known classes which extend MemberInfo
        Constructor = 0x01,
        Event = 0x02,
        Field = 0x04,
        Method = 0x08,
        Property = 0x10,
        TypeInfo = 0x20,
        Custom = 0x40,
        NestedType = 0x80,
        All = Constructor | Event | Field | Method | Property | TypeInfo | NestedType,
    }

    public static class NetCoreReflectionHelpers
    {
        public static Type GetInterface(this Type type, string fullInterfaceName)
        {
            var interfaces = type.GetInterfaces();
            return interfaces.SingleOrDefault(t => t.FullName == fullInterfaceName);
        }

        public static bool IsAssignableFrom(this TypeInfo t, Type other)
        {
            return t.IsAssignableFrom(other.GetTypeInfo());
        }

        public static MemberTypes MemberType(this MemberInfo memberInfo)
        {
            Type t = memberInfo.GetType();
            if (t == typeof(FieldInfo))
            {
                return MemberTypes.Field;
            }
            else if (t == typeof(MethodInfo))
            {
                return MemberTypes.Method;
            }
            else if (t == typeof(PropertyInfo))
            {
                return MemberTypes.Property;
            }
            else
            {
                throw new NotImplementedException(); // Not sure any others are used.
            }
        }
    }
}

namespace System.ComponentModel
{
    public class DefaultPropertyAttribute : Attribute
    {
        private string _value;

        public DefaultPropertyAttribute(string value)
        {
            _value = value;
        }

        public string Name { get { return _value; } }
    }

    public interface ISupportInitialize
    {
        void BeginInit();

        void EndInit();
    }

    public static class TypeConverterExtensions
    {
        public static bool IsValid(this TypeConverter @this, object value)
        {
            return IsValid(@this, null, value);
        }

        public static bool IsValid(this TypeConverter @this, ITypeDescriptorContext context, object value)
        {
            bool isValid = true;
            try
            {
                // Because null doesn't have a type, so we couldn't pass this to CanConvertFrom.
                // Meanwhile, we couldn't silence null value here, such as type converter like
                // NullableConverter would consider null value as a valid value.
                if (value == null || @this.CanConvertFrom(context, value.GetType()))
                {
                    @this.ConvertFrom(context, CultureInfo.CurrentCulture, value);
                }
                else
                {
                    isValid = false;
                }
            }
            catch
            {
                isValid = false;
            }

            return isValid;
        }
    }
}