using NUnit.Framework;

namespace Conventional.Tests
{
    public class ConventionConfigurationScenarios
    {
        [SetUp]
        public void Setup()
        {
            ConventionConfiguration.DefaultFailureAssertionCallback = Assert.Pass;
        }

        private class AbjectConformanceFailure
        {
            public AbjectConformanceFailure(string name, string description)
            {
                Name = name;
                Description = description;
            }

            public string Name { get; private set; }
            public string Description { private get; set; }
        }

        [Test]
        public void WhenDefaultFailureAssertionIsSet_AndWeHaveASingleConvention_AutomaticallyAsserts()
        {
            typeof(AbjectConformanceFailure)
                .MustConformTo(
                    Convention.PropertiesMustHavePublicGetters());
        }

        [Test]
        public void WhenDefaultFailureAssertionIsSet_AndWeHaveACompositeConvention_AutomaticallyAsserts()
        {
            typeof(AbjectConformanceFailure)
                .MustConformTo(
                    Convention.PropertiesMustHavePublicGetters().And(
                        Convention.PropertiesMustHavePublicSetters().And(
                            Convention.MustHaveADefaultConstructor)));
        }

        [TearDown]
        public void TearDown()
        {
            ConventionConfiguration.DefaultFailureAssertionCallback = null;
        }
    }
}