using System;
using System.Reflection;

namespace Conventional.Conventions
{
    public class ShouldHaveAttributeConventionSpecification : ConventionSpecification
    {
        private readonly Type _attributeType;
        private const string FailureDescription = "Attribute {0} not found";

        public ShouldHaveAttributeConventionSpecification(Type attributeType)
        {
            if (typeof (Attribute).IsAssignableFrom(attributeType) == false)
            {
                throw new ArgumentException("Type supplied must be an attribute", "attributeType");
            }

            _attributeType = attributeType;
        }

        protected override string FailureMessage
        {
            get { return "An attribute is required"; }
        }

        public override ConventionResult IsSatisfiedBy(Type type)
        {
            if (type.GetCustomAttribute(_attributeType) == null)
            {
                var failureMessage = BuildFailureMessage(FailureDescription.FormatWith(type.FullName));

                return ConventionResult.NotSatisfied(type.FullName, failureMessage);
            }

            return ConventionResult.Satisfied(type.FullName);
        }
    }
}