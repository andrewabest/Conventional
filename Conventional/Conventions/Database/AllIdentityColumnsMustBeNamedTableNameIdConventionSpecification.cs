namespace Conventional.Conventions.Database
{
    public class AllIdentityColumnsMustBeNamedTableNameIdConventionSpecification : DatabaseConventionSpecification
    {
        protected override string FailureMessage
        {
            get { return "All identity columns must be named {TableName}Id."; }
        }
    }
}