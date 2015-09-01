using System;
using System.Linq;
using Conventional.Conventions;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Conventional.Cecil.Conventions
{
    public class MustNotResolveCurrentTimeViaDateTimeDirectlyConventionSpecification : ConventionSpecification
    {
        protected override string FailureMessage
        {
            get { return "Must not use DateTime.Now. It is called {0} times in this type."; }
        }

        public override ConventionResult IsSatisfiedBy(Type type)
        {
            var nowAssignments =
                type.ToTypeDefinition()
                    .Methods
                    .Where(method => method.HasBody)
                    .SelectMany(method => method.Body.Instructions)
                    .Where(DateTimeForbiddenPropertiesMatcher)
                    .ToArray();

            if (nowAssignments.Any())
            {
                return ConventionResult.NotSatisfied(type.FullName, FailureMessage.FormatWith(nowAssignments.Count()));
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