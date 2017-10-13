namespace Conventional.Conventions.Database
{
    public class AllUniqueConstraintsMustBeNamedConventionSpecification : DatabaseConventionSpecification
    {
        protected override string FailureMessage => "All reference constraints must be explicitly named. {ConstraintName} is not.";
    }
}