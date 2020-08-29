namespace Icebox
{
    public class IceboxBreakingChange
    {
        public IceboxBreakingChange(
            IceboxBreakingChangeType type, 
            IceboxedContract contract = null, 
            IceboxedContractMember contractMember = null)
        {
            Type = type;
            Contract = contract;
            ContractMember = contractMember;
        }

        public IceboxBreakingChangeType Type { get; }
        
        public IceboxedContract Contract { get; }
        
        public IceboxedContractMember ContractMember { get; }
    }
}