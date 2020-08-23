using System;
using System.Collections.Generic;
using System.Reflection;
using Xunit;
using AutoFixture.Xunit2;
using FluentAssertions;
using Icebox.Exceptions;

namespace Icebox.Test
{
    public class IceboxTest
    {
        public class Check
        {
            [Theory, AutoData]
            public void ShouldCreateANewSnapshot(Icebox icebox)
            {
                var assembly = Assembly.GetExecutingAssembly();
                Action act = () => icebox.Check(assembly);
                
                act.Should().NotThrow<FrozenContractException>();
                act.Should().NotThrow<Exception>();
            }
        }

        public class FindMatchingTypesToFrozenContracts
        {
            private class TestSample
            {
                public string Id { get; set; }   
            }
            
            [Theory, AutoData]
            public void ShouldNotDetectAnyBreakingChangesSinceThereAreNone(Icebox icebox)
            {
                var types = new List<Type>
                {
                    typeof(TestSample)
                };

                var frozenContracts = new List<FrozenContract>
                {
                    new FrozenContract(nameof(TestSample), new List<FrozenContractMember>
                    {
                        new FrozenContractMember(typeof(string), "Id")
                    })
                };

                Action act = () => icebox.FindMatchingTypesToFrozenContracts(types, frozenContracts);

                act.Should().NotThrow<FrozenContractException>();
            }
            
            [Theory, AutoData]
            public void ShouldDetectABreakingChangeOfMissingProperty(Icebox icebox)
            {
                var types = new List<Type>
                {
                    typeof(TestSample)
                };

                var frozenContracts = new List<FrozenContract>
                {
                    new FrozenContract(nameof(TestSample), new List<FrozenContractMember>
                    {
                        new FrozenContractMember(typeof(string), "Id"),
                        new FrozenContractMember(typeof(string), "Name")
                    })
                };

                Action act = () => icebox.FindMatchingTypesToFrozenContracts(types, frozenContracts);

                act.Should().Throw<FrozenContractException>();
            }
            
            [Theory, AutoData]
            public void ShouldDetectABreakingDatatypeChange(Icebox icebox)
            {
                var types = new List<Type>
                {
                    typeof(TestSample)
                };

                var frozenContracts = new List<FrozenContract>
                {
                    new FrozenContract(nameof(TestSample), new List<FrozenContractMember>
                    {
                        new FrozenContractMember(typeof(int), "Id")
                    })
                };

                Action act = () => icebox.FindMatchingTypesToFrozenContracts(types, frozenContracts);

                act.Should().Throw<FrozenContractException>();
            }
        }
    }
}