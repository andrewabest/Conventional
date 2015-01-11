using System;
using Conventional.Conventions;

namespace Conventional
{
    public static class Convention
    {
        public static PropertiesMustHavePublicGettersConventionSpecification PropertiesMustHavePublicGetters
        {
            get {  return new PropertiesMustHavePublicGettersConventionSpecification(); }
        }
        
        public static PropertiesMustHavePublicSettersConventionSpecification PropertiesMustHavePublicSetters
        {
            get {  return new PropertiesMustHavePublicSettersConventionSpecification(); }
        }

        public static MustHaveAttributeConventionSpecification MustHaveAttribute(Type attributeType)
        {
            return new MustHaveAttributeConventionSpecification(attributeType);
        }

        public static NameMustStartWithConventionSpecification NameMustStartWith(string prefix)
        {
            return new NameMustStartWithConventionSpecification(prefix);
        }
        
        public static NameMustEndWithConventionSpecification NameMustEndWith(string suffix)
        {
            return new NameMustEndWithConventionSpecification(suffix);
        }

        public static MustLiveInNamespaceConventionSpecification MustLiveInNamespace(string nameSpace)
        {
            return new MustLiveInNamespaceConventionSpecification(nameSpace);
        }

        public static MustHaveADefaultConstructorConventionSpecification MustHaveADefaultConstructor
        {
            get { return new MustHaveADefaultConstructorConventionSpecification(); }
        }

        public static MustNotTakeADependencyOnConventionSpecification MustNotTakeADependencyOn(Type type)
        {
            return new MustNotTakeADependencyOnConventionSpecification(type);
        }
    }
}