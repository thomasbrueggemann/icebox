using System;

namespace Icebox
{
    public class FrozenContractMember
    {
        public string Name { get; }
        public Type Type { get; }

        public FrozenContractMember(Type type, string name)
        {
            Type = type;
            Name = name;
        }
    }
}