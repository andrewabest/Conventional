using System;
using System.Linq;
using Conventional.Extensions;
using Conventional.Roslyn.Conventions;
using NUnit.Framework;

namespace Conventional.Roslyn.Tests
{
    public class DogFoodConventions
    {
        [Test]
        public void ConventionSpecifications_MustHaveNameThatEndsWithConventionSpecification()
        {
            var baseAssembly = typeof(RoslynConvention).Assembly;

            new[] { baseAssembly }
                .WhereTypes(x => ConventionTypes.Any(c => c.IsAssignableFrom(x)) && x.IsAbstract == false)
                .MustConformTo(Convention.NameMustEndWith("ConventionSpecification"))
                .WithFailureAssertion(Assert.Fail);
        }

        private Type[] ConventionTypes => new[]
        {
            typeof (SolutionDiagnosticAnalyzerConventionSpecification)
        };
    }
}
