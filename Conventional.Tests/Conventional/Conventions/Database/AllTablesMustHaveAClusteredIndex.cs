using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace Conventional.Tests.Conventional.Conventions.Database
{
    [Ignore("Need to sort out CI for these dudes.")]
    public class AllTablesMustHaveAClusteredIndex
    {
        private const string TestDbConnectionString = @"Server=.\SQLEXPRESS;Database=Conventional;Integrated Security=true;";


        [SetUp]
        public void Setup()
        {
            CreateDatabase();
        }

        [TearDown]
        public void Teardown()
        {
            DropDatabase();
        }

        [Test]
        public void AllTablesMustHaveAClusteredIndex_Success()
        {
            ExecuteSqlScriptFromResource("Conventional.Tests.Conventional.Conventions.Database.Scripts.TablesWithoutClusteredIndexSuccess.sql");

            TheDatabase
                .WithConnectionString(TestDbConnectionString)
                .MustConformTo(Convention.AllTablesMustHaveAClusteredIndex)
                .IsSatisfied
                .Should()
                .BeTrue();
        }

        [Test]
        public void AllTablesMustHaveAClusteredIndex_Failure()
        {
            ExecuteSqlScriptFromResource("Conventional.Tests.Conventional.Conventions.Database.Scripts.TablesWithoutClusteredIndexFailure.sql");

            TheDatabase
                .WithConnectionString(TestDbConnectionString)
                .MustConformTo(Convention.AllTablesMustHaveAClusteredIndex)
                .IsSatisfied
                .Should()
                .BeFalse();
        }

        private static void ExecuteSqlScriptFromResource(string resourceName)
        {
            string script;
            using (var stream = typeof(DatabaseConventionSpecificationTests).Assembly.GetManifestResourceStream(resourceName))
            using (var reader = new StreamReader(stream))
            {
                script = reader.ReadToEnd();
            }

            using (IDbConnection dbConnection = new SqlConnection(TestDbConnectionString))
            {
                dbConnection.Open();
                var command = dbConnection.CreateCommand();
                command.CommandText = script;
                command.ExecuteNonQuery();
            }
        }

        private static void CreateDatabase()
        {
            var sb = new SqlConnectionStringBuilder(TestDbConnectionString);
            var dbName = sb.InitialCatalog;
            sb.InitialCatalog = "master";

            using (IDbConnection dbConnection = new SqlConnection(sb.ConnectionString))
            {
                dbConnection.Open();
                var command = dbConnection.CreateCommand();
                command.CommandText = SqlScripts.CreateDb(dbName);
                command.ExecuteNonQuery();
            }
        }
        
        private static void DropDatabase()
        {
            var sb = new SqlConnectionStringBuilder(TestDbConnectionString);
            var dbName = sb.InitialCatalog;
            sb.InitialCatalog = "master";

            using (IDbConnection dbConnection = new SqlConnection(sb.ConnectionString))
            {
                dbConnection.Open();
                var command = dbConnection.CreateCommand();
                command.CommandText = SqlScripts.DropDb(dbName);
                command.ExecuteNonQuery();
            }
        }
    }
}
