using System;
using System.Linq;
using System.Reflection;
using Conventional.Conventions.Assemblies;
using FluentAssertions;
using NUnit.Framework;

namespace Conventional.Tests.Conventional.Conventions.Assemblies
{
    public class MatchingFilesConventionTests
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
        public void MustIncludeAllMatchingFilesInFolder_SucceedsWhenAllMatchingFilesAreCompiled()
        {
            var result = _testAssembly.MustConformTo(Convention.MustIncludeAllMatchingFilesInFolder("*.cs"));
            result.IsSatisfied.Should().BeTrue();
            result.Failures.Should().HaveCount(0);
        }

        [Test]
        public void MustIncludeAllMatchingFilesInFolder_SucceedsWhenAllMatchingFilesAreResources()
        {
            var result = _testAssembly.MustConformTo(Convention.MustIncludeAllMatchingFilesInFolder("*.sql"));
            result.IsSatisfied.Should().BeTrue();
            result.Failures.Should().HaveCount(0);
        }

        [Test]
        public void MustIncludeAllMatchingFilesInFolder_SucceedsWhenAllMatchingFilesAreContent()
        {
            var result = _testAssembly.MustConformTo(Convention.MustIncludeAllMatchingFilesInFolder("*.txt"));
            result.IsSatisfied.Should().BeTrue();
            result.Failures.Should().HaveCount(0);
        }

        [Test]
        public void MustIncludeAllMatchingFilesInFolder_ProducesAppropriateErrorMessage()
        {
            var result = _testAssembly.MustConformTo(Convention.MustIncludeAllMatchingFilesInFolder("*.js"));
            var failureText = result.Failures.Single();
            failureText.Should().Contain(@"All files matching '*.js' within ");
            failureText.Should().Contain(@"\TestSolution\TestSolution.TestProject' must be included in the project.");
            failureText.Should().Contain(@"\TestSolution\TestSolution.TestProject\Scripts\unincludedJsFile.js");
            failureText.Split(new[] {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries).Length.Should().Be(2);
        }

        [Test]
        public void WhenAnExtensionIsExcluded_FilesWithThatExtensionAreIgnored()
        {
            var result = _testAssembly.MustConformTo(Convention.MustIncludeAllMatchingFilesInFolder("*.*").WithExcludedExtensions("csproj", ".SLN", ".js", ".suo", ".user"));
            result.IsSatisfied.Should().BeTrue();
        }

        [Test]
        public void WhenASubfolderIsProvided_FilesOutsideThatSubfolderMayBeLeftOut()
        {
            var result = _testAssembly.MustConformTo(Convention.MustIncludeAllMatchingFilesInFolder("*.csproj", "Scripts"));
            result.IsSatisfied.Should().BeTrue();
        }
    }
}