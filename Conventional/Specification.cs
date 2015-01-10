using System;
using Conventional.Conventions;

namespace Conventional
{
    public static class Convention
    {
        public static PropertiesShouldHavePublicGettersConventionSpecification PropertiesShouldHavePublicGetters
        {
            get {  return new PropertiesShouldHavePublicGettersConventionSpecification(); }
        }
        
        public static PropertiesShouldHavePublicSettersConventionSpecification PropertiesShouldHavePublicSetters
        {
            get {  return new PropertiesShouldHavePublicSettersConventionSpecification(); }
        }

        public static ShouldHaveAttributeConventionSpecification ShouldHaveAttribute(Type attributeType)
        {
            return new ShouldHaveAttributeConventionSpecification(attributeType);
        }
    }
}