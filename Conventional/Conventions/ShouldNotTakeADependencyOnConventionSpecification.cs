using System;
using System.Linq;

namespace Conventional.Conventions
{
    public class ShouldNotTakeADependencyOnConventionSpecification : ConventionSpecification
    {
        private readonly Type _dependencyType;

        public ShouldNotTakeADependencyOnConventionSpecification(Type dependencyType)
        {
            _dependencyType = dependencyType;
        }

        protected override string FailureMessage
        {
            get { return "Has a dependency on {0} and should not"; }
        }

        public override ConventionResult IsSatisfiedBy(Type type)
        {
            if (type.GetConstructors().SelectMany(x => x.GetParameters()).Any(x => x.ParameterType == _dependencyType))
            {
                return ConventionResult.NotSatisfied(type.FullName, FailureMessage.FormatWith(_dependencyType.FullName));
            }

            return ConventionResult.Satisfied(type.FullName);
        }
    }
}