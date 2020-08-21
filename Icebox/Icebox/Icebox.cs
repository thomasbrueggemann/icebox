using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Icebox
{
    public class Icebox
    {
        private Assembly _assemblyToInspect { get; }
        
        public Icebox(Assembly assembly)
        {
            _assemblyToInspect = assembly;
        }
        
        public IEnumerable<FrozenContract> Freeze(Assembly assembly)
        {
            foreach (Type assemblyType in assembly.GetTypes())
            {
                var frozenTypeContract = Freeze(assemblyType);
                if (frozenTypeContract != null)
                {
                    yield return frozenTypeContract;
                }
            }
        }

        private FrozenContract Freeze(Type type)
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
        
        private bool TypeHasFrozenAttribute(Type type) => type
            .GetCustomAttributes()
            .Any(a => a.GetType() == typeof(FrozenAttribute));

        private FrozenContract GetFrozenContractForProperties(PropertyInfo[] properties)
        {
            var frozenContract = new FrozenContract(
                properties
                    .Select(PropertyInfoToFrozenContractMember)
                    .ToList());
            
            return frozenContract;
        }

        private PropertyInfo[] GetPublicPropertiesOfType(Type type)
        {
            var flags = BindingFlags.Public | BindingFlags.Instance;
            var properties = type.GetProperties(flags);

            return properties;
        }

        private FrozenContractMember PropertyInfoToFrozenContractMember(PropertyInfo propertyInfo)
        {
            var member = new FrozenContractMember(propertyInfo.PropertyType, propertyInfo.Name);
            
            return member;
        }
    }
}