using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;

namespace Conventional
{
    public static class ThisSolution
    {
        public static ConventionResult MustConformTo(ISolutionConventionSpecification solutionConventionSpecification)
        {
            var solutionRoot = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), @"../../../"));

            return solutionConventionSpecification.IsSatisfiedBy(solutionRoot);
        }
    }

    public interface IDatabaseConventionSpecification
    {
        ConventionResult IsSatisfiedBy(DatabaseSpecimen databaseSpecimen);
    }

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

    public class DatabaseSpecimen
    {
        public DatabaseSpecimen(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public string ConnectionString { get; private set; }
    }

    public abstract class DatabaseConventionSpecification : IDatabaseConventionSpecification
    {
        protected abstract string FailureMessage { get; }

        protected abstract string EmbeddedScriptName { get; }

        protected string BuildFailureMessage(string details)
        {
            return FailureMessage +
                   Environment.NewLine +
                   details;
        }

        public ConventionResult IsSatisfiedBy(DatabaseSpecimen databaseSpecimen)
        {
            string script;
            using (var stream = typeof(DatabaseConventionSpecification).Assembly.GetManifestResourceStream(EmbeddedScriptName))
            using (var reader = new StreamReader(stream))
            {
                script = reader.ReadToEnd();
            }

            if (string.IsNullOrWhiteSpace(script))
            {
                throw new InvalidOperationException("Resource identified did not contain any SQL script.");
            }

            var failures = new List<string>();
            using (IDbConnection dbConnection = new SqlConnection(databaseSpecimen.ConnectionString))
            {
                dbConnection.Open();
                var command = dbConnection.CreateCommand();
                command.CommandText = script;

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        failures.Add(reader.GetString(0));
                    }
                }
            }

            if (failures.Any())
            {
                
            }

            // Todo: allow for post processing.

            // Todo: return success or failure.
        }
    }
}