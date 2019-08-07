using System;
using System.Collections;
using System.Linq;
using System.Reflection;

namespace Conventional.Conventions
{
    public class CollectionPropertiesMustNotHaveSetters : PropertyConventionSpecification
    {
        protected override string FailureMessage => "Collection properties must be immutable";

        protected override PropertyInfo[] GetNonConformingProperties(Type type)
        {
            return type
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(p => p.PropertyType != typeof (string) && (typeof (IEnumerable).IsAssignableFrom(p.PropertyType)))
                .Where(p => p.GetSetMethod(true) != null)
                .ToArray();
        }
    }
}