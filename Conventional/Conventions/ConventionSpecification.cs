using System;

namespace Conventional.Conventions
{
    public abstract class ConventionSpecification : IConventionSpecification
    {
        protected abstract string FailureMessage { get; }

        protected string BuildFailureMessage(string details)
        {
            return FailureMessage +
                   Environment.NewLine +
                   details;
        }

        public abstract ConventionResult IsSatisfiedBy(Type type);

        public IConventionSpecification And(IConventionSpecification conventionSpecification)
        {
            return new AndSpecification(this, conventionSpecification);
        }

        public IConventionSpecification Or(IConventionSpecification conventionSpecification)
        {
            return new OrSpecification(this, conventionSpecification);
        }

        public IConventionSpecification Not()
        {
            return new NotSpecification(this);
        }
    }
}