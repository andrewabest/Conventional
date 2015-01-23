using System;
using System.Collections.Generic;
using System.Linq;
using Conventional.Conventions;
using Mono.Cecil;

namespace Conventional.Cecil.Conventions
{
    public class ExceptionsThrownMustBeDerivedFromConventionSpecification : ConventionSpecification
    {
        private readonly Type _baseType;

        public ExceptionsThrownMustBeDerivedFromConventionSpecification(Type baseType)
        {
            _baseType = baseType;
        }

        protected override string FailureMessage
        {
            get { return "All thrown exceptions must derive from base type {0}"; }
        }

        public override ConventionResult IsSatisfiedBy(Type type)
        {
            var methodsWithBodies = type.ToTypeDefinition().Methods.Where(method => method.HasBody).ToList();

            var exceptions =
                methodsWithBodies.SelectMany(method => method.Body.Instructions
                    .Select(instr => new { Method = method, MemberReference = instr.Operand as MemberReference })
                    .Where(m => m.MemberReference != null && m.MemberReference.DeclaringType != null && m.MemberReference.DeclaringType.FullName.EndsWith("Exception"))
                    .Select(m => new KeyValuePair<TypeDefinition, string>(
                        m.MemberReference.DeclaringType.Resolve(),
                        m.Method.DeclaringType.FullName + "::" + m.Method.Name + "(" + m.MemberReference.DeclaringType.FullName + ")")));

            var failures = exceptions.Where(x => x.Key.IsAssignableTo(_baseType) == false).ToArray();

            if (failures.Any())
            {
                return ConventionResult.NotSatisfied(type.FullName, FailureMessage.FormatWith(_baseType.FullName) +
                                                                    Environment.NewLine +
                                                                    failures.Aggregate(string.Empty,
                                                                        (s, t) => s + t.Value + Environment.NewLine));
            }

            return ConventionResult.Satisfied(type.FullName);
        }
    }
}