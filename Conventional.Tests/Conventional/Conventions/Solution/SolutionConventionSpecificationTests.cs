using NUnit.Framework;

namespace Conventional.Tests.Conventional.Conventions.Solution
{
    public class SolutionConventionSpecificationTests
    {
        [Test]
        public void Dogfooding_MustOnlyContainToDoAndNoteComments()
        {
            ThisSolution
                .MustConformTo(Convention.MustOnlyContainToDoAndNoteComments())
                .WithFailureAssertion(Assert.Fail);
        }
    }
}