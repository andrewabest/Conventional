using System;
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
                // Todo, need to add this to static Convention.Solution.Net45 factory
                new IfAndElseMustHaveBracesAnalyzerConventionSpecification())
                .All(x => x.IsSatisfied)
                .Should()
                .BeTrue();
        } 
    }
}