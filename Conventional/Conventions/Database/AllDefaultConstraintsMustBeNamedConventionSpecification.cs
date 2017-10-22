namespace Conventional.Conventions.Database
{
    public class AllDefaultConstraintsMustBeNamedConventionSpecification : DatabaseConventionSpecification
    {
        protected override string FailureMessage => "All default constraints must be explicitly named. {ConstraintName} is not.";
    }
}