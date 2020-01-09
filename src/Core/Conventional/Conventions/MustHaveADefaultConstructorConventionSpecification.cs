using System;
using System.Linq;
using System.Reflection;

namespace Conventional.Conventions
{
    public class MustHaveADefaultConstructorConventionSpecification : ConventionSpecification
    {
        protected override string FailureMessage => "Does not have a default constructor";

        public override ConventionResult IsSatisfiedBy(Type type)
        {
            var defaultConstructor = type
                .GetConstructors(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)
                .FirstOrDefault(c => c.GetParameters().Any() == false);

            if (defaultConstructor == null)
            {
                return ConventionResult.NotSatisfied(type.FullName, FailureMessage);
            }

            return ConventionResult.Satisfied(type.FullName);
        }
    }
}