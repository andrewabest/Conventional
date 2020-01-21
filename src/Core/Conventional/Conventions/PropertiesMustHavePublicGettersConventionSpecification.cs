using System;
using System.Linq;
using System.Reflection;
using Conventional.Extensions;

namespace Conventional.Conventions
{
    public class PropertiesMustHavePublicGettersConventionSpecification : PropertyConventionSpecification
    {
        protected override string FailureMessage => "All properties must have public getters";

        protected override PropertyInfo[] GetNonConformingProperties(Type type)
        {
            return type.GetDeclaredProperties()
                .Where(subject => subject.CanRead == false || subject.GetGetMethod(true).IsPublic == false)
                .ToArray();
        }
    }
}