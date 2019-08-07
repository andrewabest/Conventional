using System;
using System.Linq;
using System.Reflection;

namespace Conventional.Conventions
{
    public class PropertiesMustNotHaveSetters : PropertyConventionSpecification
    {
        protected override string FailureMessage => "All properties must be immutable";

        protected override PropertyInfo[] GetNonConformingProperties(Type type)
        {
            return type.GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(x => x.GetSetMethod(true) != null)
                .ToArray();
        }
    }
}