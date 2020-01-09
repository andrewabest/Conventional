using System;
using System.Linq;

namespace Conventional.Conventions
{
    public class MustNotTakeADependencyOnConventionSpecification : ConventionSpecification
    {
        private readonly Type _dependencyType;
        private readonly string _reason;

        public MustNotTakeADependencyOnConventionSpecification(Type dependencyType, string reason)
        {
            _dependencyType = dependencyType;
            _reason = reason;
        }

        protected override string FailureMessage => "Has a dependency on {0} and must not:";

        public override ConventionResult IsSatisfiedBy(Type type)
        {
            if (type.GetConstructors().SelectMany(x => x.GetParameters()).Any(x => x.ParameterType == _dependencyType))
            {
                return ConventionResult.NotSatisfied(type.FullName, FailureMessage.FormatWith(_dependencyType.FullName) + Environment.NewLine + _reason);
            }

            return ConventionResult.Satisfied(type.FullName);
        }
    }
}