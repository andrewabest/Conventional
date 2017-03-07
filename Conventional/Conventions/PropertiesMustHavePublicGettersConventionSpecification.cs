using System;
using System.Linq;
using System.Reflection;

namespace Conventional.Conventions
{
    public class PropertiesMustHavePublicGettersConventionSpecification : ConventionSpecification
    {
        private readonly BindingFlags _bindingFlags;

        protected override string FailureMessage
        {
            get { return "All properties must have public getters"; }
        }

        public PropertiesMustHavePublicGettersConventionSpecification()
        {
            _bindingFlags = BindingFlags.Instance | BindingFlags.Public;
        }

        public PropertiesMustHavePublicGettersConventionSpecification(BindingFlags bindingFlags)
        {
            _bindingFlags = bindingFlags;
        }

        public override ConventionResult IsSatisfiedBy(Type type)
        {
            var toInspect = type.GetProperties(_bindingFlags).Where(p => p.CanWrite);

            var failures = toInspect.Where(subject => subject.GetGetMethod() == null || subject.GetGetMethod().IsPublic == false).ToArray();

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