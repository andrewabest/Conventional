using System;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Conventional.Tests.Conventional.Conventions.Assemblies;
using FluentAssertions;
using NUnit.Framework;

namespace Conventional.Tests.Conventional.Conventions.Projects
{
    public class ProjectConventionSpecificationTests
    {
        private Assembly _testAssembly;

        [SetUp]
        public void Setup()
        {
            _testAssembly =
                Assembly.LoadFrom(KnownPaths.SolutionRoot +
                                  "TestSolution/TestSolution.TestProject/bin/Debug/TestSolution.TestProject.dll");
        }

        [Test]
        public void MustIncludeAllMatchingFilesInFolder_FailsWhenThereIsAnUnreferencedFile()
        {
            var result = _testAssembly.MustConformTo(Convention.MustIncludeAllMatchingFilesInFolder("*.js"));
            result.IsSatisfied.Should().BeFalse();
            result.Failures.Should().HaveCount(1);
        }

        [Test]
        public void MustIncludeAllMatchingFilesInFolder_SucceedsWhenAllMatchingFilesAreReferenced()
        {
            var result = _testAssembly.MustConformTo(Convention.MustIncludeAllMatchingFilesInFolder("*.cs"));
            result.IsSatisfied.Should().BeTrue();
            result.Failures.Should().HaveCount(0);
        }

        [Test]
        public void MustIncludeAllMatchingFilesInFolder_ProducesAppropriateErrorMessage()
        {
            var result = _testAssembly.MustConformTo(Convention.MustIncludeAllMatchingFilesInFolder("*.js"));
            result.Should().Be(result.Failures.Single(f => f.Contains(string.Format(@"All files matching '*.js' within project folder for 'I:\Dev\Conventional\TestSolution\TestSolution.TestProject\TestSolution.TestProject.csproj' must be included in the project.{0}- I:\Dev\Conventional\TestSolution\TestSolution.TestProject\unincludedJsFile.js", Environment.NewLine))));
        }
    }
}