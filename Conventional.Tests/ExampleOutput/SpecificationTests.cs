using NUnit.Framework;

namespace Conventional.Tests.ExampleOutput
{
    public class SpecificationTests
    {
        [Test]
        public void PropertiesShouldHavePublicGettersAndSettersConformanceSpecification_FailsOnPrivatePropertyAccessors()
        {
            new[] { typeof(PropertiesShouldHavePublicGettersAndSettersMock) }
                .MustConformTo(Convention.PropertiesShouldHavePublicGetters.And(Convention.PropertiesShouldHavePublicSetters))
                .WithFailureAssertion(Assert.Fail);
        }
    }
}
