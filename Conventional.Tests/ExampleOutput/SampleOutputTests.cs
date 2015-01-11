using NUnit.Framework;

namespace Conventional.Tests.ExampleOutput
{
    //[Ignore]
    public class SampleOutputTests
    {
        private class PropertiesMustHavePublicGettersAndSettersMock
        {
            public string Public { get; set; }
            public string PrivateGet { private get; set; }
            public string PrivateSet { get; private set; }
        }

        [Test]
        public void PropertiesMustHavePublicGettersAndSettersConformanceSpecification_FailsOnPrivatePropertyAccessors()
        {
            new[] { typeof(PropertiesMustHavePublicGettersAndSettersMock) }
                .MustConformTo(Convention.PropertiesMustHavePublicGetters.And(Convention.PropertiesMustHavePublicSetters))
                .WithFailureAssertion(Assert.Fail);
        }
        
        [Test]
        public void PropertiesMustHavePublicGettersAndSettersConformanceSpecification_FailsOnPrivatePropertyAccessors_ChainedFluentSyntax()
        {
            new[] { typeof(PropertiesMustHavePublicGettersAndSettersMock) }
                .MustConformTo(Convention.PropertiesMustHavePublicGetters)
                .AndMustConformTo(Convention.PropertiesMustHavePublicSetters)
                .WithFailureAssertion(Assert.Fail);
        }
    }
}
