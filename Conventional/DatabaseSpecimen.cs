namespace Conventional
{
    public class DatabaseSpecimen
    {
        public DatabaseSpecimen(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public string ConnectionString { get; private set; }
    }
}