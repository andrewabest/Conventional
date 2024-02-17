using System;

namespace Conventional.Conventions
{
    public class NamespaceMustEndWithConventionSpecification : ConventionSpecification
    {
        private readonly string _namespaceSuffix;

        public NamespaceMustEndWithConventionSpecification(string suffix) => _namespaceSuffix = suffix;

        protected override string FailureMessage => "Must live in namespace ending with {0} but actually lives in namespace {1}";

        public override ConventionResult IsSatisfiedBy(Type type)
        {
            if (type.Namespace != null && type.Namespace.EndsWith(_namespaceSuffix))
            {
                return ConventionResult.Satisfied(type.FullName);
            }

            return ConventionResult.NotSatisfied(type.FullName, FailureMessage.FormatWith(_namespaceSuffix, type.Namespace));
        }
    }
}