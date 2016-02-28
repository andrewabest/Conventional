using System.Collections.Generic;
using System.IO;
using Conventional.Conventions.Roslyn;
using Microsoft.CodeAnalysis.MSBuild;

namespace Conventional
{
    public static partial class ThisSolution
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
