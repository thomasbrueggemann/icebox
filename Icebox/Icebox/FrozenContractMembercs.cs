using System;
using ProtoBuf;

namespace Icebox
{
    [ProtoContract]
    public class FrozenContractMember
    {
        [ProtoMember(1)]
        public string Name { get; }
        
        [ProtoMember(2)]
        public Type Type { get; }

        public FrozenContractMember(Type type, string name)
        {
            Type = type;
            Name = name;
        }

        private FrozenContractMember()
        {
        }
    }
}