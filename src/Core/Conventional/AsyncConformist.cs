using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Conventional
{
    public static class AsyncConformist
    {
        public static async Task<ConventionResult> MustConformTo(this Type type, IAsyncConventionSpecification conventionSpecification)
        {
            return Conformist.EnforceConformance(
                await conventionSpecification.IsSatisfiedBy(type));
        }

        public static async Task<WrappedConventionResult> MustConformTo(this IEnumerable<Type> types, IAsyncConventionSpecification conventionSpecification)
        {
            var filteredTypes = types.Where(ConventionConfiguration.GlobalTypeFilter).ToArray();

            return Conformist.EnforceConformance(new WrappedConventionResult(
                filteredTypes,
                await Task.WhenAll(filteredTypes.Select(conventionSpecification.IsSatisfiedBy))));
        }

        public static async Task<WrappedConventionResult> AndMustConformTo(this Task<WrappedConventionResult> results, IAsyncConventionSpecification conventionSpecification)
        {
            // TODO: Create an AsyncWrappedConventionResult that can store tasks as first-class citizens.
            var previousResults = await results;

            return Conformist.EnforceConformance(new WrappedConventionResult(
                previousResults.Types,
                previousResults.Results.Union(await Task.WhenAll(previousResults.Types.Select(conventionSpecification.IsSatisfiedBy)))));
        }
    }
}