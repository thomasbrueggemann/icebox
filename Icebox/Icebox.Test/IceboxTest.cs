using System;
using System.Collections.Generic;
using System.Reflection;
using Xunit;
using AutoFixture.Xunit2;
using FluentAssertions;
using Icebox.Exceptions;

namespace Icebox.Test
{
    public sealed class IceboxTest
    {
        public sealed class Check
        {
            [Theory, AutoData]
            public void ShouldCreateANewSnapshot(Icebox icebox)
            {
                var assembly = Assembly.GetExecutingAssembly();
                Action act = () => icebox.Check(assembly);
                
                act.Should().NotThrow<IceboxedContractException>();
                act.Should().NotThrow<Exception>();
            }
        }

        public sealed class FindMatchingTypesToIceboxedContracts
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

                var contracts = new List<IceboxedContract>
                {
                    new IceboxedContract(nameof(TestSample), new List<IceboxedContractMember>
                    {
                        new IceboxedContractMember(typeof(string), "Id")
                    })
                };

                Action act = () => icebox.FindMatchingTypesToIceboxedContracts(types, contracts);

                act.Should().NotThrow<IceboxedContractException>();
            }
            
            [Theory, AutoData]
            public void ShouldDetectABreakingChangeOfMissingProperty(Icebox icebox)
            {
                var types = new List<Type>
                {
                    typeof(TestSample)
                };

                var contracts = new List<IceboxedContract>
                {
                    new IceboxedContract(nameof(TestSample), new List<IceboxedContractMember>
                    {
                        new IceboxedContractMember(typeof(string), "Id"),
                        new IceboxedContractMember(typeof(string), "Name")
                    })
                };

                Action act = () => icebox.FindMatchingTypesToIceboxedContracts(types, contracts);

                act.Should().Throw<IceboxedContractException>();
            }
            
            [Theory, AutoData]
            public void ShouldDetectABreakingDatatypeChange(Icebox icebox)
            {
                var types = new List<Type>
                {
                    typeof(TestSample)
                };

                var contracts = new List<IceboxedContract>
                {
                    new IceboxedContract(nameof(TestSample), new List<IceboxedContractMember>
                    {
                        new IceboxedContractMember(typeof(int), "Id")
                    })
                };

                Action act = () => icebox.FindMatchingTypesToIceboxedContracts(types, contracts);

                act.Should().Throw<IceboxedContractException>();
            }
        }
    }
}