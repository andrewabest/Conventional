using System;

namespace Conventional.Conventions
{
    public class NameMustStartWithConventionSpecification : ConventionSpecification
    {
        private readonly string _prefix;

        public NameMustStartWithConventionSpecification(string prefix)
        {
            _prefix = prefix;
        }

        protected override string FailureMessage => "Type name does not start with {0}";

        public override ConventionResult IsSatisfiedBy(Type type)
        {
            if (type.Name.StartsWith(_prefix) == false)
            {
                return ConventionResult.NotSatisfied(type.FullName, FailureMessage.FormatWith(_prefix));
            }

            return ConventionResult.Satisfied(type.FullName);
        }
    }
}