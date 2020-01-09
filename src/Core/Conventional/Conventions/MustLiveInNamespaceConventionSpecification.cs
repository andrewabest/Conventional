using System;

namespace Conventional.Conventions
{
    public class MustLiveInNamespaceConventionSpecification : ConventionSpecification
    {
        private readonly string _nameSpace;

        public MustLiveInNamespaceConventionSpecification(string nameSpace)
        {
            _nameSpace = nameSpace;
        }

        protected override string FailureMessage => "Must live in namespace {0} but actually lives in namespace {1}";

        public override ConventionResult IsSatisfiedBy(Type type)
        {
            if (type.Namespace == null || type.Namespace.Equals(_nameSpace) == false)
            {
                return ConventionResult.NotSatisfied(type.FullName, FailureMessage.FormatWith(_nameSpace, type.Namespace));
            }

            return ConventionResult.Satisfied(type.FullName);
        }
    }
}