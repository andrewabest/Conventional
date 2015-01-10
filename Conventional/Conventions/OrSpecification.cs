using System;

namespace Conventional.Conventions
{
    public class OrSpecification : ConventionSpecification
    {
        private readonly IConventionSpecification _left;
        private readonly IConventionSpecification _right;

        public OrSpecification(IConventionSpecification x, IConventionSpecification y)
        {
            _left = x;
            _right = y;
        }

        public override ConventionResult IsSatisfiedBy(Type type)
        {
            var leftResult = _left.IsSatisfiedBy(type);
            var rightResult = _right.IsSatisfiedBy(type);

            return leftResult.Or(rightResult);
        }
    }
}