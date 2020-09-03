using System.Collections.Generic;
using ProtoBuf;

namespace Icebox
{
    [ProtoContract]
    public class IceboxedContract
    {
        [ProtoMember(1)]
        public string? Name { get; }

        [ProtoMember(2)]
        public IReadOnlyCollection<IceboxedContractMember> Members { get; }
        
        public IceboxedContract(string name, IReadOnlyCollection<IceboxedContractMember> members)
        {
            Name = name;
            Members = members;
        }
        
        private IceboxedContract()
        {
            Members = new IceboxedContractMember[] { };
        }
    }
}