using System.Collections.Generic;

namespace Icebox
{
    public class FrozenContract
    {
        public IReadOnlyCollection<FrozenContractMember> Items { get; }
        
        public FrozenContract(IReadOnlyCollection<FrozenContractMember> items)
        {
            Items = items;
        }
    }
}