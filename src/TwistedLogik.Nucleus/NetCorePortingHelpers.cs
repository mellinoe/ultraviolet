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
        }
    }
}