using System;
using System.Linq;
using System.Reflection;

namespace Conventional.Conventions
{
    public class PropertiesMustHavePublicSettersConventionSpecification : ConventionSpecification
    {
        private readonly BindingFlags _bindingFlags;

        protected override string FailureMessage
        {
            get { return "All properties must have public setters"; }
        }

        public PropertiesMustHavePublicSettersConventionSpecification()
        {
            _bindingFlags = BindingFlags.Instance | BindingFlags.Public;
        }

        public PropertiesMustHavePublicSettersConventionSpecification(BindingFlags bindingFlags)
        {
            _bindingFlags = bindingFlags;
        }

        public override ConventionResult IsSatisfiedBy(Type type)
        {
            var toInspect = type.GetProperties(_bindingFlags).Where(p => p.CanWrite);

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