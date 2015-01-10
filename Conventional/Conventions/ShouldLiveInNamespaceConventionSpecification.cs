using System;

namespace Conventional.Conventions
{
    public class ShouldLiveInNamespaceConventionSpecification : ConventionSpecification
    {
        private readonly string _nameSpace;

        public ShouldLiveInNamespaceConventionSpecification(string nameSpace)
        {
            _nameSpace = nameSpace;
        }

        protected override string FailureMessage
        {
            get { return "Should live in namespace {0} but actually lives in namespace {1}"; }
        }

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