using System;
using System.Linq;

namespace Conventional.Conventions
{
    public class PropertiesMustHavePublicSettersConventionSpecification : ConventionSpecification
    {
        protected override string FailureMessage => "All properties must have public setters";

        public override ConventionResult IsSatisfiedBy(Type type)
        {
            var toInspect = type.GetProperties().Where(p => p.CanWrite);

            var failures = toInspect.Where(subject => subject.GetSetMethod() == null || subject.GetSetMethod().IsPublic == false).ToArray();

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