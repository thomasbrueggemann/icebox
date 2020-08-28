using System;
using ProtoBuf;

namespace Icebox
{
    [ProtoContract]
    public class IceboxedContractMember
    {
        [ProtoMember(1)]
        public string Name { get; }
        
        [ProtoMember(2)]
        public Type Type { get; }

        public IceboxedContractMember(Type type, string name)
        {
            Type = type;
            Name = name;
        }

        private IceboxedContractMember()
        {
        }
    }
}