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
        public void MustHaveFilesWithACertainExtensionBeResources_FailsWhenFilesAreNotResources_Regex()
        {
            var expectedFailureMessage = @"
All files matching '.+\.txt' within assembly 'Conventional.Tests' must have their build action set to 'Resource'
- Conventional\Conventions\Assemblies\non_embedded_text_file_first.txt [type=Content]
- Conventional\Conventions\Assemblies\non_embedded_text_file_second.txt [type=Content]
".Trim();

            var result = typeof(AssemblyConventionSpecificationTests).Assembly
                .MustConformTo(Convention.MustHaveFilesBeResources(new Regex(@".+\.txt")));

            result.IsSatisfied.Should().BeFalse();
            result.Failures.Single().Should().Be(expectedFailureMessage);
        }


        [Test]
        public void MustHaveFilesWithACertainExtensionBeResources_FailsWhenFilesAreNotResources_FileExtension()
        {
            var expectedFailureMessage = @"
All files matching '*.txt' within assembly 'Conventional.Tests' must have their build action set to 'Resource'
- Conventional\Conventions\Assemblies\non_embedded_text_file_first.txt [type=Content]
- Conventional\Conventions\Assemblies\non_embedded_text_file_second.txt [type=Content]
".Trim();

            var result = typeof(AssemblyConventionSpecificationTests).Assembly
                .MustConformTo(Convention.MustHaveFilesBeResources("*.txt"));

            result.IsSatisfied.Should().BeFalse();
            result.Failures.Single().Should().Be(expectedFailureMessage);
        }


        [Test]
        public void MustHaveFilesWithACertainExtensionBeEmbeddedResources_FailsWhenFilesAreNotEmbeddedResources_FileExtension()
        {
            var expectedFailureMessage = @"
All files matching '*.txt' within assembly 'Conventional.Tests' must have their build action set to 'Embedded Resource'
- Conventional\Conventions\Assemblies\non_embedded_text_file_first.txt [type=Content]
- Conventional\Conventions\Assemblies\non_embedded_text_file_second.txt [type=Content]
".Trim();

            var result = typeof (AssemblyConventionSpecificationTests).Assembly
                .MustConformTo(Convention.MustHaveFilesBeEmbeddedResources("*.txt"));

            result.IsSatisfied.Should().BeFalse();
            result.Failures.Single().Should().Be(expectedFailureMessage);
        }


        [Test]
        public void MustHaveFilesWithACertainExtensionBeEmbeddedResources_FailsWhenFilesAreNotEmbeddedResources_RegEx()
        {
            var expectedFailureMessage = @"
All files matching '.*NON_EMBEDDED.*' within assembly 'Conventional.Tests' must have their build action set to 'Embedded Resource'
- Conventional\Conventions\Assemblies\non_embedded_text_file_first.txt [type=Content]
- Conventional\Conventions\Assemblies\non_embedded_text_file_second.txt [type=Content]
".Trim();

            var matchNonEmbeddedRegEx = new Regex(".*NON_EMBEDDED.*", RegexOptions.IgnoreCase);

            var result = typeof(AssemblyConventionSpecificationTests).Assembly
                .MustConformTo(Convention.MustHaveFilesBeEmbeddedResources(matchNonEmbeddedRegEx));

            result.IsSatisfied.Should().BeFalse();
            result.Failures.Single().Should().Be(expectedFailureMessage);
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
            var expectedFailureMessage = @"
All files matching '.png' within assembly 'Conventional.Tests' must have their build action set to 'Content - Copy if newer'
- Conventional\Conventions\Assemblies\copy-not.png [type=Content]
".Trim();

            var result = typeof(AssemblyConventionSpecificationTests).Assembly
                .MustConformTo(Convention.MustHaveFilesBeContent(".png"));

            result.IsSatisfied.Should().BeFalse();
            result.Failures.Single().Should().Be(expectedFailureMessage);
        }

        [Test]
        public void MustHaveCertainFilesBeContentCopyIfNewer_Regex()
        {
            var expectedFailureMessage = @"
All files matching '.*\.png' within assembly 'Conventional.Tests' must have their build action set to 'Content - Copy if newer'
- Conventional\Conventions\Assemblies\copy-not.png [type=Content]
".Trim();

            var result = typeof(AssemblyConventionSpecificationTests).Assembly
                .MustConformTo(Convention.MustHaveFilesBeContent(new Regex(@".*\.png")));

            result.IsSatisfied.Should().BeFalse();
            result.Failures.Single().Should().Be(expectedFailureMessage);
        }

    }
}