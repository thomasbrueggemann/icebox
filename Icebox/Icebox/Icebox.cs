using System.Reflection;
using static Icebox.IceboxIO;

namespace Icebox
{
    public class Icebox
    {
        private IceboxOptions _options;
        
        public Icebox(IceboxOptions options)
        {
            _options = options;
        }
        
        public Icebox() {}

        public void CheckContracts(Assembly assembly)
        {
            var frozenContracts = IceboxGenerator.Freeze(assembly);
            
            foreach (var frozenContract in frozenContracts)
            {
                WriteFrozenContract(frozenContract, _options.StoreIceboxesAlongsideSourceFiles);
            }
        }
    }
}