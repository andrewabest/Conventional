using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Resources;
using FluentAssertions;
using NUnit.Framework;

namespace Conventional.Tests.Conventional.Conventions.Database
{
    public class DatabaseConventionSpecificationTests
    {
        private readonly DevelopmentSettings _settings = DevelopmentSettings.Create();

        [SetUp]
        public void Setup()
        {
            try
            {
                CreateDatabase();
            }
            catch (SqlException e)
            {
                Assert.Fail(
                    "Conventional's test suite requires a default named .\\SQLEXPRESS instance. If you want to use an alternative database instance, create a development.json file in the solution root with your desired connection string.\n See development.json.example in the solution root for an example that uses LocalDB."
                    + Environment.NewLine
                    + Environment.NewLine
                    + $"Exception Message: {e.Message}"
                    + Environment.NewLine
                    + Environment.NewLine
                    + $"Stack Trace: {e.StackTrace}");
            }
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
                .WithConnectionString(_settings.ConnectionString)
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
                .WithConnectionString(_settings.ConnectionString)
                .MustConformTo(Convention.AllCheckConstraintsMustBeNamed);

            result.IsSatisfied.Should().BeFalse();
            result.Failures.Should().HaveCount(1);
        }

        [Test]
        public void AllDefaultConstraintsMustBeNamedConventionalSpecification_Success()
        {
            ExecuteSqlScriptFromResource("AllDefaultConstraintsMustBeNamedConventionalSpecification_Success.sql");

            TheDatabase
                .WithConnectionString(_settings.ConnectionString)
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
                .WithConnectionString(_settings.ConnectionString)
                .MustConformTo(Convention.AllDefaultConstraintsMustBeNamed);

            result.IsSatisfied.Should().BeFalse();
            result.Failures.Should().HaveCount(1);
        }

        [Test]
        public void AllPrimaryKeyConstraintsMustBeNamedConventionalSpecification_Success()
        {
            ExecuteSqlScriptFromResource("AllPrimaryKeyConstraintsMustBeNamedConventionalSpecification_Success.sql");

            TheDatabase
                .WithConnectionString(_settings.ConnectionString)
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
                .WithConnectionString(_settings.ConnectionString)
                .MustConformTo(Convention.AllPrimaryKeyConstraintsMustBeNamed);

            result.IsSatisfied.Should().BeFalse();
            result.Failures.Should().HaveCount(1);
        }

        [Test]
        public void AllReferenceConstraintsMustBeNamedConventionalSpecification_Success()
        {
            ExecuteSqlScriptFromResource("AllReferenceConstraintsMustBeNamedConventionalSpecification_Success.sql");

            TheDatabase
                .WithConnectionString(_settings.ConnectionString)
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
                .WithConnectionString(_settings.ConnectionString)
                .MustConformTo(Convention.AllReferenceConstraintsMustBeNamed);

            result.IsSatisfied.Should().BeFalse();
            result.Failures.Should().HaveCount(1);
        }

        [Test]
        public void AllUniqueConstraintsMustBeNamedConventionalSpecification_Success()
        {
            ExecuteSqlScriptFromResource("AllUniqueConstraintsMustBeNamedConventionalSpecification_Success.sql");

            TheDatabase
                .WithConnectionString(_settings.ConnectionString)
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
                .WithConnectionString(_settings.ConnectionString)
                .MustConformTo(Convention.AllUniqueConstraintsMustBeNamed);

            result.IsSatisfied.Should().BeFalse();
            result.Failures.Should().HaveCount(1);
        }

        [Test]
        public void AllNamedColumnsMustBeNullableConventionSpecification_Success()
        {
            ExecuteSqlScriptFromResource("AllNamedColumnsMustBeNullableConventionalSpecification_Success.sql");

            TheDatabase
                .WithConnectionString(_settings.ConnectionString)
                .MustConformTo(Convention.AllNamedColumnsMustBeNullable("UpdatedDateTime"))
                .IsSatisfied
                .Should()
                .BeTrue();
        }

        [Test]
        public void AllNamedColumnsMustBeNonNullableConventionSpecification_Fails()
        {
            ExecuteSqlScriptFromResource("AllNamedColumnsMustBeNullableConventionalSpecification_Fail.sql");

            var result = TheDatabase
                .WithConnectionString(_settings.ConnectionString)
                .MustConformTo(Convention.AllNamedColumnsMustBeNullable("UpdatedDateTime"));

            result.IsSatisfied.Should().BeFalse();
            result.Failures.Should().HaveCount(1);
        }

