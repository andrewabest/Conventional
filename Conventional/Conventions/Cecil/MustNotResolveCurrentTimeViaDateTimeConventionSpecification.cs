using System;
using System.Linq;
using Conventional.Conventions;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Conventional.Cecil.Conventions
{
    public class MustNotResolveCurrentTimeViaDateTimeConventionSpecification : ConventionSpecification
    {
        protected override string FailureMessage
        {
            get { return "Must not use DateTime directly. It is used {0} times in {1}. "; }
        }

        public override ConventionResult IsSatisfiedBy(Type type)
        {
            var forbiddenUsages =
                type.ToTypeDefinition()
                    .Methods
                    .Where(method => method.HasBody)
                    .SelectMany(method => method.Body.Instructions)
                    .Where(DateTimeForbiddenPropertiesMatcher)
                    .ToArray();

            if (forbiddenUsages.Any())
            {
                var failureMessage = FailureMessage.FormatWith(forbiddenUsages.Count(), type.FullName);
                return ConventionResult.NotSatisfied(type.FullName, failureMessage);
            }

            return ConventionResult.Satisfied(type.FullName);
        }

        private static bool DateTimeForbiddenPropertiesMatcher(Instruction instruction)
        {
            var forbidenProperties = new[] { "get_Now", "get_UtcNow", "get_Today" };
            if (instruction.OpCode == OpCodes.Call && instruction.Operand is MethodReference)
            {
                var method = (MethodReference)instruction.Operand;
                return method.DeclaringType.FullName == "System.DateTime" && forbidenProperties.Contains(method.Name);
            }

            return false;
        }
    }
}