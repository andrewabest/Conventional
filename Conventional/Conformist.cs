using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Conventional
{
    public static class Conformist
    {

        public static ConventionResult MustConformTo(this Type type, IConventionSpecification conventionSpecification)
        {
            return conventionSpecification.IsSatisfiedBy(type);
        }

        public static WrappedConventionResult MustConformTo(this IEnumerable<Type> types, IConventionSpecification conventionSpecification)
        {
            return new WrappedConventionResult(
                types, 
                types.Select(conventionSpecification.IsSatisfiedBy));
        }

        public static IEnumerable<Type> WhereTypes(this IEnumerable<Assembly> assemblies, Func<Type, bool> predicate)
        {
            var types = assemblies.SelectMany(x => x.GetExportedTypes()).Where(predicate).ToArray();

            return types;
        }

        public static WrappedConventionResult AndMustConformTo(this WrappedConventionResult results, IConventionSpecification conventionSpecification)
        {
            return new WrappedConventionResult(
                results.Types, 
                results.Results.Union(results.Types.Select(conventionSpecification.IsSatisfiedBy)));
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