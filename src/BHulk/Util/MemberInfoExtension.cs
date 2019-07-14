using System;
using System.Reflection;

namespace BHulk.Util
{
    internal static class MemberInfoExtension
    {
        internal static void CheckValueType(this MemberInfo member, object value)
        {
            var propertyType = ((PropertyInfo) member).PropertyType;

            if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                if (value == null) return;
                propertyType = propertyType.GetGenericArguments()[0];
            }

            if (propertyType != value.GetType())
                throw new ArgumentException($"Invalid value for {member.Name}");
            
        }
    }
}
