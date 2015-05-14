using System;
using System.Linq;
using Conventional.Cecil;
using Mono.Cecil;
using Mono.Cecil.Rocks;

namespace Conventional.Conventions.Cecil
{
    public class AllPropertiesMustBeAssignedDuringConstructionConventionSpecification : ConventionSpecification
    {
        protected override string FailureMessage
        {
            get { return "All properties must be instantiated during construction"; }
        }

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
                return ConventionResult.NotSatisfied(type.FullName, FailureMessage + " - could not enforce method of construction due to no parameterized constructor existing");
            };

            var setters =
                constructor
                    .Body
                    .Instructions
                    .Select(i => i.Operand)
                    .OfType<MethodDefinition>()
                    .Select(m => m.Name)
                    .ToArray();

            if (setters.Count(subjectPropertySetters.Contains) == subjectPropertySetters.Count())
            {
                return ConventionResult.Satisfied(type.FullName);
            }

            var failureMessage =
                    BuildFailureMessage(setters.Aggregate(string.Empty,
                        (s, name) => s + "\t- " + name + Environment.NewLine));

            return ConventionResult.NotSatisfied(type.FullName, BuildFailureMessage(failureMessage));
        }
    }
}