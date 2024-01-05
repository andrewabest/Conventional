using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
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
            _testAssembly = Assembly.LoadFrom(KnownPaths.SolutionRoot +
                $"TestSolution{Path.DirectorySeparatorChar}TestSolution.TestProject{Path.DirectorySeparatorChar}bin{Path.DirectorySeparatorChar}Debug{Path.DirectorySeparatorChar}TestSolution.TestProject.dll");
        }

        [Test]
        public void MustNotReferenceDllsFromTransientOrSdkDirectories_FailsWhenAssemblyReferencesDllsFromBinDirectory()
        {
            var result = _testAssembly.MustConformTo(Convention.MustNotReferenceDllsFromTransientOrSdkDirectories);
            result.IsSatisfied.Should().BeFalse();
            result.Failures.Should().HaveCount(1);
        }

        [Test]
        public void MustNotReferenceDllsFromTransientOrSdkDirectories_FailsWhenAssemblyReferencesDllsFromReferenceAssembliesDirectory()
        {
            var result = _testAssembly.MustConformTo(Convention.MustNotReferenceDllsFromTransientOrSdkDirectories);
            result.IsSatisfied.Should().BeFalse();
            result.Failures.Should().HaveCount(1);
        }

        [Test]
        public void MustNotReferenceDllsFromTransientOrSdkDirectories_Success()
        {
            var result = typeof(AssemblyConventionSpecificationTests).Assembly.MustConformTo(Convention.MustNotReferenceDllsFromTransientOrSdkDirectories);
            result.IsSatisfied.Should().BeTrue();
        }

        [Test]
        public void MustHaveFilesWithACertainExtensionBeResources_Success_FileExtension()
        {
            typeof(AssemblyConventionSpecificationTests).Assembly
                .MustConformTo(Convention.MustHaveFilesBeResources(".resx"))
                .IsSatisfied
                .Should()
                .BeTrue();
        }

        [Test]
        public void MustHaveFilesWithACertainExtensionBeResources_Success_RegEx()
        {
            var matchResxFiles = new Regex(@"\.RESX$", RegexOptions.IgnoreCase);

            typeof(AssemblyConventionSpecificationTests).Assembly
                .MustConformTo(Convention.MustHaveFilesBeResources(matchResxFiles))
                .IsSatisfied
                .Should()
                .BeTrue();
        }

        [Test]
        public void MustHaveFilesWithACertainExtensionBeResources_FailsWhenFilesAreNotResources_Regex()
        {
            var result = typeof(AssemblyConventionSpecificationTests).Assembly
                .MustConformTo(Convention.MustHaveFilesBeResources(new Regex(@".\.txt")));

            result.IsSatisfied.Should().BeFalse();
            result.Failures.Single().Should().Contain("non_embedded_text_file_first.txt");
            result.Failures.Single().Should().Contain("non_embedded_text_file_second.txt");
        }


        [Test]
        public void MustHaveFilesWithACertainExtensionBeResources_FailsWhenFilesAreNotResources_FileExtension()
        {
            var result = typeof(AssemblyConventionSpecificationTests).Assembly
                .MustConformTo(Convention.MustHaveFilesBeResources("*.txt"));

            result.IsSatisfied.Should().BeFalse();
            result.Failures.Single().Should().Contain("non_embedded_text_file_first.txt");
            result.Failures.Single().Should().Contain("non_embedded_text_file_second.txt");
        }


        [Test]
        public void MustHaveFilesWithACertainExtensionBeEmbeddedResources_Success_WithWildCardedFileExtension()
        {
            typeof(AssemblyConventionSpecificationTests).Assembly
                .MustConformTo(Convention.MustHaveFilesBeEmbeddedResources("*.sql"))
                .IsSatisfied
                .Should()
                .BeTrue();
        }

        [Test]
        public void MustHaveFilesWithACertainExtensionBeEmbeddedResources_Success_WithNonWildcardedFileExtension()
        {
            typeof(AssemblyConventionSpecificationTests).Assembly
                .MustConformTo(Convention.MustHaveFilesBeEmbeddedResources("sql"))
                .IsSatisfied
                .Should()
                .BeTrue();
        }


        [Test]
        public void MustHaveFilesWithACertainExtensionBeEmbeddedResources_Success_RegEx()
        {
            var matchSqlFiles = new Regex(@"\.SQL$", RegexOptions.IgnoreCase);

            typeof(AssemblyConventionSpecificationTests).Assembly
                .MustConformTo(Convention.MustHaveFilesBeEmbeddedResources(matchSqlFiles))
                .IsSatisfied
                .Should()
                .BeTrue();
        }

        [Test]
        public void
            MustHaveFilesWithACertainExtensionBeEmbeddedResources_FailsWhenFilesAreNotEmbeddedResources_FileExtension()
        {
            var result = typeof(AssemblyConventionSpecificationTests).Assembly
                .MustConformTo(Convention.MustHaveFilesBeEmbeddedResources("*.txt"));

            result.IsSatisfied.Should().BeFalse();
            result.Failures.Single().Should().Contain("non_embedded_text_file_first.txt");
            result.Failures.Single().Should().Contain("non_embedded_text_file_second.txt");
        }


        [Test]
        public void MustHaveFilesWithACertainExtensionBeEmbeddedResources_FailsWhenFilesAreNotEmbeddedResources_RegEx()
        {
            var matchNonEmbeddedRegEx = new Regex(".*NON_EMBEDDED.*", RegexOptions.IgnoreCase);

            var result = typeof(AssemblyConventionSpecificationTests).Assembly
                .MustConformTo(Convention.MustHaveFilesBeEmbeddedResources(matchNonEmbeddedRegEx));

            result.IsSatisfied.Should().BeFalse();
            result.Failures.Single().Should().Contain("non_embedded_text_file_first.txt");
            result.Failures.Single().Should().Contain("non_embedded_text_file_second.txt");
        }

        [Test]
        public void MustHaveCertainFilesBeContentCopyIfNewer_FileExtension_Success()
        {
            var result = typeof(AssemblyConventionSpecificationTests).Assembly
                .MustConformTo(Convention.MustHaveFilesBeContent(".svg"));

            result.IsSatisfied.Should().BeTrue();
        }

        [Test]
        public void MustHaveCertainFilesBeContentCopyIfNewer_Regex_Success()
        {
            var result = typeof(AssemblyConventionSpecificationTests).Assembly
                .MustConformTo(Convention.MustHaveFilesBeContent(new Regex(@".+\.svg")));

            result.IsSatisfied.Should().BeTrue();
        }

        [Test]
        public void MustHaveCertainFilesBeContentCopyIfNewer_FileExtension()
        {
            var result = typeof(AssemblyConventionSpecificationTests).Assembly
                .MustConformTo(Convention.MustHaveFilesBeContent(".png"));

            result.IsSatisfied.Should().BeFalse();
            result.Failures.Single().Should().EndWith("copy-not.png");
        }

        [Test]
        public void MustHaveCertainFilesBeContentCopyIfNewer_Regex()
        {
            var result = typeof(AssemblyConventionSpecificationTests).Assembly
                .MustConformTo(Convention.MustHaveFilesBeContent(new Regex(@".*\.png")));

            result.IsSatisfied.Should().BeFalse();
            result.Failures.Single().Should().EndWith("copy-not.png");
        }

        private AssemblySpecimen[] TestProjects =>
            AllAssemblies.WithNamesMatching("*")
                .Where(specimen => specimen.ProjectFilePath.Contains("Tests"))
                .ToArray();

        // Note: In practice, this list of assemblies would be used to drive further convention tests (i.e. assembly.GetTypes())
        private static readonly List<Assembly> TestAssemblies = new List<Assembly>
        {
            typeof(DogFoodConventions).Assembly
        };

        [Test]
        public void MustBeIncludedInSetOfAssemblies_Success()
        {
            var result = TestProjects
                .MustConformTo(Convention.MustBeIncludedInSetOfAssemblies(TestAssemblies, "TestAssemblies"));

            // TODO: Use result.Should().AllSatisfy() once we've updated to fluentassertions 6.5.0+
            result.Select(x => x.IsSatisfied).Distinct().Single().Should().BeTrue();
        }

        [Test]
        public void MustBeIncludedInSetOfAssemblies_Failure()
        {
            // ReSharper disable once CollectionNeverUpdated.Local
            var staleTestAssemblies = new List<Assembly>();

            var result = TestProjects
                .MustConformTo(Convention.MustBeIncludedInSetOfAssemblies(staleTestAssemblies, "TestAssemblies"));

            // TODO: Use result.Should().AllSatisfy() once we've updated to fluentassertions 6.5.0+
            result.Select(x => x.IsSatisfied).Distinct().Single().Should().BeFalse();
        }

        [Test]
        public void MustNotIncludeProjectReferences_Success()
        {
            var result = TheAssembly
                .WithNameMatching("TestProjectTwo")
                .MustConformTo(Convention.MustNotIncludeProjectReferences);

            // Note: TestProjectTwo doesn't import any other projects (at time of writing)
            result.IsSatisfied.Should().BeTrue();
        }

        [Test]
        public void MustNotIncludeProjectReferences_Failure()
        {
            var result = TheAssembly
                .WithNameMatching("Conventional.Tests")
                .MustConformTo(Convention.MustNotIncludeProjectReferences); // Note: Conventional.Tests of course includes a reference to Conventional

            result.IsSatisfied.Should().BeFalse();
            result.Failures.Single().Should().StartWith("Conventional.Tests includes reference to project");
        }

        [Test]
        public void MustReferencePackage_Success()
        {
            var result = TheAssembly
                .WithNameMatching("SdkClassLibrary1")
                .MustConformTo(Convention.MustReferencePackage("coverlet.collector"));

            result.IsSatisfied.Should().BeTrue();
        }

        [Test]
        public void MustReferencePackage_Failure()
        {
            var result = TheAssembly
                .WithNameMatching("SdkClassLibrary1")
                .MustConformTo(Convention.MustReferencePackage("koverlet.kollector"));

            result.IsSatisfied.Should().BeFalse();
        }

        [Test]
        public void MustNotReferencePackage_Success()
        {
            var result = TheAssembly
                .WithNameMatching("SdkClassLibrary1")
                .MustConformTo(Convention.MustNotReferencePackage("foo.bar.baz"));

            result.IsSatisfied.Should().BeTrue();
        }

        [Test]
        public void MustNotReferencePackage_Failure()
        {
            var result = TheAssembly
                .WithNameMatching("SdkClassLibrary1")
                .MustConformTo(Convention.MustNotReferencePackage("coverlet.collector"));

            result.IsSatisfied.Should().BeFalse();
        }

        [Test]
        public void MustSetPropertyValue_SingleValue_Success()
        {
            var result = TheAssembly
                .WithNameMatching("SdkClassLibrary1")
                .MustConformTo(Convention.MustSetPropertyValue("TheUniversalAnswer", "42"));

            result.IsSatisfied.Should().BeTrue();
        }

        [Theory]
        [TestCase("Potato")]
        [TestCase("Carrot")]
        public void MustSetPropertyValue_MultipleValues_Success(string value)
        {
            var result = TheAssembly
                .WithNameMatching("SdkClassLibrary1")
                .MustConformTo(Convention.MustSetPropertyValue("Vegetable", value));

            result.IsSatisfied.Should().BeTrue();
        }

        [Test]
        public void MustSetPropertyValue_SingleValue_Failure()
        {
            var result = TheAssembly
                .WithNameMatching("SdkClassLibrary1")
                .MustConformTo(Convention.MustSetPropertyValue("TheUniversalAnswer", "41.999"));

            result.IsSatisfied.Should().BeFalse();
            result.Failures.Single().Should().Be("SdkClassLibrary1 should have property TheUniversalAnswer with value 41.999");
        }

        [Test]
        public void MustSetPropertyValue_MultipleValues_Failure()
        {
            var result = TheAssembly
                .WithNameMatching("SdkClassLibrary1")
                .MustConformTo(Convention.MustSetPropertyValue("Vegetable", "Turnip")); // Note: Assumes no <Vegetable>Turnip</Vegetable> in the csproj

            result.IsSatisfied.Should().BeFalse();
            result.Failures.Single().Should().Be("SdkClassLibrary1 should have property Vegetable with value Turnip");
        }

        [Test]
        public void MustSetPropertyValue_NoValues_Failure()
        {
            var result = TheAssembly
                .WithNameMatching("SdkClassLibrary1")
                .MustConformTo(Convention.MustSetPropertyValue("ThisPropertyShouldNeverEverExist", "x")); // Note: Assumes no <ThisPropertyShouldNeverEverExist>x</ThisPropertyShouldNeverEverExist> in the csproj

            result.IsSatisfied.Should().BeFalse();
            result.Failures.Single().Should().Be("SdkClassLibrary1 should have property ThisPropertyShouldNeverEverExist with value x");
        }
    }
}