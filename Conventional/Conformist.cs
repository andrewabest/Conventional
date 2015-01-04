using System;
using System.Collections.Generic;
using System.Linq;

namespace Conventional
{
    public static class Conformist
    {
        public static IEnumerable<ConformanceResult> MustConformTo(this Type type, IConformanceSpecification conformanceSpecification)
        {
            return new [] { conformanceSpecification.IsSatisfiedBy(type) };
        }

        public static IEnumerable<ConformanceResult> MustAllConformTo(this IEnumerable<Type> types, IConformanceSpecification conformanceSpecification)
        {
            return types.Select(conformanceSpecification.IsSatisfiedBy);
        }

        public static void WithFailureAssertion(this IEnumerable<ConformanceResult> results, Action<string> failureAssertion)
        {
            var aggregateResult = results.Where(result => result.IsSatisfied == false).Aggregate(string.Empty, (s, result) => s + result.FailureMessage + Environment.NewLine);

            failureAssertion(aggregateResult);
        }
    }
}