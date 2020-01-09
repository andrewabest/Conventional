using System;
using Conventional.Conventions.Cecil;

namespace Conventional
{
    public static partial class Convention
    {
        public static MustNotResolveCurrentTimeViaDateTimeConventionSpecification MustNotResolveCurrentTimeViaDateTime => new MustNotResolveCurrentTimeViaDateTimeConventionSpecification();

        public static MustNotUseDateTimeOffsetNowConventionSpecification MustNotUseDateTimeOffsetNow => new MustNotUseDateTimeOffsetNowConventionSpecification();

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

        public static LibraryCodeShouldCallConfigureAwaitWhenAwaitingTasksConventionSpecification LibraryCodeShouldCallConfigureAwaitWhenAwaitingTasks => new LibraryCodeShouldCallConfigureAwaitWhenAwaitingTasksConventionSpecification();
    }
}