using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Conventional.Conventions.Cecil
{
    public abstract class MustNotUsePropertyGetterSpecification<TClass, TMember> : ConventionSpecification
    {
        private readonly (string DeclaringType, string MethodName)[] _getterDetails;

        protected MustNotUsePropertyGetterSpecification(Expression<Func<TClass, TMember>> expression,
            string failureMessage)
            :this(new[] {expression},failureMessage)
        {
        }

        protected MustNotUsePropertyGetterSpecification(Expression<Func<TClass, TMember>>[] expressions, string failureMessage)
        {
            FailureMessage = failureMessage;

            _getterDetails = expressions.Select(e =>
            {
                var memberExpression = (MemberExpression) e.Body;
                var propertyInfo = memberExpression.Member as PropertyInfo;
                if (propertyInfo == null)
                {
                    throw new ArgumentException("Expression must be a memberexpression selecting a single property.",
                        nameof(expressions));
                }
                return (typeof (TClass).FullName, propertyInfo.GetGetMethod().Name);
            }).ToArray();
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
                            .SelectMany(method => method.Body.Instructions)
                    );

            var assignments =
                typeInstructions
                    .Where(x =>
                        x.OpCode == OpCodes.Call && x.Operand is MethodReference)
                    .Join(_getterDetails, 
                        x => (((MethodReference)x.Operand).DeclaringType.FullName, ((MethodReference)x.Operand).Name),
                        g => (g.DeclaringType, g.MethodName),
                        (x,g)=>x)
                    .ToArray();

            if (assignments.Any())
            {
                return ConventionResult.NotSatisfied(type.FullName, FailureMessage.FormatWith(assignments.Count(), type.FullName));
            }

            return ConventionResult.Satisfied(type.FullName);
        }
    }
}