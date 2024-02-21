using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Conventional.Roslyn.Conventions;
using Microsoft.Build.Locator;
using Microsoft.CodeAnalysis.MSBuild;

namespace Conventional.Roslyn
{
    public static class ThisCodebase
    {
        public static IEnumerable<ConventionResult> MustConformTo(ISolutionDiagnosticAnalyzerConventionSpecification convention, int knownOffenders = 0)
        {
            // Locate and register the default instance of MSBuild installed on this machine.
            // https://github.com/dotnet/roslyn/issues/17974#issuecomment-624408861
            if (!MSBuildLocator.IsRegistered)
            {
                MSBuildLocator.RegisterDefaults();
            }

            var workspace = MSBuildWorkspace.Create();

            var solution = workspace.OpenSolutionAsync(KnownPaths.FullPathToSolution).Result;

            foreach (var diagnostic in workspace.Diagnostics)
            {
                Trace.WriteLine(diagnostic.Message);
            }

            var conventionResults = Conformist.EnforceConformance(convention.IsSatisfiedBy(solution)).ToList();
            var remainingOffenders = CalculateRemainingOffenders(conventionResults, knownOffenders);
            return RecombineSatisfactoryResultsWithRemainingOffenders(conventionResults, remainingOffenders);
        }

        private static IEnumerable<ConventionResult> CalculateRemainingOffenders(IEnumerable<ConventionResult> results, int knownOffenders)
        {
            var failures = results.Where(x => !x.IsSatisfied).ToList();
            return failures.Take(Math.Max(failures.Count - Math.Max(knownOffenders, 0), 0));
        }

        private static IEnumerable<ConventionResult> RecombineSatisfactoryResultsWithRemainingOffenders(IEnumerable<ConventionResult> conventionResults, IEnumerable<ConventionResult> remainingOffenders)
        {
            var results = conventionResults.Where(x => x.IsSatisfied).ToList();
            results.AddRange(remainingOffenders);
            return results;
        }
    }
}