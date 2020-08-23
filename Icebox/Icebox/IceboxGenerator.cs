using System;
using System.Linq;
using System.Reflection;

namespace Icebox
{
    public static class IceboxGenerator
    {
        public static FrozenContract Freeze(Type type)
        {
            var properties = GetPublicPropertiesOfType(type);
            var frozenContract = GetFrozenContractForProperties(type, properties);

            return frozenContract;
        }

        private static FrozenContract GetFrozenContractForProperties(Type type, PropertyInfo[] properties)
        {
            var frozenContract = new FrozenContract(
                type.Name,
                properties
                    .Select(ConvertPropertyInfoToFrozenContractMember)
                    .ToList());
            
            return frozenContract;
        }

        public static PropertyInfo[] GetPublicPropertiesOfType(Type type)
        {
            var flags = BindingFlags.Public | BindingFlags.Instance;
            var properties = type.GetProperties(flags);

            return properties;
        }

        private static FrozenContractMember ConvertPropertyInfoToFrozenContractMember(PropertyInfo propertyInfo)
        {
            var member = new FrozenContractMember(propertyInfo.PropertyType, propertyInfo.Name);
            
            return member;
        }
    }
}