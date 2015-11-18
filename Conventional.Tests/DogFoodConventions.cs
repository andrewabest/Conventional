using Conventional.Cecil;
using Conventional.Conventions;
using Conventional.Extensions;
using NUnit.Framework;

namespace Conventional.Tests
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