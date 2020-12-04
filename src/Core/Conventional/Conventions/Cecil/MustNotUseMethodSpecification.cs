using System;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Conventional.Conventions.Cecil
{
    public abstract class MustNotUseMethodSpecification : ConventionSpecification
    {
        private readonly MethodInfo[] _methodInfos;
        private readonly bool _includeVirtualMethodCalls;

        protected MustNotUseMethodSpecification(MethodInfo methodInfo,
            string failureMessage,
            bool includeVirtualMethodCalls = false)
            :this(new[] {methodInfo},failureMessage, includeVirtualMethodCalls)
        {
        }

        protected MustNotUseMethodSpecification(MethodInfo[] methodInfos, string failureMessage, bool includeVirtualMethodCalls = false)
        {
            FailureMessage = failureMessage;
            _methodInfos = methodInfos;
            _includeVirtualMethodCalls = includeVirtualMethodCalls;
        }

        protected override string FailureMessage { get; }

        public override ConventionResult IsSatisfiedBy(Type type)
        {
            var typeDefinition = type.ToTypeDefinition();

            var typeInstructions =
                typeDefinition
                    .Methods
                    .Where(method => method.HasBody)
                    .SelectMany(method => method.Body.Instructions)
                    .Union(
                        typeDefinition
                            .Methods
                            .Where(x => x.HasAttribute<AsyncStateMachineAttribute>())
                            .SelectMany(x => x.GetAsyncStateMachineType().Methods.Where(method => method.HasBody))
                            .SelectMany(method => method.Body.Instructions))
                    .Union(
                        typeDefinition
                            .Methods
                            .Where(x => x.HasAttribute<IteratorStateMachineAttribute>())
                            .SelectMany(x => x.GetIteratorStateMachineType().Methods.Where(method => method.HasBody))
                            .SelectMany(method => method.Body.Instructions));

            var operandPredicate = _includeVirtualMethodCalls
                ? (Func<Instruction, bool>)(x => (x.OpCode == OpCodes.Call || x.OpCode == OpCodes.Callvirt) && x.Operand is MethodReference)
                : x => x.OpCode == OpCodes.Call && x.Operand is MethodReference;

                    var assignments =
                typeInstructions
                    .Where(operandPredicate)
                    .Join(_methodInfos,
                        x => (((MethodReference)x.Operand).DeclaringType.FullName, ((MethodReference)x.Operand).Name),
                        g => (g.DeclaringType?.FullName, g.Name),
                        (x,g) => x)
                    .Distinct().ToArray();

            if (assignments.Any())
            {
                return ConventionResult.NotSatisfied(type.FullName, FailureMessage.FormatWith(assignments.Count(), type.FullName));
            }

            return ConventionResult.Satisfied(type.FullName);
        }
    }
}