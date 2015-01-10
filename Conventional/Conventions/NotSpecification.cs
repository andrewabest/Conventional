using System;

namespace Conventional.Conventions
{
    public class NotSpecification : ConventionSpecification
    {
        private readonly IConventionSpecification _wrapped;

        public NotSpecification(IConventionSpecification x)
        {
            _wrapped = x;
        }

        public override ConventionResult IsSatisfiedBy(Type type)
        {
            var result = _wrapped.IsSatisfiedBy(type);
            result.Not();
            return result;
        }
    }
}