using System.Collections.Generic;

namespace Icebox
{
    public class IceboxMatchResults
    {
        private readonly List<IceboxBreakingChange> _breakingChanges = new List<IceboxBreakingChange>();

        public bool Succeeded { get; private set; } = true;

        public IReadOnlyCollection<IceboxBreakingChange> BreakingChanges => _breakingChanges;

        public void AddBreakingChange(IceboxBreakingChange breakingChange)
        {
            _breakingChanges.Add(breakingChange);
            Succeeded = false;
        }
    }
}