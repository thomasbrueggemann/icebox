using System.Collections.Generic;
using ProtoBuf;

namespace Icebox
{
    [ProtoContract]
    public class FrozenContract
    {
        [ProtoMember(1)]
        public string Name { get; }
        
        [ProtoMember(2)]
        public string FilePath { get; }
    
        [ProtoMember(3)]
        public IReadOnlyCollection<FrozenContractMember> Items { get; }
        
        public FrozenContract(string name, string filePath, IReadOnlyCollection<FrozenContractMember> items)
        {
            Name = name;
            Items = items;
            FilePath = filePath;
        }
    }
}