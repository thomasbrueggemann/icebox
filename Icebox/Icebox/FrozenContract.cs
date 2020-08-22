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
        public IReadOnlyCollection<FrozenContractMember> Members { get; }
        
        public FrozenContract(string name, IReadOnlyCollection<FrozenContractMember> members)
        {
            Name = name;
            Members = members;
        }
    }
}