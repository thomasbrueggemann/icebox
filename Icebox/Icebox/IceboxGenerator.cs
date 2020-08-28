using System;
using System.Linq;
using System.Reflection;

namespace Icebox
{
    public static class IceboxGenerator
    {
        public static IceboxedContract Freeze(Type type)
        {
            var properties = GetPublicPropertiesOfType(type);
            var frozenContract = GetIceboxedContractForProperties(type, properties);

            return frozenContract;
        }

        private static IceboxedContract GetIceboxedContractForProperties(Type type, PropertyInfo[] properties)
        {
            var frozenContract = new IceboxedContract(
                type.Name,
                properties
                    .Select(ConvertPropertyInfoToIceboxedContractMember)
                    .ToList());
            
            return frozenContract;
        }

        public static PropertyInfo[] GetPublicPropertiesOfType(Type type)
        {
            var flags = BindingFlags.Public | BindingFlags.Instance;
            var properties = type.GetProperties(flags);

            return properties;
        }

        private static IceboxedContractMember ConvertPropertyInfoToIceboxedContractMember(PropertyInfo propertyInfo)
        {
            var member = new IceboxedContractMember(propertyInfo.PropertyType, propertyInfo.Name);
            
            return member;
        }
    }
}