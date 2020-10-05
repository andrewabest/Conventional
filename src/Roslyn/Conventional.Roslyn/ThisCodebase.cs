using System.Collections.Generic;
using System.Diagnostics;
using Conventional.Roslyn.Conventions;
using Microsoft.Build.Locator;
using Microsoft.CodeAnalysis.MSBuild;

namespace Conventional.Roslyn
{
    public static class ThisCodebase
    {
        public static IEnumerable<ConventionResult> MustConformTo(ISolutionDiagnosticAnalyzerConventionSpecification convention)
        {
            // Locate and register the default instance of MSBuild installed on this machine.
            // https://github.com/dotnet/roslyn/issues/17974#issuecomment-624408861
            if (!MSBuildLocator.IsRegistered)
            {
                MSBuildLocator.RegisterDefaults();
            }

            var workspace = MSBuildWorkspace.Create();

            var solution = workspace.OpenSolutionAsync(KnownPaths.FullPathToSolution).Result;

            foreach(var diagnostic in workspace.Diagnostics)
            {
                Trace.WriteLine(diagnostic.Message);
            }

            return Conformist.EnforceConformance(
                convention.IsSatisfiedBy(solution));
        }
    }
}
