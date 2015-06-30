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
            var name = type.Name;

            if (type.IsGenericType)
                name = name.Substring(0, name.IndexOf("`", StringComparison.Ordinal));

            return name.EndsWith(_suffix) == false
                ? ConventionResult.NotSatisfied(type.FullName, FailureMessage.FormatWith(_suffix))
                : ConventionResult.Satisfied(type.FullName);
        }
    }
}
