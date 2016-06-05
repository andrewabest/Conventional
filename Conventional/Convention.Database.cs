using System;
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

        public static AllNamedColumnsMustBeNullableConventionSpecification AllNamedColumnsMustBeNullable(string columnName)
        {
            return new AllNamedColumnsMustBeNullableConventionSpecification(columnName);
        }

        public static AllNamedColumnsMustBeNonNullableConventionSpecification AllNamedColumnsMustBeNonNullable(string columnName)
        {
            return new AllNamedColumnsMustBeNonNullableConventionSpecification(columnName);
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

        public static EachRowMustHaveACorrespondingEnumConventionSpecification EachRowMustHaveACorrespondingEnum<T>(string tableName, string columnName) where T: struct
        {
            return new EachRowMustHaveACorrespondingEnumConventionSpecification(tableName, columnName, typeof(T));
        }
    }
}