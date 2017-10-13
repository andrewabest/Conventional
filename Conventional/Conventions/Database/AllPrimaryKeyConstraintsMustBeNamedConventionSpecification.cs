namespace Conventional.Conventions.Database
{
    public class AllPrimaryKeyConstraintsMustBeNamedConventionSpecification : DatabaseConventionSpecification
    {
        protected override string FailureMessage => "All primary key constraints must be explicitly named. {ConstraintName} is not.";
    }
}