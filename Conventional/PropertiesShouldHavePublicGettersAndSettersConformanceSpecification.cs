using System;
using System.Linq;

namespace Conventional
{
    public class PropertiesShouldHavePublicGettersAndSettersConformanceSpecification : IConformanceSpecification
    {
        private const string FailureMessage = "All properties should have public getters and setters";

        public ConformanceResult IsSatisfiedBy(Type type)
        {
            var toInspect = type.GetProperties().Where(p => p.CanWrite);

            var failures = toInspect.Where(subject => subject.GetMethod.IsPublic == false || subject.SetMethod.IsPublic == false).ToArray();

            if (failures.Any())
            {
                var failureMessage =
                    type.FullName +
                    Environment.NewLine +
                    StringConstants.Underline +
                    Environment.NewLine +
                    FailureMessage + 
                    Environment.NewLine +
                    failures.Aggregate(string.Empty, (s, info) => s + "\t- " + info.Name + Environment.NewLine);

                return ConformanceResult.NotSatisfied(type.FullName, failureMessage);
            }

            return ConformanceResult.Satisfied(type.FullName);
        }
    }
}