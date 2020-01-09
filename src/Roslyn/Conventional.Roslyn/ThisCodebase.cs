using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using Conventional.Roslyn.Conventions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.MSBuild;

namespace Conventional.Roslyn
{
    public static class ThisCodebase
    {
        public static IEnumerable<ConventionResult> MustConformTo(ISolutionDiagnosticAnalyzerConventionSpecification convention)
        {
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
