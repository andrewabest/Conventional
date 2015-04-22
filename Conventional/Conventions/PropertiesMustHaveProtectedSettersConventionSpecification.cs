using System;
using System.Linq;

namespace Conventional.Conventions
{
    public class PropertiesMustHaveProtectedSettersConventionSpecification : ConventionSpecification
    {
        protected override string FailureMessage
        {
            get { return "All properties should have protected setters"; }
        }

        public override ConventionResult IsSatisfiedBy(Type type)
        {
            var toInspect = type.GetProperties().Where(p => p.CanWrite);

            var failures = toInspect.Where(subject => subject.GetSetMethod(true) == null || subject.GetSetMethod(true).IsFamily == false).ToArray();

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