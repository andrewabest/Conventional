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
        private readonly (string DeclaringType, string MethodName)[] _getterDetails;
        private readonly MethodInfo[] _methodInfos;

        protected MustNotUseMethodSpecification(MethodInfo methodInfo,
            string failureMessage)
            :this(new[] {methodInfo},failureMessage)
        {
        }

        protected MustNotUseMethodSpecification(MethodInfo[] methodInfos, string failureMessage)
        {
            FailureMessage = failureMessage;
            _methodInfos = methodInfos;
        }

        protected override string FailureMessage { get; }

        public override ConventionResult IsSatisfiedBy(Type type)
        {
            var typeInstructions =
                type.ToTypeDefinition()
                    .Methods
                    .Where(method => method.HasBody)
                    .SelectMany(method => method.Body.Instructions)
                    .Union(
                        type.ToTypeDefinition()
                            .Methods
                            .Where(x => x.HasAttribute<AsyncStateMachineAttribute>())
                            .SelectMany(x => x.GetAsyncStateMachineType().Methods.Where(method => method.HasBody))
                            .SelectMany(method => method.Body.Instructions))
                    .Union(
                        type.ToTypeDefinition()
                            .Methods
                            .Where(x => x.HasAttribute<IteratorStateMachineAttribute>())
                            .SelectMany(x => x.GetIteratorStateMachineType().Methods.Where(method => method.HasBody))
                            .SelectMany(method => method.Body.Instructions));

            var assignments =
                typeInstructions
                    .Where(x =>
                        x.OpCode == OpCodes.Call && x.Operand is MethodReference)
                    .Join(_methodInfos, 
                        x => (((MethodReference)x.Operand).DeclaringType.FullName, ((MethodReference)x.Operand).Name),
                        g => (g.DeclaringType?.FullName, g.Name),
                        (x,g) => x)
                    .ToArray();

            if (assignments.Any())
            {
                return ConventionResult.NotSatisfied(type.FullName, FailureMessage.FormatWith(assignments.Count(), type.FullName));
            }

            return ConventionResult.Satisfied(type.FullName);
        }
    }
}