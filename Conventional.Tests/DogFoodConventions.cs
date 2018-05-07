using System;
using System.Linq;
using Conventional.Conventions;
using Conventional.Conventions.Assemblies;
using Conventional.Conventions.Database;
using Conventional.Conventions.Solution;
using Conventional.Extensions;
using NUnit.Framework;

namespace Conventional.Tests
{
    public class DogFoodConventions
    {
        [Test]
        public void ConventionSpecifications_MustHaveNameThatEndsWithConventionSpecification()
        {
            var baseAssembly = typeof(Convention).Assembly;

            new[] { baseAssembly }
                .WhereTypes(x => ConventionTypes.Any(c => c.IsAssignableFrom(x)) && x.IsAbstract == false)
                .MustConformTo(Convention.NameMustEndWith("ConventionSpecification"))
                .WithFailureAssertion(Assert.Fail);
        }

        [Test]
        public void GivenAPattern_LocatesAndReturnsAllAssembliesForThatPattern()
        {
            AllAssemblies.WithNamesMatching("*")
                .MustConformTo(Convention.MustNotReferenceDllsFromTransientOrSdkDirectories)
                .WithFailureAssertion(Assert.Fail);
        }

        private static Type[] ConventionTypes => new[]
        {
            typeof (ConventionSpecification),
            typeof (AssemblyConventionSpecification),
            typeof (DatabaseConventionSpecification),
            typeof (SolutionConventionSpecification)
        };
    }
}