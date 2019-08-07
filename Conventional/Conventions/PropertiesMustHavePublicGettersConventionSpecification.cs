using System;
using System.Linq;
using System.Reflection;

namespace Conventional.Conventions
{
    public class PropertiesMustHavePublicGettersConventionSpecification : PropertyConventionSpecification
    {
        protected override string FailureMessage => "All properties must have public getters";

        protected override PropertyInfo[] GetNonConformingProperties(Type type)
        {
            return type.GetProperties()
                .Where(p => p.CanWrite)
                .Where(subject => subject.GetGetMethod(true).IsPublic == false)
                .ToArray();
        }
    }
}