using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Icebox.Attributes;

namespace Icebox
{
    public class Icebox
    {
        private readonly Assembly _assembly;
        
        public Icebox(Assembly assembly)
        {
            _assembly = assembly;
        }

        public string? Name => _assembly?.GetName()?.Name;
        
        public IReadOnlyCollection<IceboxedContract> Freeze()
        {
            var typesWithIceboxedAttribute = GetTypesWithIceboxedAttribute();
            
            var contracts = typesWithIceboxedAttribute
                .Select(IceboxGenerator.Freeze)
                .ToList();

            return contracts;
        }

        public IceboxMatchResults Check()
        {
            var iceboxPath = _assembly.Location;
            
            var iceboxExistsOnDisk = IceboxIO.ExistsOnDisk(iceboxPath, Name);
            if (iceboxExistsOnDisk == true)
            {
                var contracts = IceboxIO.ReadFromDisk(iceboxPath, Name);
                var results = FindMatchingTypesToIceboxedContracts(contracts);

                return results;
            }

            var fileNotFoundResult = new IceboxMatchResults();
            fileNotFoundResult.AddBreakingChange(new IceboxBreakingChange(IceboxBreakingChangeType.FileNotFound));

            return fileNotFoundResult;
        }
        
        private IceboxMatchResults FindMatchingTypesToIceboxedContracts(IReadOnlyCollection<IceboxedContract> contracts)
        {
            var result = new IceboxMatchResults();
            
            foreach (IceboxedContract contract in contracts)
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

        private IReadOnlyCollection<Type> GetTypesWithIceboxedAttribute()
        {
            var iceboxedTypes = _assembly.GetExportedTypes()
                .Where(TypeHasFrozenAttribute)
                .ToList();

            return iceboxedTypes;
        }

        private static PropertyInfo FindMatchingPropertyToIceboxedContractMember(
            IceboxedContractMember frozenContractMember, 
            IEnumerable<PropertyInfo> assemblyProperties)
        {
            var property = assemblyProperties.FirstOrDefault(p 
                => p.PropertyType == frozenContractMember.Type && 
                   p.Name == frozenContractMember.Name);
            
            return property;
        }

        private static Type FindMatchingAssemblyTypeToIceboxedContract(
            IceboxedContract frozenContract,
            IEnumerable<Type> types)
        {
            var type = types.FirstOrDefault(t => t.Name == frozenContract.Name);
            return type;
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