using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Icebox
{
    public static class IceboxGenerator
    {
        public static IReadOnlyCollection<FrozenContract> Freeze(Assembly assembly)
        {
            var frozenContracts = assembly.GetTypes()
                .Select(FreezeType)
                .Where(t => t != null)
                .ToList();

            return frozenContracts;
        }

        private static FrozenContract FreezeType(Type type)
        {
            var isFrozenType = TypeHasFrozenAttribute(type);
            if (!isFrozenType)
            {
                return null;
            }
            
            var properties = GetPublicPropertiesOfType(type);
            var frozenContract = GetFrozenContractForProperties(properties);

            return frozenContract;
        }

        private static bool TypeHasFrozenAttribute(Type type)
        {
            var hasFrozenAttributes = type
                .GetCustomAttributes()
                .Any(a => a.GetType() == typeof(FrozenAttribute));

            return hasFrozenAttributes;
        }

        private static FrozenContract GetFrozenContractForProperties(PropertyInfo[] properties)
        {
            var frozenContract = new FrozenContract(
                properties
                    .Select(ConvertPropertyInfoToFrozenContractMember)
                    .ToList());
            
            return frozenContract;
        }

        private static PropertyInfo[] GetPublicPropertiesOfType(Type type)
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