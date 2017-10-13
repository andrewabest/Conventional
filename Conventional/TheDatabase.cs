namespace Conventional
{
    public static class TheDatabase
    {
        public static DatabaseSpecimen WithConnectionString(string connectionString)
        {
            return new DatabaseSpecimen(connectionString);
        }

        public static ConventionResult MustConformTo(this DatabaseSpecimen databaseSpecimen, IDatabaseConventionSpecification databaseConventionSpecification)
        {
            return databaseConventionSpecification.IsSatisfiedBy(databaseSpecimen);
        }
    }
}