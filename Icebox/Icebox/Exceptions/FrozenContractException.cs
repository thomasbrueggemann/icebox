using System;

namespace Icebox.Exceptions
{
    public class FrozenContractException : Exception
    {
        public FrozenContract FrozenContractContract { get; }
        public FrozenContractMember FrozenContractMember { get; }
        
        public FrozenContractException(
            FrozenContract frozenContractContract, 
            FrozenContractMember frozenContractMember, 
            string message)
        : base(message)
        {
            FrozenContractContract = frozenContractContract;
            FrozenContractMember = frozenContractMember;
        }
        
        public FrozenContractException(
            FrozenContract frozenContractContract,
            string message)
            : base(message)
        {
            FrozenContractContract = frozenContractContract;
            FrozenContractMember = null;
        }
    }
}