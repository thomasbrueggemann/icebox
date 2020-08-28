using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Icebox.Attributes;
using Icebox.Exceptions;
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

        public void Check(Assembly assembly)
        {
            var typesWithFrozenAttribute = GetTypesWithFrozenAttribute(assembly);
            
            var iceboxName = assembly.GetName().Name;
            var iceboxExistsOnDisk = ExistsOnDisk(iceboxName);
            if (!iceboxExistsOnDisk)
            {
                FrezeAndWriteToDisk(typesWithFrozenAttribute, iceboxName);
                return;
            }

            var frozenContracts = ReadFromDisk(iceboxName);
            FindMatchingTypesToIceboxedContracts(assembly.GetTypes(), frozenContracts);
        }

        public void FindMatchingTypesToIceboxedContracts(
            IEnumerable<Type> assemblyTypes, 
            IReadOnlyCollection<IceboxedContract> frozenContracts)
        {
            foreach (IceboxedContract frozenContract in frozenContracts)
            {
                Type matchingAssemblyType = FindMatchingAssemblyTypeToIceboxedContract(frozenContract, assemblyTypes);

                if (matchingAssemblyType == null)
                {
                    throw new IceboxedContractException(
                        frozenContract, 
                        "IceboxedContract Type not found in Assembly. Shame!");
                }

                var publicPropertiesOfAssemblyType = IceboxGenerator.GetPublicPropertiesOfType(matchingAssemblyType);
                foreach (IceboxedContractMember frozenContractMember in frozenContract.Members)
                {
                    var matchingProperty =
                        FindMatchingPropertyToIceboxedContractMember(frozenContractMember,
                            publicPropertiesOfAssemblyType);

                    if (matchingProperty == null)
                    {
                        throw new IceboxedContractException(
                            frozenContract, 
                            frozenContractMember, 
                            "IceboxedContract Property not found in Type. OMG!");
                    }
                }
            }
        }

        private static List<Type> GetTypesWithFrozenAttribute(Assembly assembly)
        {
            return assembly.GetTypes()
                .Where(TypeHasFrozenAttribute)
                .ToList();
        }

        private static PropertyInfo FindMatchingPropertyToIceboxedContractMember(
            IceboxedContractMember frozenContractMember, PropertyInfo[] assemblyProperties)
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
        
        private static void FrezeAndWriteToDisk(IEnumerable<Type> typesWithFrozenAttribute, string iceboxName)
        {
            var frozenContracts = typesWithFrozenAttribute
                .Select(IceboxGenerator.Freeze)
                .ToList();

            if (frozenContracts.Count > 0)
            {
                WriteToDisk(iceboxName, frozenContracts);
            }
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