using System;
using System.Linq;
using System.Reflection;

namespace Conventional.Conventions
{
    public class PropertiesMustNotHaveSettersConventionSpecification : PropertyConventionSpecification
    {
        protected override string FailureMessage => "All properties must not have setters";

        protected override PropertyInfo[] GetNonConformingProperties(Type type)
        {
            return type.GetProperties()
                .Where(x => x.GetSetMethod(true) != null)
                .ToArray();
        }
    }
}