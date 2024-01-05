using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Conventional.Conventions.Cecil
{
    public class MustNotUseGuidNewGuidConventionSpecification :
        MustNotCallMethodConventionSpecification
    {
        private static MethodInfo GetMethod()
        {
            Expression<Func<Guid>> expr = () => Guid.NewGuid();
            return ((MethodCallExpression)expr.Body).Method;
        }

        public MustNotUseGuidNewGuidConventionSpecification()
            : base(new[] { GetMethod() })
        {
        }
    }
}