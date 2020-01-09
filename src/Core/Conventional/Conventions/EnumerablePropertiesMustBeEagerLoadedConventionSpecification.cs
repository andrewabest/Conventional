using System;
using System.Collections;
using System.Linq;
using System.Reflection;

namespace Conventional.Conventions
{
    public class EnumerablePropertiesMustBeEagerLoadedConventionSpecification : ConventionSpecification
    {
        protected override string FailureMessage => "All enumerable properties must be eager loaded";

        public override ConventionResult IsSatisfiedBy(Type type)
        {
            var enumerables = type
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Select(p => p.PropertyType)
                .Where(t => t != typeof(string) && (typeof(IEnumerable).IsAssignableFrom(t)))
                .ToArray();

            var failures = enumerables.Where(x => x.IsArray == false).ToArray();

            if (failures.Any())
            {
                return ConventionResult.NotSatisfied(type.FullName,
                    BuildFailureMessage(failures.Aggregate(string.Empty,
                        (s, t) => s + "\t" + type.FullName + Environment.NewLine)));
            }

            return ConventionResult.Satisfied(type.FullName);
        }
    }
}