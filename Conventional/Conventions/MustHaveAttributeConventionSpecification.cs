using System;
using System.Linq;
using System.Reflection;

namespace Conventional.Conventions
{
    public class MustHaveAttributeConventionSpecification : ConventionSpecification
    {
        private readonly Type _attributeType;

        public MustHaveAttributeConventionSpecification(Type attributeType)
        {
            if (typeof (Attribute).IsAssignableFrom(attributeType) == false)
            {
                throw new ArgumentException("Type supplied must be an attribute", "attributeType");
            }

            _attributeType = attributeType;
        }

        protected override string FailureMessage
        {
            get { return "Attribute {0} not found"; }
        }

        public override ConventionResult IsSatisfiedBy(Type type)
        {
            if (type.GetCustomAttributes(_attributeType, false).Any() == false)
            {
                return ConventionResult.NotSatisfied(type.FullName, FailureMessage.FormatWith(_attributeType.FullName));
            }

            return ConventionResult.Satisfied(type.FullName);
        }
    }
}
