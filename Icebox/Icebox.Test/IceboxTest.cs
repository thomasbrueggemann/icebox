using System;
using Xunit;

namespace Icebox.Test
{
    public class IceboxTest
    {
        private class TestClass
        {
            public string Id { get; set; }
            public int Age { get; set; }
        }
        
        [Fact]
        public void CreateAFreezeSnapshot()
        {
            var icebox = new Icebox(typeof(TestClass));
            icebox.Freeze();
        }
    }
}