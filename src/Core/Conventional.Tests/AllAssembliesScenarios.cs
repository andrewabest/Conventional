using FluentAssertions;
using NUnit.Framework;

namespace Conventional.Tests
{
    public class AllAssembliesScenarios
    {
        [Test]
        public void GivenAPattern_LocatesAndReturnsAllAssembliesForThatPattern()
        {
            var assemblySpecimen = AllAssemblies.WithNamesMatching("*");

            assemblySpecimen.Should().HaveCount(6);
        }
    }
}