using System;

namespace Conventional.Conventions
{
    public class NotConventionSpecification : ConventionSpecification
    {
        private readonly IConventionSpecification _wrapped;

        public NotConventionSpecification(IConventionSpecification x)
        {
            _wrapped = x;
        }

        protected override string FailureMessage
        {
            get { throw new NotImplementedException(); }
        }

        public override ConventionResult IsSatisfiedBy(Type type)
        {
            var result = _wrapped.IsSatisfiedBy(type);
            result.Not();
            return result;
        }
    }
}