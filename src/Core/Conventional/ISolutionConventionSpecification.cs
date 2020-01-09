namespace Conventional
{
    public interface ISolutionConventionSpecification
    {
        ConventionResult IsSatisfiedBy(string solutionRoot);
    }
}