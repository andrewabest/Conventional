namespace Conventional
{
    public interface IDatabaseConventionSpecification
    {
        ConventionResult IsSatisfiedBy(DatabaseSpecimen databaseSpecimen);
    }
}