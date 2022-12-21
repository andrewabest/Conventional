using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Conventional.Conventions.Cecil
{
    public abstract class MustNotUsePropertyGetterSpecification<TClass, TMember> : MustNotCallMethodConventionSpecification
    {
        public MustNotUsePropertyGetterSpecification(Expression<Func<TClass, TMember>> expression)
            : this(new[] {expression})
        {
        }

        public MustNotUsePropertyGetterSpecification(Expression<Func<TClass, TMember>>[] expressions):base(
            expressions.Select(GetMethodInfoFromExpression).ToArray())
        {
        }

        private static MethodInfo GetMethodInfoFromExpression(Expression<Func<TClass, TMember>> expr)
        {
            var memberExpression = (MemberExpression) expr.Body;
            var propertyInfo = memberExpression.Member as PropertyInfo;
            if (propertyInfo == null)
            {
                throw new ArgumentException("Expression must be a memberexpression selecting a single property.",
                    nameof(expr));
            }
            return propertyInfo.GetGetMethod();
        }
    }
}