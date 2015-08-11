using System.Linq;
using NUnit.Framework;
using FluentAssertions;

namespace Conventional.Tests.Conventional.Conventions.Solution
{
    public class SolutionConventionSpecificationTests
    {
        [Test]
        public void Dogfooding_MustOnlyContainToDoAndNoteComments()
        {
            ThisSolution
                .MustConformTo(Convention.MustOnlyContainToDoAndNoteComments)
                .WithFailureAssertion(Assert.Fail);
        }


        [Test]
        public void FilesMustBeEmbeddedResourcesConvention_Success()
        {
            ThisSolution
                .MustConformTo(Convention.FilesMustBeEmbeddedResources("*.sql"))
                .IsSatisfied
                .Should()
                .BeTrue();
        }

        [Test]
        public void FilesMustBeEmbeddedResourcesConvention_FailsWhenFilesAreNotEmbeddedResources()
        {
            var expectedFailureMessage = @"
All files with the extension '*.txt' within this solution must have their build action set to 'Embedded Resource'
- Conventional.Tests\Conventional\Conventions\Solution\non_embedded_text_file_first.txt
- Conventional.Tests\Conventional\Conventions\Solution\non_embedded_text_file_second.txt
".Trim();

            var result = ThisSolution.MustConformTo(Convention.FilesMustBeEmbeddedResources("*.txt"));

            result.IsSatisfied.Should().BeFalse();
            result.Failures.Single().Should().Be(expectedFailureMessage);
        }
    }
}