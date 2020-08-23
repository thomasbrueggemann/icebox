using Icebox.Attributes;

namespace Icebox.Test
{
    [Frozen]
    public class FrozenSimpleTestClass
    {
        public string ID { get; set; }
        public int Counter { get; set; }
        public bool IsActive { get; set; }
    }
}