using System;
using Conventional.Cecil.Conventions;
using Conventional.Conventions.Cecil;

namespace Conventional
{
    public static partial class Convention
    {
        public static MustNotResolveCurrentTimeViaDateTimeConventionSpecification MustNotResolveCurrentTimeViaDateTime
        {
            get { return new MustNotResolveCurrentTimeViaDateTimeConventionSpecification(); }
        }

        public static MustNotUseDateTimeOffsetNowConventionSpecification MustNotUseDateTimeOffsetNow
        {
            get { return new MustNotUseDateTimeOffsetNowConventionSpecification(); }
        }

        public static ExceptionsThrownMustBeDerivedFromConventionSpecification ExceptionsThrownMustBeDerivedFrom(Type baseType)
        {
            return new ExceptionsThrownMustBeDerivedFromConventionSpecification(baseType);
        }

        public static MustInstantiatePropertiesOfSpecifiedTypeInDefaultConstructorConventionSpecification
            MustInstantiatePropertiesOfSpecifiedTypeInDefaultConstructor(Type propertyType)
        {
            return new MustInstantiatePropertiesOfSpecifiedTypeInDefaultConstructorConventionSpecification(propertyType);
        }

        public static MustInstantiatePropertiesOfSpecifiedTypeInDefaultConstructorConventionSpecification
            MustInstantiatePropertiesOfSpecifiedTypeInDefaultConstructor(Type[] propertyTypes)
        {
            return new MustInstantiatePropertiesOfSpecifiedTypeInDefaultConstructorConventionSpecification(propertyTypes);
        }

        public static AllPropertiesMustBeAssignedDuringConstructionConventionSpecification AllPropertiesMustBeAssignedDuringConstruction(bool ignoreTypesWithoutConstructors = false)
        {
            return new AllPropertiesMustBeAssignedDuringConstructionConventionSpecification(ignoreTypesWithoutConstructors);
        }
    }
}