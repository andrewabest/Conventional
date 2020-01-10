using System.IO;
using FluentAssertions;
using NUnit.Framework;

namespace Conventional.Tests
{
    public class ThisAssemblyScenarios
    {
        [Test]
        public void GivenAPattern_LocatesAndReturnsAssemblyForThatPattern()
        {
            var assemblySpecimen = TheAssembly.WithNameMatching("TestSolution.TestProject");

            assemblySpecimen.ProjectFilePath.Should()
                .Match(x => x.EndsWith(
                    $"TestSolution{Path.DirectorySeparatorChar}TestSolution.TestProject{Path.DirectorySeparatorChar}TestSolution.TestProject.csproj"));
        }
    }
}