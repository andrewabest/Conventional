using Conventional.Conventions;
using NUnit.Framework;

namespace Conventional.Tests.Net45
{
    public class DogFoodConventions
    {
        [Test]
        public void ConventionSpecifications_MustHaveNameThatEndsWithConventionSpecification()
        {
            var baseAssembly = typeof (Convention).Assembly;

            new[] { baseAssembly }
                .WhereTypes(x => typeof(ConventionSpecification).IsAssignableFrom(x) && x.IsAbstract == false)
                .MustConformTo(Convention.NameMustEndWith("ConventionSpecification"))
                .WithFailureAssertion(Assert.Fail);
        }
    }
}