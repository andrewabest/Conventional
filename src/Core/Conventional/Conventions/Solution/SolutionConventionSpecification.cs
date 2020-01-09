using System;

namespace Conventional.Conventions.Solution
{
    public abstract class SolutionConventionSpecification : ISolutionConventionSpecification
    {
        protected const string SolutionConventionResultIdentifier = "Solution convention";

        protected abstract string FailureMessage { get; }

        protected string BuildFailureMessage(string details)
        {
            return FailureMessage +
                   Environment.NewLine +
                   details;
        }

        public abstract ConventionResult IsSatisfiedBy(string solutionRoot);
    }
}