using System;

namespace Icebox.Exceptions
{
    public class IceboxedContractException : Exception
    {
        public IceboxedContract IceboxedContractContract { get; }
        public IceboxedContractMember IceboxedContractMember { get; }
        
        public IceboxedContractException(
            IceboxedContract frozenContractContract, 
            IceboxedContractMember frozenContractMember, 
            string message)
        : base(message)
        {
            IceboxedContractContract = frozenContractContract;
            IceboxedContractMember = frozenContractMember;
        }
        
        public IceboxedContractException(
            IceboxedContract frozenContractContract,
            string message)
            : base(message)
        {
            IceboxedContractContract = frozenContractContract;
            IceboxedContractMember = null;
        }
    }
}