using Conventional.Conventions.Database;

namespace Conventional
{
    public static partial class Convention
    {
        public static AllIdentityColumnsMustBeNamedTableNameIdConventionSpecification AllIdentityColumnsMustBeNamedTableNameId
        {
            get {  return new AllIdentityColumnsMustBeNamedTableNameIdConventionSpecification(); }
        }

        public static AllTablesMustHaveAClusteredIndexConventionSpecification AllTablesMustHaveAClusteredIndex
        {
            get { return new AllTablesMustHaveAClusteredIndexConventionSpecification(); }
        }

        public static AllCheckConstraintsMustBeNamedConventionSpecification AllCheckConstraintsMustBeNamed
        {
            get { return new AllCheckConstraintsMustBeNamedConventionSpecification(); }
        }

        public static AllDefaultConstraintsMustBeNamedConventionSpecification AllDefaultConstraintsMustBeNamed
        {
            get { return new AllDefaultConstraintsMustBeNamedConventionSpecification(); }
        }

        public static AllPrimaryKeyConstraintsMustBeNamedConventionSpecification AllPrimaryKeyConstraintsMustBeNamed
        {
            get { return new AllPrimaryKeyConstraintsMustBeNamedConventionSpecification(); }
        }

        public static AllReferenceConstraintsMustBeNamedConventionSpecification AllReferenceConstraintsMustBeNamed
        {
            get { return new AllReferenceConstraintsMustBeNamedConventionSpecification(); }
        }

        public static AllUniqueConstraintsMustBeNamedConventionSpecification AllUniqueConstraintsMustBeNamed
        {
            get { return new AllUniqueConstraintsMustBeNamedConventionSpecification(); }
        }

    }
}