        [Test]
        public void AllNamedColumnsMustBeNonNullableConventionSpecification_Success()
        {
            ExecuteSqlScriptFromResource("AllNamedColumnsMustBeNullableConventionalSpecification_Success.sql");

            TheDatabase
                .WithConnectionString(_settings.ConnectionString)
                .MustConformTo(Convention.AllNamedColumnsMustBeNonNullable("CreatedDateTime"))
                .IsSatisfied
                .Should()
                .BeTrue();
        }

        [Test]
        public void AllNamedColumnsMustBeNullableConventionSpecification_Fails()
        {
            ExecuteSqlScriptFromResource("AllNamedColumnsMustBeNullableConventionalSpecification_Fail.sql");

            var result = TheDatabase
                .WithConnectionString(_settings.ConnectionString)
                .MustConformTo(Convention.AllNamedColumnsMustBeNullable("UpdatedDateTime"));

            result.IsSatisfied.Should().BeFalse();
            result.Failures.Should().HaveCount(1);
        }

        [Test]
        public void AllIdentityColumnsMustBeNamedTableNameIdConventionSpecification_Success()
        {
            ExecuteSqlScriptFromResource("AllIdentityColumnsMustBeNamedTableNameIdConventionSpecificationSuccess.sql");

            TheDatabase
                .WithConnectionString(_settings.ConnectionString)
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
                .WithConnectionString(_settings.ConnectionString)
                .MustConformTo(Convention.AllIdentityColumnsMustBeNamedTableNameId);

            result.IsSatisfied.Should().BeFalse();
            result.Failures.Should().HaveCount(1);
        }
        
        [Test]
        public void AllTablesMustHaveAClusteredIndex_Success()
        {
            ExecuteSqlScriptFromResource("TablesWithoutClusteredIndexSuccess.sql");

            TheDatabase
                .WithConnectionString(_settings.ConnectionString)
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
                .WithConnectionString(_settings.ConnectionString)
                .MustConformTo(Convention.AllTablesMustHaveAClusteredIndex);

            result.IsSatisfied.Should().BeFalse();
            result.Failures.Should().HaveCount(1);
        }

        [Test]
        public void EachRowMustHaveACorrespondingEnum_Success()
        {
            ExecuteSqlScriptFromResource("EachRowMustHaveACorrespondingEnum_Success.sql");

            TheDatabase
                .WithConnectionString(_settings.ConnectionString)
                .MustConformTo(Convention.EachRowMustHaveACorrespondingEnum<CloudServiceEnum>("dbo.CloudService", "CloudServiceId"))
                .IsSatisfied
                .Should()
                .BeTrue();
        }

        [Test]
        public void EachRowMustHaveACorrespondingEnum_Fail()
        {
            ExecuteSqlScriptFromResource("EachRowMustHaveACorrespondingEnum_Fail.sql");

            var result = TheDatabase
                .WithConnectionString(_settings.ConnectionString)
                .MustConformTo(Convention.EachRowMustHaveACorrespondingEnum<CloudServiceEnum>("dbo.CloudService", "CloudServiceId"));

            result.IsSatisfied.Should().BeFalse();
            result.Failures.Should().HaveCount(1);
        }

        private void ExecuteSqlScriptFromResource(string resourceName)
        {
            string script;

            var fullResourceName = ScriptNamespace.Qualifier + "." + resourceName;
            using (var stream = typeof (SqlScripts).Assembly.GetManifestResourceStream(fullResourceName))
            {
                if (stream == null) { throw new MissingManifestResourceException(fullResourceName); }

                using (var reader = new StreamReader(stream))
                {
                    script = reader.ReadToEnd();
                }
            }

            using (IDbConnection dbConnection = new SqlConnection(_settings.ConnectionString))
            {
                dbConnection.Open();
                var command = dbConnection.CreateCommand();
                command.CommandText = script;
                command.ExecuteNonQuery();
            }
        }

        private void CreateDatabase()
        {
            var sb = new SqlConnectionStringBuilder(_settings.ConnectionString);
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
        
        private void DropDatabase()
        {
            var sb = new SqlConnectionStringBuilder(_settings.ConnectionString);
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
