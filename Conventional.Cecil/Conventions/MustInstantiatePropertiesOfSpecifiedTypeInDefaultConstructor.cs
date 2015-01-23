using System;
using System.Collections.Generic;
using System.Linq;
using Conventional.Conventions;
using Mono.Cecil;
using Mono.Cecil.Rocks;

namespace Conventional.Cecil.Conventions
{
    public class MustInstantiatePropertiesOfSpecifiedTypeInDefaultConstructorConventionSpecification : ConventionSpecification
    {
        private readonly Type _propertyType;

        public MustInstantiatePropertiesOfSpecifiedTypeInDefaultConstructorConventionSpecification(Type propertyType)
        {
            _propertyType = propertyType;
        }

        protected override string FailureMessage
        {
            get { return "Properties of type {0} must be instantiated in the default constructor"; }
        }

        public override ConventionResult IsSatisfiedBy(Type type)
        {
            var typeDefinition = type.ToTypeDefinition();

            var subjects = typeDefinition.GetPropertiesOfType(_propertyType);

            var subjectPropertySetters = subjects
                .Where(p => p.SetMethod != null)
                .Select(p => p.SetMethod.Name)
                .ToArray();

            var defaultConstructor =
                typeDefinition
                    .GetConstructors()
                    .SingleOrDefault(c => c.Parameters.Any() == false && c.IsStatic == false);

            if (defaultConstructor == null)
            {
                return ConventionResult.Satisfied(type.FullName);
            };

            var setters =
                defaultConstructor
                    .Body
                    .Instructions
                    .Select(i => i.Operand)
                    .OfType<MethodDefinition>()
                    .Select(m => m.Name);

            if (setters.Count(subjectPropertySetters.Contains) == subjectPropertySetters.Count())
            {
                return ConventionResult.Satisfied(type.FullName);
            }

            return ConventionResult.NotSatisfied(type.FullName, FailureMessage.FormatWith(_propertyType));
        }
    }
}