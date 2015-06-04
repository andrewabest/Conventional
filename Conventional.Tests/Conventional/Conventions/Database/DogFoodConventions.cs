using System.Linq;
using Conventional.Conventions.Database;
using NUnit.Framework;

namespace Conventional.Tests.Conventional.Conventions.Database
{
    public class DogFoodConventions
    {
        [Test]
        public void DatabaseConventions_MustHaveMatchingSqlScriptEmbeddedResources()
        {
            typeof (DatabaseConventionSpecification).Assembly.GetExportedTypes()
                .Where(x => typeof (DatabaseConventionSpecification).IsAssignableFrom(x) && !x.IsAbstract)
                .MustConformTo(Convention.MustHaveMatchingEmbeddedResourcesConventionSpecification(".sql"))
                .WithFailureAssertion(Assert.Fail);
        }
    }
}