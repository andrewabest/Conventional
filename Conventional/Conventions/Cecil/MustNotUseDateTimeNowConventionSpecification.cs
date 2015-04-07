using System;
using System.Linq;
using Conventional.Conventions;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Conventional.Cecil.Conventions
{
    public class MustNotUseDateTimeNowConventionSpecification : ConventionSpecification
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
                    .Where(
                        x =>
                            x.OpCode == OpCodes.Call && x.Operand is MethodReference &&
                            ((MethodReference) x.Operand).DeclaringType.FullName == "System.DateTime" &&
                            ((MethodReference) x.Operand).Name == "get_Now")
                    .ToArray();

            if (nowAssignments.Any())
            {
                return ConventionResult.NotSatisfied(type.FullName, FailureMessage.FormatWith(nowAssignments.Count()));
            }

            return ConventionResult.Satisfied(type.FullName);
        }
    }
}