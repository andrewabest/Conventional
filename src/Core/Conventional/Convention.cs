using System;
using System.Collections.Generic;
using Conventional.Conventions;

namespace Conventional
{
    public static partial class Convention
    {
        public static PropertiesMustBePublicConventionSpecification PropertiesMustBePublic => new PropertiesMustBePublicConventionSpecification();

        public static PropertiesMustHavePublicGettersConventionSpecification PropertiesMustHavePublicGetters => new PropertiesMustHavePublicGettersConventionSpecification();

        public static PropertiesMustHavePublicSettersConventionSpecification PropertiesMustHavePublicSetters => new PropertiesMustHavePublicSettersConventionSpecification();

        public static PropertiesMustHaveProtectedSettersConventionSpecification PropertiesMustHaveProtectedSetters => new PropertiesMustHaveProtectedSettersConventionSpecification();

        public static PropertiesMustHavePrivateSettersConventionSpecification PropertiesMustHavePrivateSetters => new PropertiesMustHavePrivateSettersConventionSpecification();

        public static PropertiesOfTypeMustHaveAttributeConventionSpecification PropertiesOfTypeMustHaveAttribute(Type propertyType, Type attributeType, bool writablePropertiesOnly = true)
        {
            return new PropertiesOfTypeMustHaveAttributeConventionSpecification(propertyType, attributeType, writablePropertiesOnly);
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

        public static MustHaveADefaultConstructorConventionSpecification MustHaveADefaultConstructor => new MustHaveADefaultConstructorConventionSpecification();

        public static MustHaveANonPublicDefaultConstructorConventionSpecification MustHaveANonPublicDefaultConstructor => new MustHaveANonPublicDefaultConstructorConventionSpecification();

        public static MustNotTakeADependencyOnConventionSpecification MustNotTakeADependencyOn(Type type, string reason)
        {
            return new MustNotTakeADependencyOnConventionSpecification(type, reason);
        }

        public static MustHaveAppropriateConstructorsConventionSpecification MustHaveAppropriateConstructors => new MustHaveAppropriateConstructorsConventionSpecification();

        public static RequiresACorrespondingImplementationOfConventionSpecification RequiresACorrespondingImplementationOf(Type required, Type[] subjects)
        {
            return new RequiresACorrespondingImplementationOfConventionSpecification(required, subjects);
        }

        public static EnumerablePropertiesMustBeEagerLoadedConventionSpecification EnumerablePropertiesMustBeEagerLoaded => new EnumerablePropertiesMustBeEagerLoadedConventionSpecification();

        public static CollectionPropertiesMustNotHaveSettersConventionSpecification CollectionPropertiesMustNotHaveSetters => new CollectionPropertiesMustNotHaveSettersConventionSpecification();

        public static PropertiesMustNotHaveSettersConventionSpecification PropertiesMustNotHaveSetters => new PropertiesMustNotHaveSettersConventionSpecification();

        public static MustHaveMatchingEmbeddedResourcesConventionSpecification MustHaveMatchingEmbeddedResources(string extension)
        {
            return new MustHaveMatchingEmbeddedResourcesConventionSpecification(extension);
        }

        public static MustHaveMatchingEmbeddedResourcesConventionSpecification MustHaveMatchingEmbeddedResources(Func<Type, string> resourceNameMatcher)
        {
            return new MustHaveMatchingEmbeddedResourcesConventionSpecification(resourceNameMatcher);
        }

        public static MustNotHaveAPropertyOfTypeConventionSpecification MustNotHaveAPropertyOfType(Type type, string reason)
        {
            return new MustNotHaveAPropertyOfTypeConventionSpecification(type, reason);
        }

        public static MustHaveCorrespondingEnumConventionSpecification MustHaveCorrespondingEnum(IEnumerable<Type> sourceTypes)
        {
            return new MustHaveCorrespondingEnumConventionSpecification(sourceTypes);
        }

        public static VoidMethodsMustNotBeAsyncConventionSpecification VoidMethodsMustNotBeAsync => new VoidMethodsMustNotBeAsyncConventionSpecification();

        public static AsyncMethodsMustHaveAsyncSuffixConventionSpecification AsyncMethodsMustHaveAsyncSuffix => new AsyncMethodsMustHaveAsyncSuffixConventionSpecification();
    }
}