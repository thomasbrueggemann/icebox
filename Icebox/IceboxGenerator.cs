using System;
using System.Collections.Generic;
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

        private static IceboxedContract GetIceboxedContractForProperties(Type type, IEnumerable<PropertyInfo> properties)
        {
            var contract = new IceboxedContract(
                type.Name,
                properties
                    .Select(ConvertPropertyInfoToIceboxedContractMember)
                    .ToList());
            
            return contract;
        }

        public static IEnumerable<PropertyInfo> GetPublicPropertiesOfType(Type? type)
        {
            var flags = BindingFlags.Public | BindingFlags.Instance;
            var properties = type?.GetProperties(flags).ToList() ?? new List<PropertyInfo>();

            return properties;
        }

        private static IceboxedContractMember ConvertPropertyInfoToIceboxedContractMember(PropertyInfo propertyInfo)
        {
            var member = new IceboxedContractMember(propertyInfo.PropertyType, propertyInfo.Name);
            
            return member;
        }
    }
}