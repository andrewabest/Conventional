using System;
using System.Linq;
using System.Reflection;
using Conventional.Extensions;

namespace Conventional.Conventions
{
    public class PropertiesMustHaveProtectedSettersConventionSpecification : PropertyConventionSpecification
    {
        protected override string FailureMessage => "All properties must have protected setters";

        protected override PropertyInfo[] GetNonConformingProperties(Type type)
        {
            return type.GetDeclaredProperties()
                .Where(subject => subject.CanWrite == false || subject.GetSetMethod(true).IsFamily == false)
                .ToArray();
        }
    }
}