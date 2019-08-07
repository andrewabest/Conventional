using System;
using System.Linq;
using System.Reflection;

namespace Conventional.Conventions
{
    public abstract class PropertyConventionSpecification : ConventionSpecification
    {
        protected abstract PropertyInfo[] GetNonConformingProperties(Type type);

        public override ConventionResult IsSatisfiedBy(Type type)
        {
            var failures = GetNonConformingProperties(type);

            if (failures.Any())
            {
                var failureMessage =
                    BuildFailureMessage(failures.Aggregate(string.Empty,
                        (s, info) => s + "\t- " + info.Name + Environment.NewLine));

                return ConventionResult.NotSatisfied(type.FullName, failureMessage);
            }

            return ConventionResult.Satisfied(type.FullName);
        }
    }
}