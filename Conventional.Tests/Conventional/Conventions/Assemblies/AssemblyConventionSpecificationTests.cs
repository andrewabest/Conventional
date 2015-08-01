using System.IO;
using System.Reflection;
using FluentAssertions;
using NUnit.Framework;

namespace Conventional.Tests.Conventional.Conventions.Assemblies
{
    public class AssemblyConventionSpecificationTests
    {
        private Assembly _testAssembly;

        [SetUp]
        public void Setup()
        {
            _testAssembly = Assembly.LoadFrom(KnownPaths.SolutionRoot + "TestSolution/TestSolution.TestProject/bin/Debug/TestSolution.TestProject.dll");
        }

        [Test]
        public void MustNotReferenceDllsFromBinOrObjDirectories_FailsWhenAssemblyReferencesDllsFromBinDirectory()
        {
            var result = _testAssembly.MustConformTo(Convention.MustNotReferenceDllsFromBinOrObjDirectories);
            result.IsSatisfied.Should().BeFalse();
            result.Failures.Should().HaveCount(1);
        }

        [Test]
        public void MustNotReferenceDllsFromBinOrObjDirectories_Success()
        {
            var result = typeof(AssemblyConventionSpecificationTests).Assembly.MustConformTo(Convention.MustNotReferenceDllsFromBinOrObjDirectories);
            result.IsSatisfied.Should().BeTrue();
        }
    }
}