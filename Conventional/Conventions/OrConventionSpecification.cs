using System;

namespace Conventional.Conventions
{
    public class OrConventionSpecification : ConventionSpecification
    {
        private readonly IConventionSpecification _left;
        private readonly IConventionSpecification _right;

        public OrConventionSpecification(IConventionSpecification x, IConventionSpecification y)
        {
            _left = x;
            _right = y;
        }

        protected override string FailureMessage
        {
            get { throw new NotImplementedException(); }
        }

        public override ConventionResult IsSatisfiedBy(Type type)
        {
            var leftResult = _left.IsSatisfiedBy(type);
            var rightResult = _right.IsSatisfiedBy(type);

            return leftResult.Or(rightResult);
        }
    }
}