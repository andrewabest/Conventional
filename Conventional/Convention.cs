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

        public static NameShouldStartWithConventionSpecification NameShouldStartWith(string prefix)
        {
            return new NameShouldStartWithConventionSpecification(prefix);
        }
        
        public static NameShouldEndWithConventionSpecification NameShouldEndWith(string suffix)
        {
            return new NameShouldEndWithConventionSpecification(suffix);
        }

        public static ShouldLiveInNamespaceConventionSpecification ShouldLiveInNamespace(string nameSpace)
        {
            return new ShouldLiveInNamespaceConventionSpecification(nameSpace);
        }

        public static ShouldHaveADefaultConstructorConventionSpecification ShouldHaveADefaultConstructor
        {
            get { return new ShouldHaveADefaultConstructorConventionSpecification(); }
        }

        public static ShouldNotTakeADependencyOnConventionSpecification ShouldNotTakeADependencyOn(Type type)
        {
            return new ShouldNotTakeADependencyOnConventionSpecification(type);
        }
    }
}