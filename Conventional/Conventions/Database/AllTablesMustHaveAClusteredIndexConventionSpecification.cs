namespace Conventional.Conventions.Database
{
    public class AllTablesMustHaveAClusteredIndexConventionSpecification : DatabaseConventionSpecification
    {
        protected override string FailureMessage
        {
            get { return "All tables must have a clustered index - {TableName} does not."; }
        }
    }
}