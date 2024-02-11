using System;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace Conventional.Roslyn.Tests.Conventions
{
    public class SolutionDiagnosticAnalyzerConventionSpecificationTests
    {
        public SolutionDiagnosticAnalyzerConventionSpecificationTests()
        {
            if (true)
                Console.WriteLine("Ignore Me - I need to be in the source code");
        }

        [Test]
        public void KnownOffendersAreIgnored()
        {
            ThisCodebase
                .MustConformTo(
                    RoslynConvention.IfAndElseMustHaveBraces(), 1)
                .All(x => x.IsSatisfied)
                .Should()
                .BeTrue();
        }

        [Test]
        public void KnownOffenderFailTheResult()
        {
            ThisCodebase
                .MustConformTo(
                    RoslynConvention.IfAndElseMustHaveBraces(), 0)
                .All(x => x.IsSatisfied)
                .Should()
                .BeFalse();
        }

                [Test]
        public void WeirdKnownOffenderNumbersDontFailTheResult()
        {
            ThisCodebase
                .MustConformTo(
                    RoslynConvention.IfAndElseMustHaveBraces(), -42)
                .All(x => x.IsSatisfied)
                .Should()
                .BeFalse();
        }

        [Test]
        public void IfAndElseMustHaveBracesAnalyzer_Success()
        {
            using (new TestSolution("TestSolutionSuccess"))
            {
                ThisCodebase.MustConformTo(
                        RoslynConvention.IfAndElseMustHaveBraces())
                    .All(x => x.IsSatisfied)
                    .Should()
                    .BeTrue();
            }
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

        [Test]
        public void UsingStatementsMustNotBeNestedAnalyzer_Success()
        {
            using (new TestSolution("TestSolutionSuccess"))
            {
                ThisCodebase.MustConformTo(
                        RoslynConvention.UsingStatementsMustNotBeNested())
                    .All(x => x.IsSatisfied)
                    .Should()
                    .BeTrue();
            }
        }

        [Test]
        public void UsingStatementsMustNotBeNestedAnalyzer_FailesWhenFileHasUsingsInsideNamespace()
        {
            using (new TestSolution("TestSolution"))
            {
                ThisCodebase.MustConformTo(
                        RoslynConvention.UsingStatementsMustNotBeNested())
                    .Should()
                    .Contain(x => x.IsSatisfied == false && x.SubjectName.EndsWith("SplitNamespaces.cs"));
            }
        }
    }
}