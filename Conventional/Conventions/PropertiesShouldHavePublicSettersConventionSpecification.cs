using System;
using System.Linq;

namespace Conventional.Conventions
{
    public class PropertiesShouldHavePublicSettersConventionSpecification : ConventionSpecification
    {
        private const string FailureMessage = "All properties should have public setters";

        public override ConventionResult IsSatisfiedBy(Type type)
        {
            var toInspect = type.GetProperties().Where(p => p.CanWrite);

            var failures = toInspect.Where(subject => subject.SetMethod.IsPublic == false).ToArray();

            if (failures.Any())
            {
                var failureMessage =
                    FailureMessage + 
                    Environment.NewLine +
                    failures.Aggregate(string.Empty, (s, info) => s + "\t- " + info.Name + Environment.NewLine);

                return ConventionResult.NotSatisfied(type.FullName, failureMessage);
            }

            return ConventionResult.Satisfied(type.FullName);
        }
    }
}