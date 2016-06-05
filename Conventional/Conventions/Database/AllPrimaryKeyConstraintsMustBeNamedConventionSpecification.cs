namespace Conventional.Conventions.Database
{
    public class AllPrimaryKeyConstraintsMustBeNamedConventionSpecification : DatabaseConventionSpecification
    {
        protected override string FailureMessage { get { return "All primary key constraints must be explicitly named. {ConstraintName} is not."; } }
    }
}