using System;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Rocks;

namespace Conventional.Conventions.Cecil
{
    public class MustInstantiatePropertiesOfSpecifiedTypeInDefaultConstructorConventionSpecification : ConventionSpecification
    {
        private readonly Type[] _propertyTypes;

        public MustInstantiatePropertiesOfSpecifiedTypeInDefaultConstructorConventionSpecification(Type propertyType)
        {
            _propertyTypes = new [] { propertyType };
        }

        public MustInstantiatePropertiesOfSpecifiedTypeInDefaultConstructorConventionSpecification(Type[] propertyTypes)
        {
            _propertyTypes = propertyTypes;
        }

        protected override string FailureMessage => "Properties of type {0} must be instantiated in the default constructor";

        public override ConventionResult IsSatisfiedBy(Type type)
        {
            var typeDefinition = DecompilationCache.GetTypeDefinitionFor(type);

            var subjects = _propertyTypes.SelectMany(x => typeDefinition.GetPropertiesOfType(x));

            var subjectProperties = subjects
                .Where(p => p.SetMethod != null)
                .ToArray();

            var defaultConstructor =
                typeDefinition
                    .GetConstructors()
                    .SingleOrDefault(c => c.Parameters.Any() == false && c.IsStatic == false);

            if (defaultConstructor == null)
            {
                return ConventionResult.Satisfied(type.FullName);
            };

            var executedSetters =
                defaultConstructor
                    .Body
                    .Instructions
                    .Select(i => i.Operand)
                    .OfType<MethodDefinition>()
                    .Select(m => m.Name);

            var uninitialisedProperties = subjectProperties.Where(p => !executedSetters.Contains(p.SetMethod.Name));

            if (!uninitialisedProperties.Any())
            {
                return ConventionResult.Satisfied(type.FullName);
            }

            var failureDetails = string.Join(", ", uninitialisedProperties
                .GroupBy(p => p.PropertyType.Name)
                .Select(p => $"{p.Key} ({string.Join(", ", p.Select(x => x.Name))})"));

            return ConventionResult.NotSatisfied(type.FullName, FailureMessage.FormatWith(failureDetails));
        }
    }
}