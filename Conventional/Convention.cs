using System;
using Conventional.Conventions;

namespace Conventional
{
    public static partial class Convention
    {
        public static PropertiesMustHavePublicGettersConventionSpecification PropertiesMustHavePublicGetters
        {
            get {  return new PropertiesMustHavePublicGettersConventionSpecification(); }
        }
        
        public static PropertiesMustHavePublicSettersConventionSpecification PropertiesMustHavePublicSetters
        {
            get {  return new PropertiesMustHavePublicSettersConventionSpecification(); }
        }
        
        public static PropertiesMustHaveProtectedSettersConventionSpecification PropertiesMustHaveProtectedSetters
        {
            get {  return new PropertiesMustHaveProtectedSettersConventionSpecification(); }
        }
        
        public static PropertiesMustHavePrivateSettersConventionSpecification PropertiesMustHavePrivateSetters
        {
            get {  return new PropertiesMustHavePrivateSettersConventionSpecification(); }
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

        public static MustHaveANonPublicDefaultConstructorConventionSpecification MustHaveANonPublicDefaultConstructor
        {
            get { return new MustHaveANonPublicDefaultConstructorConventionSpecification(); }
        }

        public static MustNotTakeADependencyOnConventionSpecification MustNotTakeADependencyOn(Type type, string reason)
        {
            return new MustNotTakeADependencyOnConventionSpecification(type, reason);
        }

        public static MustHaveAppropriateConstructorsConventionSpecification MustHaveAppropriateConstructors
        {
            get {  return new MustHaveAppropriateConstructorsConventionSpecification(); }
        }

        public static RequiresACorrespondingImplementationOfConventionSpecification RequiresACorrespondingImplementationOf(Type required, Type[] subjects)
        {
            return new RequiresACorrespondingImplementationOfConventionSpecification(required, subjects);
        }

        public static EnumerablePropertiesMustBeEagerLoadedConventionSpecification EnumerablePropertiesMustBeEagerLoadedConventionSpecification
        {
            get { return new EnumerablePropertiesMustBeEagerLoadedConventionSpecification(); }
        }

        public static CollectionPropertiesMustBeImmutableConventionSpecification CollectionPropertiesMustBeImmutable
        {
            get { return new CollectionPropertiesMustBeImmutableConventionSpecification(); }
        }

        public static AllPropertiesMustBeImmutableConventionSpecification AllPropertiesMustBeImmutable
        {
            get { return new AllPropertiesMustBeImmutableConventionSpecification(); }
        }

        public static MustHaveMatchingEmbeddedResourcesConventionSpecification MustHaveMatchingEmbeddedResourcesConventionSpecification(string extension)
        {
            return new MustHaveMatchingEmbeddedResourcesConventionSpecification(extension);
        }

        public static MustNotHaveAPropertyOfTypeConventionSpecification MustNotHaveAPropertyOfType(Type type, string reason)
        {
            return new MustNotHaveAPropertyOfTypeConventionSpecification(type, reason);
        }
    }
}