using System;
using System.Threading.Tasks;

namespace Conventional.Conventions
{
    public abstract class AsyncConventionSpecification : IAsyncConventionSpecification
    {
        protected abstract string FailureMessage { get; }

        protected string BuildFailureMessage(string details)
        {
            return FailureMessage +
                   Environment.NewLine +
                   details;
        }

        public abstract Task<ConventionResult> IsSatisfiedBy(Type type);
    }
}