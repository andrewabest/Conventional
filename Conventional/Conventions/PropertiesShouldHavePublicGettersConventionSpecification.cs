using System;
using System.Linq;

namespace Conventional.Conventions
{
    public class PropertiesShouldHavePublicGettersConventionSpecification : ConventionSpecification
    {
        private const string FailureMessage = "All properties should have public getters";

        public override ConventionResult IsSatisfiedBy(Type type)
        {
            var toInspect = type.GetProperties().Where(p => p.CanWrite);

            var failures = toInspect.Where(subject => subject.GetMethod.IsPublic == false).ToArray();

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