using Conventional.Cecil;
using Conventional.Conventions;
using NUnit.Framework;

namespace Conventional.Tests
{
    public class DogFoodConventions
    {
        [Test]
        public void ConventionSpecifications_MustHaveNameThatEndsWithConventionSpecification()
        {
            var baseAssembly = typeof (Convention).Assembly;
            var cecilAssembly = typeof (CecilConvention).Assembly;

            new[] { baseAssembly, cecilAssembly }
                .WhereTypes(x => typeof(ConventionSpecification).IsAssignableFrom(x) && x.IsAbstract == false)
                .MustConformTo(Convention.NameMustEndWith("ConventionSpecification"))
                .WithFailureAssertion(Assert.Fail);
        }
    }
}