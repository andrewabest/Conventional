using System;
using System.Linq;
using System.Reflection;

namespace Conventional.Conventions
{
    public class PropertiesMustHavePrivateSettersConventionSpecification : PropertyConventionSpecification
    {
        protected override string FailureMessage => "All properties must have private setters";

        protected override PropertyInfo[] GetNonConformingProperties(Type type)
        {
            return type.GetProperties()
                .Where(p => p.CanWrite)
                .Where(subject => subject.GetSetMethod(true).IsPrivate == false)
                .ToArray();
        }
    }
}