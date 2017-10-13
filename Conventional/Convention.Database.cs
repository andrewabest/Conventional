using Conventional.Conventions.Database;

namespace Conventional
{
    public static partial class Convention
    {
        public static AllIdentityColumnsMustBeNamedTableNameIdConventionSpecification AllIdentityColumnsMustBeNamedTableNameId => new AllIdentityColumnsMustBeNamedTableNameIdConventionSpecification();

        public static AllTablesMustHaveAClusteredIndexConventionSpecification AllTablesMustHaveAClusteredIndex => new AllTablesMustHaveAClusteredIndexConventionSpecification();

        public static AllCheckConstraintsMustBeNamedConventionSpecification AllCheckConstraintsMustBeNamed => new AllCheckConstraintsMustBeNamedConventionSpecification();

        public static AllDefaultConstraintsMustBeNamedConventionSpecification AllDefaultConstraintsMustBeNamed => new AllDefaultConstraintsMustBeNamedConventionSpecification();

        public static AllNamedColumnsMustBeNullableConventionSpecification AllNamedColumnsMustBeNullable(string columnName)
        {
            return new AllNamedColumnsMustBeNullableConventionSpecification(columnName);
        }

        public static AllNamedColumnsMustBeNonNullableConventionSpecification AllNamedColumnsMustBeNonNullable(string columnName)
        {
            return new AllNamedColumnsMustBeNonNullableConventionSpecification(columnName);
        }

        public static AllPrimaryKeyConstraintsMustBeNamedConventionSpecification AllPrimaryKeyConstraintsMustBeNamed => new AllPrimaryKeyConstraintsMustBeNamedConventionSpecification();

        public static AllReferenceConstraintsMustBeNamedConventionSpecification AllReferenceConstraintsMustBeNamed => new AllReferenceConstraintsMustBeNamedConventionSpecification();

        public static AllUniqueConstraintsMustBeNamedConventionSpecification AllUniqueConstraintsMustBeNamed => new AllUniqueConstraintsMustBeNamedConventionSpecification();

        public static EachRowMustHaveACorrespondingEnumConventionSpecification EachRowMustHaveACorrespondingEnum<T>(string tableName, string columnName) where T: struct
        {
            return new EachRowMustHaveACorrespondingEnumConventionSpecification(tableName, columnName, typeof(T));
        }
    }
}