using System;
using System.Linq;
using System.Reflection;

namespace Conventional.Conventions
{
    public class PropertiesMustHaveProtectedSettersConventionSpecification : PropertyConventionSpecification
    {
        protected override string FailureMessage => "All properties must have protected setters";

        protected override PropertyInfo[] GetNonConformingProperties(Type type)
        {
            return type.GetProperties()
                .Where(subject => subject.CanWrite == false || subject.GetSetMethod(true).IsFamily == false)
                .ToArray();
        }
    }
}