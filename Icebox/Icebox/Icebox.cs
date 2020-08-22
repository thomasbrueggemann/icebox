using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using static Icebox.IceboxIO;

namespace Icebox
{
    public class Icebox
    {
        private IceboxOptions _options;
        
        public Icebox(IceboxOptions options)
        {
            _options = options;
        }
        
        public Icebox() {}

        public void CheckContracts(Assembly assembly)
        {
            var iceboxName = assembly.FullName;
            
            var typesWithFrozenAttribute = assembly.GetTypes()
                .Where(TypeHasFrozenAttribute)
                .ToList();
            
            var iceboxExistsOnDisk = ExistsOnDisk(iceboxName);
            if (!iceboxExistsOnDisk)
            {
                WriteFrozenContractsToDisk(typesWithFrozenAttribute, iceboxName);
                return;
            }

            var frozenContracts = ReadFromDisk(iceboxName);
            foreach (FrozenContract frozenContract in frozenContracts)
            {
                Type matchingAssemblyType = FindMatchingAssemblyTypeToFrozenContract(frozenContract, assembly);
                
                if (matchingAssemblyType == null)
                {
                    throw new Exception("FrozenContract Type not found in Assembly. Shame!");
                }

                var publicPropertiesOfAssemblyType = IceboxGenerator.GetPublicPropertiesOfType(matchingAssemblyType);
                foreach (FrozenContractMember frozenContractMember in frozenContract.Members)
                {
                    var matchingProperty =
                        FindMatchingPropertyToFrozenContractMember(frozenContractMember,
                            publicPropertiesOfAssemblyType);
                }
            }

            // TODO: figure out how to allow breaking changes that are intentional
        }

        private static PropertyInfo FindMatchingPropertyToFrozenContractMember(
            FrozenContractMember frozenContractMember, PropertyInfo[] assemblyProperties)
        {
            return assemblyProperties.FirstOrDefault();
        }

        private static Type FindMatchingAssemblyTypeToFrozenContract(FrozenContract frozenContract, Assembly assembly)
        {
            return assembly.GetTypes().FirstOrDefault(t => t.Name == frozenContract.Name);
        }
        
        private static void WriteFrozenContractsToDisk(IEnumerable<Type> typesWithFrozenAttribute, string iceboxName)
        {
            var frozenContracts = typesWithFrozenAttribute
                .Select(IceboxGenerator.Freeze)
                .ToList();

            WriteToDisk(iceboxName, frozenContracts);
        }

        private static bool TypeHasFrozenAttribute(Type type)
        {
            var hasFrozenAttributes = type
                .GetCustomAttributes()
                .Any(a => a.GetType() == typeof(FrozenAttribute));

            return hasFrozenAttributes;
        }
    }
}