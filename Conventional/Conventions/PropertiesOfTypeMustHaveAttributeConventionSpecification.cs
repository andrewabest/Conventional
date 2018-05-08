using System;
using System.Linq;
using System.Reflection;

namespace Conventional.Conventions
{
    public class PropertiesOfTypeMustHaveAttributeConventionSpecification : ConventionSpecification
    {
        private readonly Type _propertyType;
        private readonly Type _attributeType;
        private readonly bool _inheritAttributes;

        public PropertiesOfTypeMustHaveAttributeConventionSpecification(Type propertyType, Type attributeType, bool inheritAttributes = false)
        {
            _propertyType = propertyType;
            _attributeType = attributeType;
            _inheritAttributes = inheritAttributes;
        }

        protected override string FailureMessage => "Properties of {0} must have {1} attribute";

        public override ConventionResult IsSatisfiedBy(Type type)
        {
            var properties = type
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(p => _propertyType.IsAssignableFrom(p.PropertyType))
                .ToArray();

            var failures = properties.Where(x => !x.GetCustomAttributes(_attributeType, _inheritAttributes).Any()).ToArray();

            if (failures.Any())
            {
                return ConventionResult.NotSatisfied(type.FullName,
                    BuildFailureMessage(failures.Aggregate(string.Empty,
                        (s, info) => s + "\t" + info.Name + Environment.NewLine)).FormatWith(_propertyType.Name, _attributeType.Name));
            }

            return ConventionResult.Satisfied(type.FullName);
        }
    }
}