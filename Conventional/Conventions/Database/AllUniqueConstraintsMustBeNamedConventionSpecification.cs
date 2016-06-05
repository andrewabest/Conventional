namespace Conventional.Conventions.Database
{
    public class AllUniqueConstraintsMustBeNamedConventionSpecification : DatabaseConventionSpecification
    {
        protected override string FailureMessage { get { return "All reference constraints must be explicitly named. {ConstraintName} is not."; } }
    }
}