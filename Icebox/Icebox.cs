using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Icebox.Attributes;

namespace Icebox
{
    public class Icebox
    {
        private Assembly _assembly;
        
        public Icebox(Assembly assembly)
        {
            _assembly = assembly;
        }

        public string? Name => _assembly?.GetName()?.Name;

        public IceboxMatchResults FindMatchingTypesToIceboxedContracts(IReadOnlyCollection<IceboxedContract> iceboxedContracts)
        {
            var result = new IceboxMatchResults();
            
            foreach (IceboxedContract contract in iceboxedContracts)
            {
                Type matchingAssemblyType = FindMatchingAssemblyTypeToIceboxedContract(contract, _assembly.GetTypes());

                if (matchingAssemblyType == null)
                {
                    result.AddBreakingChange(new IceboxBreakingChange(
                        IceboxBreakingChangeType.TypeNotFound,
                        contract));
                }

                var publicPropertiesOfAssemblyType = IceboxGenerator
                    .GetPublicPropertiesOfType(matchingAssemblyType)
                    .ToList();
                
                foreach (IceboxedContractMember contractMember in contract.Members)
                {
                    var matchingProperty =
                        FindMatchingPropertyToIceboxedContractMember(contractMember,
                            publicPropertiesOfAssemblyType);

                    if (matchingProperty == null)
                    {
                        result.AddBreakingChange(new IceboxBreakingChange(
                            IceboxBreakingChangeType.TypeNotFound,
                            contract,
                            contractMember));
                    }
                }
            }

            return result;
        }
        
        public IReadOnlyCollection<IceboxedContract> Freeze()
        {
            var typesWithIceboxedAttribute = GetTypesWithIceboxedAttribute();
            
            var iceboxedContracts = typesWithIceboxedAttribute
                .Select(IceboxGenerator.Freeze)
                .ToList();

            return iceboxedContracts;
        }

        private IReadOnlyCollection<Type> GetTypesWithIceboxedAttribute()
        {
            return _assembly.GetExportedTypes()
                .Where(TypeHasFrozenAttribute)
                .ToList();
        }

        private static PropertyInfo FindMatchingPropertyToIceboxedContractMember(
            IceboxedContractMember frozenContractMember, 
            IEnumerable<PropertyInfo> assemblyProperties)
        {
            return assemblyProperties.FirstOrDefault(p 
                => p.PropertyType == frozenContractMember.Type && p.Name == frozenContractMember.Name);
        }

        private static Type FindMatchingAssemblyTypeToIceboxedContract(
            IceboxedContract frozenContract,
            IEnumerable<Type> types)
        {
            return types.FirstOrDefault(t => t.Name == frozenContract.Name);
        }

        private static bool TypeHasFrozenAttribute(Type type)
        {
            var hasFrozenAttributes = type
                .GetCustomAttributes()
                .Any(a => a.GetType() == typeof(IceboxedAttribute));

            return hasFrozenAttributes;
        }
    }
}