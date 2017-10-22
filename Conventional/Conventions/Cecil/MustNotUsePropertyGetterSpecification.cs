using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Conventional.Conventions.Cecil
{
    public abstract class MustNotUsePropertyGetterSpecification<TClass, TMember> : ConventionSpecification
    {
        private class GetterDetails
        {
            public GetterDetails(string declaringType, string methodName)
            {
                DeclaringType = declaringType;
                MethodName = methodName;
            }

            public string DeclaringType { get; private set; }
            public string MethodName { get; private set; }
        }

        private readonly string _failureMessage;

        private readonly GetterDetails[] _getterDetails;

        protected MustNotUsePropertyGetterSpecification(Expression<Func<TClass, TMember>> expression,
            string failureMessage)
            :this(new[] {expression},failureMessage)
        {
            
        }

        protected MustNotUsePropertyGetterSpecification(Expression<Func<TClass, TMember>>[] expressions, string failureMessage)
        {
            _failureMessage = failureMessage;

            _getterDetails = expressions.Select(e =>
            {
                var memberExpression = (MemberExpression) e.Body;
                var propertyInfo = memberExpression.Member as PropertyInfo;
                if (propertyInfo == null)
                {
                    throw new ArgumentException("Expression must be a memberexpression selecting a single property.",
                        nameof(expressions));
                }
                return new GetterDetails(typeof (TClass).FullName, propertyInfo.GetGetMethod().Name);
            }).ToArray();
        }

        protected override string FailureMessage => _failureMessage;

        public override ConventionResult IsSatisfiedBy(Type type)
        {
            var assignments =
                type.ToTypeDefinition()
                    .Methods
                    .Where(method => method.HasBody)
                    .SelectMany(method => method.Body.Instructions)
                    .Where(x =>
                        x.OpCode == OpCodes.Call && x.Operand is MethodReference)
                    .Join(_getterDetails, 
                        x=>new
                        {
                            DeclaringType = ((MethodReference)x.Operand).DeclaringType.FullName,
                            MethodName = ((MethodReference)x.Operand).Name
                        },
                        g=>new
                        {
                            g.DeclaringType,
                            g.MethodName
                        },
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