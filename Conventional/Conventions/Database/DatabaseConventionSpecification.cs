using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Resources;

namespace Conventional.Conventions.Database
{
    public abstract class DatabaseConventionSpecification : IDatabaseConventionSpecification
    {
        private const string DatabaseConventionResultIdentifier = "Database convention";

        protected abstract string FailureMessage { get; }

        public ConventionResult IsSatisfiedBy(DatabaseSpecimen databaseSpecimen)
        {
            var script = GetScript();

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
                return ConventionResult.NotSatisfied(DatabaseConventionResultIdentifier,
                    FailureMessage + Environment.NewLine +
                    failures.Aggregate((x, y) => x + Environment.NewLine + y));
            }

            return ConventionResult.Satisfied(DatabaseConventionResultIdentifier);
        }

        protected virtual string GetScript()
        {
            var resourceName = GetType().FullName + ".sql";

            var assembly =
                GetType().Assembly.GetManifestResourceNames().Contains(resourceName) ?
                GetType().Assembly : typeof(DatabaseConventionSpecification).Assembly;

            string script;
            using (var stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream == null) { throw new MissingManifestResourceException(resourceName); }
                
                using (var reader = new StreamReader(stream))
                {
                    script = reader.ReadToEnd();
                }
            }
            return script;
        }
    }
}