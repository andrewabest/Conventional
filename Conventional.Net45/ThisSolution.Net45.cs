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

            // TODO: make a generic search for the .sln file here, allow it to be overridden to return a specific one.
            var solution = workspace.OpenSolutionAsync(KnownPaths.SolutionRoot + "Conventional.sln").Result;

            return Conformist.EnforceConformance(
                convention.IsSatisfiedBy(solution));
        } 
    }
}
