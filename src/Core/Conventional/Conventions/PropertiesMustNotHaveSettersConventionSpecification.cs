using System;
using System.Linq;
using System.Reflection;
using Conventional.Extensions;

namespace Conventional.Conventions
{
    public class PropertiesMustNotHaveSettersConventionSpecification : PropertyConventionSpecification
    {
        protected override string FailureMessage => "All properties must not have setters";

        protected override PropertyInfo[] GetNonConformingProperties(Type type)
        {
            return type.GetDeclaredProperties()
                .Where(x => x.GetSetMethod(true) != null)
                .ToArray();
        }
    }
}