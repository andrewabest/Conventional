using System;
using System.Linq;
using System.Reflection;

namespace Conventional.Conventions
{
    public abstract class PropertiesMustBeImmutableConventionSpecification : ConventionSpecification
    {
        protected abstract PropertyInfo[] GetProperties(Type type);

        public override ConventionResult IsSatisfiedBy(Type type)
        {
            var enumerables = GetProperties(type);

            var failures = enumerables.Where(x => x.GetSetMethod() != null).ToArray();

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