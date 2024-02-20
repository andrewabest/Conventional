using System;

namespace Conventional.Conventions
{
    public class NamespaceMustStartWithConventionSpecification : ConventionSpecification
    {
        private readonly string _namespacePrefix;

        public NamespaceMustStartWithConventionSpecification(string prefix) => _namespacePrefix = prefix;

        protected override string FailureMessage => "Must live in namespace beginning with {0} but actually lives in namespace {1}";

        public override ConventionResult IsSatisfiedBy(Type type)
        {
            if (type.Namespace != null && type.Namespace.StartsWith(_namespacePrefix))
            {
                return ConventionResult.Satisfied(type.FullName);
            }

            return ConventionResult.NotSatisfied(type.FullName, FailureMessage.FormatWith(_namespacePrefix, type.Namespace));
        }
    }
}