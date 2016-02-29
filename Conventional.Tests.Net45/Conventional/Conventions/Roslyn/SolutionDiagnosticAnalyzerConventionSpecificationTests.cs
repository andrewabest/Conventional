using System.Linq;
using Conventional.Conventions.Roslyn;
using FluentAssertions;
using NUnit.Framework;

namespace Conventional.Tests.Net45.Conventional.Conventions.Roslyn
{
    public class SolutionDiagnosticAnalyzerConventionSpecificationTests
    {
        [Test]
        public void IfAndElseMustHaveBracesAnalyzer_Success()
        {
            ThisSolution.MustConformTo(
                Convention.IfAndElseMustHaveBraces())
                .All(x => x.IsSatisfied)
                .Should()
                .BeTrue();
        }

        [Test]
        public void IfAndElseMustHaveBracesAnalyzer_FailsWhenIfBlockDoesNotHaveBrace()
        {
            using (new TestSolution("TestSolution"))
            {
                ThisSolution.MustConformTo(
                    Convention.IfAndElseMustHaveBraces())
                    .Where(x => x.IsSatisfied == false)
                    .Should()
                    .Contain(x => x.IsSatisfied == false && x.SubjectName.EndsWith("IfBracelessWonder.cs"));
            }
        }

        [Test]
        public void IfAndElseMustHaveBracesAnalyzer_FailsWhenElseBlockDoesNotHaveBrace()
        {
            using (new TestSolution("TestSolution"))
            {
                ThisSolution.MustConformTo(
                    Convention.IfAndElseMustHaveBraces())
                    .Should()
                    .Contain(x => x.IsSatisfied == false && x.SubjectName.EndsWith("ElseBracelessWonder.cs"));
            }
        }
    }
}