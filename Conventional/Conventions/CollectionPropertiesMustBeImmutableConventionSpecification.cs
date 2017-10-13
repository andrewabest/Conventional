using System;
using System.Collections;
using System.Linq;
using System.Reflection;

namespace Conventional.Conventions
{
    public class CollectionPropertiesMustBeImmutableConventionSpecification : PropertiesMustBeImmutableConventionSpecification
    {
        protected override string FailureMessage => "Collection properties must be immutable";

        protected override PropertyInfo[] GetProperties(Type type)
        {
            return type
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(p => p.PropertyType != typeof (string) && (typeof (IEnumerable).IsAssignableFrom(p.PropertyType)))
                .ToArray();
        }
    }
}