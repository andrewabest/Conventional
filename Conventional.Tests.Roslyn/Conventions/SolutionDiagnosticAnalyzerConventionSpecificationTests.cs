using System.Linq;
using Conventional.Roslyn;
using NUnit.Framework;
using FluentAssertions;

namespace Conventional.Tests.Roslyn.Conventions
{
    public class SolutionDiagnosticAnalyzerConventionSpecificationTests
    {
        [Test]
        public void IfAndElseMustHaveBracesAnalyzer_Success()
        {
            ThisCodebase.MustConformTo(
                RoslynConvention.IfAndElseMustHaveBraces())
                .All(x => x.IsSatisfied)
                .Should()
                .BeTrue();
        }

        [Test]
        public void IfAndElseMustHaveBracesAnalyzer_FailsWhenIfBlockDoesNotHaveBrace()
        {
            using (new TestSolution("TestSolution"))
            {
                ThisCodebase.MustConformTo(
                    RoslynConvention.IfAndElseMustHaveBraces())
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
                ThisCodebase.MustConformTo(
                    RoslynConvention.IfAndElseMustHaveBraces())
                    .Should()
                    .Contain(x => x.IsSatisfied == false && x.SubjectName.EndsWith("ElseBracelessWonder.cs"));
            }
        }
    }
}