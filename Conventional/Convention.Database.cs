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
    }
}