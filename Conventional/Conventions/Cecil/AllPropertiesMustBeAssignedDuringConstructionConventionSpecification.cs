using System;
using Mono.Cecil;
using Mono.Cecil.Rocks;
using System.Linq;

namespace Conventional.Conventions.Cecil
{
    public class AllPropertiesMustBeAssignedDuringConstructionConventionSpecification : ConventionSpecification
    {
        private readonly bool _ignoreTypesWithoutConstructors;

        public AllPropertiesMustBeAssignedDuringConstructionConventionSpecification(bool ignoreTypesWithoutConstructors = false)
        {
            _ignoreTypesWithoutConstructors = ignoreTypesWithoutConstructors;
        }

        protected override string FailureMessage => "All properties must be instantiated during construction";

        public override ConventionResult IsSatisfiedBy(Type type)
        {
            var typeDefinition = type.ToTypeDefinition();

            var subjects = typeDefinition.Properties;

            var subjectPropertySetters = subjects
                .Where(p => p.SetMethod != null)
                .Select(p => p.SetMethod.Name)
                .ToArray();

            var constructors =
                typeDefinition
                    .GetConstructors()
                    .Where(c => c.Parameters.Any() && c.IsStatic == false)
                    .ToArray();

            if (constructors.Count() > 1)
            {
                return ConventionResult.NotSatisfied(type.FullName, FailureMessage + " - could not determine method of construction due to more than one parameterized constructor existing");
            }

            var constructor = constructors.SingleOrDefault();
            if (constructor == null)
            {
                return _ignoreTypesWithoutConstructors ? ConventionResult.Satisfied(type.FullName) : ConventionResult.NotSatisfied(type.FullName, FailureMessage + " - could not enforce method of construction due to no parameterized constructor existing");
            }

            var setters =
                constructor
                    .Body
                    .Instructions
                    .Select(i => i.Operand)
                    .OfType<MethodDefinition>()
                    .Select(m => m.Name)
                    .ToArray();

            if (subjectPropertySetters.All(setters.Contains))
            {
                return ConventionResult.Satisfied(type.FullName);
            }

            var failureMessage =
                BuildFailureMessage(subjectPropertySetters.Where(x => setters.Contains(x) == false).Aggregate(string.Empty,
                    (s, name) => s + "\t- " + name + Environment.NewLine));

            return ConventionResult.NotSatisfied(type.FullName, failureMessage);
        }
    }
}