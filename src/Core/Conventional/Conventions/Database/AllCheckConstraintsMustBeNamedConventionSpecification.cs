namespace Conventional.Conventions.Database
{
    public class AllCheckConstraintsMustBeNamedConventionSpecification : DatabaseConventionSpecification
    {
        protected override string FailureMessage => "All check constraints must be explicitly named. {ConstraintName} is not.";
    }
}