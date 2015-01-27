using System;
using System.Linq;
using System.Reflection;

namespace Conventional.Conventions
{
    public class AllPropertiesMustBeImmutableConventionSpecification : PropertiesMustBeImmutableConventionSpecification
    {
        protected override string FailureMessage
        {
            get { return "All properties must be immutable"; }
        }

        protected override PropertyInfo[] GetProperties(Type type)
        {
            return type
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .ToArray();
        }
    }
}