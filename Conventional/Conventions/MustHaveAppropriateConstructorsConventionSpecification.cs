using System;
using System.Linq;
using System.Reflection;

namespace Conventional.Conventions
{
    public class MustHaveAppropriateConstructorsConventionSpecification : ConventionSpecification
    {
        protected override string FailureMessage => "Must have either a public default constructor, or one protected default constructor with public non-default constructors.";

        public override ConventionResult IsSatisfiedBy(Type type)
        {
            var constructors = type.GetConstructors(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);

            // Note: only one constructor? it should be public.
            if (constructors.Count() == 1)
            {
                if (constructors.Single().IsPublic)
                {
                    return ConventionResult.Satisfied(type.FullName);
                }

                return ConventionResult.NotSatisfied(type.FullName, FailureMessage);
            }

            // Note: more than one constructor? the default constructor should be protected and the others public.
            var defaultConstructorIsProtected = constructors.Where(c => c.GetParameters().Any() == false).All(c => c.IsFamily);
            var allNonDefaultConstructorsArePublic = constructors.Where(c => c.GetParameters().Any()).All(c => c.IsPublic);

            if (defaultConstructorIsProtected && allNonDefaultConstructorsArePublic)
            {
                return ConventionResult.Satisfied(type.FullName);
            }

            return ConventionResult.NotSatisfied(type.FullName, FailureMessage);
        }
    }
}