using System;
using System.Reflection;

namespace Icebox
{
    public class Icebox
    {
        private Type _typeToInspect { get; set; }
        
        public Icebox(Type typeToInspect)
        {
            _typeToInspect = typeToInspect;
        }

        public void Freeze()
        {
            var flags = BindingFlags.Public | BindingFlags.Instance;
            var properties = _typeToInspect.GetProperties(flags);

            foreach (PropertyInfo property in properties)
            {
                Console.WriteLine("Name: " + property.Name);
            }
        }
    }
}