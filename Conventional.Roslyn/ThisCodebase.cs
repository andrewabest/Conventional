using System.Collections.Generic;
using Conventional.Roslyn.Conventions;
using Microsoft.CodeAnalysis.MSBuild;

namespace Conventional.Roslyn
{
    public static class ThisCodebase
    {
        public static IEnumerable<ConventionResult> MustConformTo(ISolutionDiagnosticAnalyzerConventionSpecification convention)
        {
            var workspace = MSBuildWorkspace.Create();

            var solution = workspace.OpenSolutionAsync(KnownPaths.FullPathToSolution).Result;

            return Conformist.EnforceConformance(
                convention.IsSatisfiedBy(solution));
        } 
    }
}
