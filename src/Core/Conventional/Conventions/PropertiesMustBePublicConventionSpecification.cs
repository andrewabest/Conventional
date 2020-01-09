using System;
using System.Linq;
using System.Reflection;

namespace Conventional.Conventions
{
    public class PropertiesMustBePublicConventionSpecification : ConventionSpecification
    {
        protected override string FailureMessage => "All properties must be public";

        public override ConventionResult IsSatisfiedBy(Type type)
        {
            var failures = type.GetProperties(BindingFlags.Instance | BindingFlags.NonPublic);

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