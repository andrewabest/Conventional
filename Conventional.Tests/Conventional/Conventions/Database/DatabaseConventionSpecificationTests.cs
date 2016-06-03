using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Resources;
using FluentAssertions;
using NUnit.Framework;

namespace Conventional.Tests.Conventional.Conventions.Database
{
    public class DatabaseConventionSpecificationTests
    {
#if DEBUG
        private const string TestDbConnectionString = @"Server=.\SQLEXPRESS;Database=Conventional;Integrated Security=true;";
#else
        private const string TestDbConnectionString = @"Server=(local)\SQL2014;Database=Conventional;User ID=sa;Password=Password12!";
#endif

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
        public void AllCheckConstraintsMustBeNamedConventionalSpecification_Success()
        {
            ExecuteSqlScriptFromResource("AllCheckConstraintsMustBeNamedConventionalSpecification_Success.sql");

            TheDatabase
                .WithConnectionString(TestDbConnectionString)
                .MustConformTo(Convention.AllCheckConstraintsMustBeNamed)
                .IsSatisfied
                .Should()
                .BeTrue();
        }

        [Test]
        public void AllCheckConstraintsMustBeNamedConventionalSpecification_Fail()
        {
            ExecuteSqlScriptFromResource("AllCheckConstraintsMustBeNamedConventionalSpecification_Fail.sql");

            var result = TheDatabase
                .WithConnectionString(TestDbConnectionString)
                .MustConformTo(Convention.AllCheckConstraintsMustBeNamed);

            result.IsSatisfied.Should().BeFalse();
            result.Failures.Should().HaveCount(1);
        }

        [Test]
        public void AllDefaultConstraintsMustBeNamedConventionalSpecification_Success()
        {
            ExecuteSqlScriptFromResource("AllDefaultConstraintsMustBeNamedConventionalSpecification_Success.sql");

            TheDatabase
                .WithConnectionString(TestDbConnectionString)
                .MustConformTo(Convention.AllDefaultConstraintsMustBeNamed)
                .IsSatisfied
                .Should()
                .BeTrue();
        }

        [Test]
        public void AllDefaultConstraintsMustBeNamedConventionalSpecification_Fail()
        {
            ExecuteSqlScriptFromResource("AllDefaultConstraintsMustBeNamedConventionalSpecification_Fail.sql");

            var result = TheDatabase
                .WithConnectionString(TestDbConnectionString)
                .MustConformTo(Convention.AllDefaultConstraintsMustBeNamed);

            result.IsSatisfied.Should().BeFalse();
            result.Failures.Should().HaveCount(1);
        }

        [Test]
        public void AllPrimaryKeyConstraintsMustBeNamedConventionalSpecification_Success()
        {
            ExecuteSqlScriptFromResource("AllPrimaryKeyConstraintsMustBeNamedConventionalSpecification_Success.sql");

            TheDatabase
                .WithConnectionString(TestDbConnectionString)
                .MustConformTo(Convention.AllPrimaryKeyConstraintsMustBeNamed)
                .IsSatisfied
                .Should()
                .BeTrue();
        }

        [Test]
        public void AllPrimaryKeyConstraintsMustBeNamedConventionalSpecification_Fail()
        {
            ExecuteSqlScriptFromResource("AllPrimaryKeyConstraintsMustBeNamedConventionalSpecification_Fail.sql");

            var result = TheDatabase
                .WithConnectionString(TestDbConnectionString)
                .MustConformTo(Convention.AllPrimaryKeyConstraintsMustBeNamed);

            result.IsSatisfied.Should().BeFalse();
            result.Failures.Should().HaveCount(1);
        }

        [Test]
        public void AllReferenceConstraintsMustBeNamedConventionalSpecification_Success()
        {
            ExecuteSqlScriptFromResource("AllReferenceConstraintsMustBeNamedConventionalSpecification_Success.sql");

            TheDatabase
                .WithConnectionString(TestDbConnectionString)
                .MustConformTo(Convention.AllReferenceConstraintsMustBeNamed)
                .IsSatisfied
                .Should()
                .BeTrue();
        }

        [Test]
        public void AllReferenceConstraintsMustBeNamedConventionalSpecification_Fail()
        {
            ExecuteSqlScriptFromResource("AllReferenceConstraintsMustBeNamedConventionalSpecification_Fail.sql");

            var result = TheDatabase
                .WithConnectionString(TestDbConnectionString)
                .MustConformTo(Convention.AllReferenceConstraintsMustBeNamed);

            result.IsSatisfied.Should().BeFalse();
            result.Failures.Should().HaveCount(1);
        }

        [Test]
        public void AllUniqueConstraintsMustBeNamedConventionalSpecification_Success()
        {
            ExecuteSqlScriptFromResource("AllUniqueConstraintsMustBeNamedConventionalSpecification_Success.sql");

            TheDatabase
                .WithConnectionString(TestDbConnectionString)
                .MustConformTo(Convention.AllUniqueConstraintsMustBeNamed)
                .IsSatisfied
                .Should()
                .BeTrue();
        }

        [Test]
        public void AllUniqueConstraintsMustBeNamedConventionalSpecification_Fail()
        {
            ExecuteSqlScriptFromResource("AllUniqueConstraintsMustBeNamedConventionalSpecification_Fail.sql");

            var result = TheDatabase
                .WithConnectionString(TestDbConnectionString)
                .MustConformTo(Convention.AllUniqueConstraintsMustBeNamed);

            result.IsSatisfied.Should().BeFalse();
            result.Failures.Should().HaveCount(1);
        }

        [Test]
        public void AllIdentityColumnsMustBeNamedTableNameIdConventionSpecification_Success()
        {
            ExecuteSqlScriptFromResource("AllIdentityColumnsMustBeNamedTableNameIdConventionSpecificationSuccess.sql");

            TheDatabase
                .WithConnectionString(TestDbConnectionString)
                .MustConformTo(Convention.AllIdentityColumnsMustBeNamedTableNameId)
                .IsSatisfied
                .Should()
                .BeTrue();
        }
        
        [Test]
        public void AllIdentityColumnsMustBeNamedTableNameIdConventionSpecification_FailsWhenIdentityColumnIsNotNamedId()
        {
            ExecuteSqlScriptFromResource("AllIdentityColumnsMustBeNamedTableNameIdConventionSpecificationFailure.sql");

            var result = TheDatabase
                .WithConnectionString(TestDbConnectionString)
                .MustConformTo(Convention.AllIdentityColumnsMustBeNamedTableNameId);

            result.IsSatisfied.Should().BeFalse();
            result.Failures.Should().HaveCount(1);
        }
        
        [Test]
        public void AllTablesMustHaveAClusteredIndex_Success()
        {
            ExecuteSqlScriptFromResource("TablesWithoutClusteredIndexSuccess.sql");

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
            ExecuteSqlScriptFromResource("TablesWithoutClusteredIndexFailure.sql");

            var result = TheDatabase
                .WithConnectionString(TestDbConnectionString)
                .MustConformTo(Convention.AllTablesMustHaveAClusteredIndex);

            result.IsSatisfied.Should().BeFalse();
            result.Failures.Should().HaveCount(1);
        }
        
        private static void ExecuteSqlScriptFromResource(string resourceName)
        {
            string script;

            var fullResourceName = ScriptNamespace.Qualifier + "." + resourceName;
            using (var stream = typeof (SqlScripts).Assembly.GetManifestResourceStream(fullResourceName))
            {
                if (stream == null) throw new MissingManifestResourceException(fullResourceName);
                using (var reader = new StreamReader(stream))
                {
                    script = reader.ReadToEnd();
                }
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
