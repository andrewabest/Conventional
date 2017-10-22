namespace Conventional.Conventions.Database
{
    public class AllReferenceConstraintsMustBeNamedConventionSpecification : DatabaseConventionSpecification
    {
        protected override string FailureMessage => "All reference constraints must be explicitly named. {ConstraintName} is not.";
    }
}