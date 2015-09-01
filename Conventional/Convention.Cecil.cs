using System;
using Conventional.Cecil.Conventions;
using Conventional.Conventions.Cecil;

namespace Conventional
{
    public static partial class Convention
    {
        public static MustNotResolveCurrentTimeViaDateTimeDirectlyConventionSpecification MustNotResolveCurrentTimeViaDateTimeDirectly
        {
            get { return new MustNotResolveCurrentTimeViaDateTimeDirectlyConventionSpecification(); }
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

        public static AllPropertiesMustBeAssignedDuringConstructionConventionSpecification AllPropertiesMustBeAssignedDuringConstruction(bool ignoreTypesWithoutConstructors = false)
        {
            return new AllPropertiesMustBeAssignedDuringConstructionConventionSpecification(ignoreTypesWithoutConstructors);
        }
    }
}