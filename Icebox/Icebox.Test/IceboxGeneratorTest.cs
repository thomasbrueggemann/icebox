using System;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace Icebox.Test
{
    public class IceboxGeneratorTest
    {
        public class Freeze
        { 
            private class TestSubject
            {
                public string Id { get; set; }
                public int Age { get; set; }

                private string _test;

                public string ToString()
                {
                    return "";
                }
            }
             
            [Fact]
            public void ShouldFreezeAClassWithTwoPublicMembers()
            {
                var frozenContract = IceboxGenerator.Freeze(typeof(TestSubject));

                frozenContract.Members.Count.Should().Be(2);

                var idMember = frozenContract.Members.FirstOrDefault(m => m.Name == "Id");
                idMember.Should().NotBeNull();
                idMember?.Type.Should().Be(typeof(string));

                var ageMember = frozenContract.Members.FirstOrDefault(m => m.Name == "Age");
                ageMember.Should().NotBeNull();
                ageMember?.Type.Should().Be(typeof(int));
            }   
        }

        public class GetPublicPropertiesOfType
        {
            private class TestTypeWithPublicProperties
            {
                public string Id { get; set; }
                public int Age { get; set; }

                private string _test;

                public string ToString()
                {
                    return "";
                }
            }
            
            private class TestTypeWithNoPublicProperties
            {
                private string _test;

                public string ToString()
                {
                    return "";
                }
            }
            
            [Fact]
            public void ShouldReturnAllPublicPropertiesOfAType()
            {
                var properties = IceboxGenerator.GetPublicPropertiesOfType(typeof(TestTypeWithPublicProperties));
                properties.Length.Should().Be(2);
            }
            
            [Fact]
            public void ShouldReturnEmptyArrayForATypeWithNoPublicProperties()
            {
                var properties = IceboxGenerator.GetPublicPropertiesOfType(typeof(TestTypeWithNoPublicProperties));
                properties.Length.Should().Be(0);
            }
        }
    }
}