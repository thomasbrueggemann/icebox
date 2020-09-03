using System;
using System.Collections.Generic;
using System.Reflection;
using Xunit;
using AutoFixture.Xunit2;
using FluentAssertions;
using Moq;

namespace Icebox.Test
{
    public sealed class IceboxTest
    {
        public sealed class FindMatchingTypesToIceboxedContracts
        {
            private class TestSample
            {
                public string Id { get; set; }   
            }
            
            [Theory, AutoData]
            public void ShouldNotDetectAnyBreakingChangesSinceThereAreNone()
            {
                var assemblyMock = new Mock<Assembly>();
                assemblyMock.Setup(a => a.GetTypes()).Returns(new []
                {
                    typeof(TestSample)
                });

                var sut = new Icebox(assemblyMock.Object);

                var contracts = new List<IceboxedContract>
                {
                    new IceboxedContract(nameof(TestSample), new List<IceboxedContractMember>
                    {
                        new IceboxedContractMember(typeof(string), "Id")
                    })
                };

                var result = sut.FindMatchingTypesToIceboxedContracts(contracts);

                result.Succeeded.Should().BeTrue();
            }
            
            [Theory, AutoData]
            public void ShouldDetectABreakingChangeOfMissingProperty()
            {
                var assemblyMock = new Mock<Assembly>();
                assemblyMock.Setup(a => a.GetTypes()).Returns(new []
                {
                    typeof(TestSample)
                });

                var sut = new Icebox(assemblyMock.Object);

                var contracts = new List<IceboxedContract>
                {
                    new IceboxedContract(nameof(TestSample), new List<IceboxedContractMember>
                    {
                        new IceboxedContractMember(typeof(string), "Id"),
                        new IceboxedContractMember(typeof(string), "Name")
                    })
                };

                var result = sut.FindMatchingTypesToIceboxedContracts(contracts);

                result.Succeeded.Should().BeFalse();
            }
            
            [Theory, AutoData]
            public void ShouldDetectABreakingDatatypeChange()
            {
                var assemblyMock = new Mock<Assembly>();
                assemblyMock.Setup(a => a.GetTypes()).Returns(new []
                {
                    typeof(TestSample)
                });

                var sut = new Icebox(assemblyMock.Object);

                var contracts = new List<IceboxedContract>
                {
                    new IceboxedContract(nameof(TestSample), new List<IceboxedContractMember>
                    {
                        new IceboxedContractMember(typeof(int), "Id")
                    })
                };

                var result = sut.FindMatchingTypesToIceboxedContracts(contracts);

                result.Succeeded.Should().BeFalse();
            }
        }
    }
}