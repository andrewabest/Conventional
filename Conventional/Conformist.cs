using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Conventional.Conventions.Solution;

namespace Conventional
{
    public static class Conformist
    {
        public static ConventionResult MustConformTo(this Type type, IConventionSpecification conventionSpecification)
        {
            return EnforceConformance(
                conventionSpecification.IsSatisfiedBy(type));
        }

        public static WrappedConventionResult MustConformTo(this IEnumerable<Type> types, IConventionSpecification conventionSpecification)
        {
            return EnforceConformance(new WrappedConventionResult(
                types, 
                types.Select(conventionSpecification.IsSatisfiedBy)));
        }

        public static ConventionResult MustConformTo(this Assembly assembly, IAssemblyConventionSpecification assemblyConventionSpecification)
        {
            return EnforceConformance(
                assemblyConventionSpecification.IsSatisfiedBy(assembly));
        }

        public static ConventionResult MustConformTo(this AssemblySpecimen assemblySpecimen, IAssemblyConventionSpecification assemblyConventionSpecification)
        {
            return EnforceConformance(
                assemblyConventionSpecification.IsSatisfiedBy(assemblySpecimen.ProjectFilePath));
        }

        public static IEnumerable<Type> WhereTypes(this IEnumerable<Assembly> assemblies, Func<Type, bool> predicate)
        {
            var types = assemblies.SelectMany(x => x.GetExportedTypes()).Where(predicate).ToArray();

            return types;
        }

        public static WrappedConventionResult AndMustConformTo(this WrappedConventionResult results, IConventionSpecification conventionSpecification)
        {
            return EnforceConformance(new WrappedConventionResult(
                results.Types,
                results.Results.Union(results.Types.Select(conventionSpecification.IsSatisfiedBy))));
        }

        private static ConventionResult EnforceConformance(ConventionResult result)
        {
            if (Configuration.DefaultFailureAssertionCallback != null)
            {
                new[] { result }.WithFailureAssertion(Configuration.DefaultFailureAssertionCallback);
            }

            return result;
        }

        private static WrappedConventionResult EnforceConformance(WrappedConventionResult results)
        {
            if (Configuration.DefaultFailureAssertionCallback != null)
            {
                results.WithFailureAssertion(Configuration.DefaultFailureAssertionCallback);
            }

            return results;
        }

        public static void WithFailureAssertion(this ConventionResult result, Action<string> failureAssertion)
        {
            new[] {result}.WithFailureAssertion(failureAssertion);
        }

        public static void WithFailureAssertion(this IEnumerable<ConventionResult> results, Action<string> failureAssertion)
        {
            var evaluatedResults = results.ToList();

            if (evaluatedResults.All(x => x.IsSatisfied))
            {
                return;
            }

            var result =
                evaluatedResults.Where(x => x.IsSatisfied == false).Aggregate(string.Empty, (s, x) =>
                    s +
                    x.TypeName +
                    Environment.NewLine +
                    StringConstants.Underline +
                    Environment.NewLine +
                    x.Failures.Aggregate(string.Empty, (s1, s2) => s1 + s2 + Environment.NewLine) +
                    Environment.NewLine);

            failureAssertion(result);
        }
    }
}