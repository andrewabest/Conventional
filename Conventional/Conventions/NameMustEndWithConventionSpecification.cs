using System;

namespace Conventional.Conventions
{
    public class NameMustEndWithConventionSpecification : ConventionSpecification
    {
        private readonly string _suffix;

        public NameMustEndWithConventionSpecification(string suffix)
        {
            _suffix = suffix;
        }

        protected override string FailureMessage
        {
            get { return "Type name does not end with {0}"; }
        }

        public override ConventionResult IsSatisfiedBy(Type type)
        {
            if (type.Name.EndsWith(_suffix) == false)
            {
                return ConventionResult.NotSatisfied(type.FullName, FailureMessage.FormatWith(_suffix));
            }

            return ConventionResult.Satisfied(type.FullName);
        }
    }
}