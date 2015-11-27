using System;
using System.Linq;

namespace Conventional.Conventions
{
    public class MustNotHaveAPropertyOfTypeConventionSpecification : ConventionSpecification
    {
        private readonly Type _propertyType;
        private readonly string _reason;

        public MustNotHaveAPropertyOfTypeConventionSpecification(Type propertyType, string reason)
        {
            _propertyType = propertyType;
            _reason = reason;
        }

        protected override string FailureMessage
        {
            get { return "Has a property of type {0} and must not:"; }
        }

        public override ConventionResult IsSatisfiedBy(Type type)
        {
            if (_propertyType.IsGenericTypeDefinition)
            {
                return type.GetProperties()
                    .Any(p => p.PropertyType.IsGenericType &&
                              p.PropertyType.GetGenericTypeDefinition() == _propertyType)
                    ? NotSatisfied(type)
                    : ConventionResult.Satisfied(type.FullName);
            }

            return type.GetProperties().Any(p => p.PropertyType == _propertyType)
                ? NotSatisfied(type)
                : ConventionResult.Satisfied(type.FullName);
        }

        private ConventionResult NotSatisfied(Type type)
        {
            var format = FailureMessage + Environment.NewLine + _reason;
            var message = string.Format(format, _propertyType.FullName);
            return ConventionResult.NotSatisfied(type.FullName, message);
        }
    }
}
