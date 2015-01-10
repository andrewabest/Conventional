using NUnit.Framework;

namespace Conventional.Tests.ExampleOutput
{
    //[Ignore]
    public class SampleOutputTests
    {
        private class PropertiesShouldHavePublicGettersAndSettersMock
        {
            public string Public { get; set; }
            public string PrivateGet { private get; set; }
            public string PrivateSet { get; private set; }
        }

        [Test]
        public void PropertiesShouldHavePublicGettersAndSettersConformanceSpecification_FailsOnPrivatePropertyAccessors()
        {
            new[] { typeof(PropertiesShouldHavePublicGettersAndSettersMock) }
                .MustConformTo(Convention.PropertiesShouldHavePublicGetters.And(Convention.PropertiesShouldHavePublicSetters))
                .WithFailureAssertion(Assert.Fail);
        }
    }
}
