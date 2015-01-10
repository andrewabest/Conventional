using FluentAssertions;
using NUnit.Framework;

namespace Conventional.Tests.Conventions
{
    public class ConventionSpecificationTests
    {
        private class AllPublicMock
        {
            public string Public { get; set; }
        }

        [Test]
        public void PropertiesShouldHavePublicGetters_Success()
        {
            typeof(AllPublicMock)
                .MustConformTo(Convention.PropertiesShouldHavePublicGetters)
                .IsSatisfied
                .Should()
                .BeTrue();
        }

        private class PrivateGetterMock
        {
            public string PrivateGet { private get; set; }
        }

        [Test]
        public void PropertiesShouldHavePublicGetters_FailsWhenPrivateGetterExists()
        {
            var result = typeof (PrivateGetterMock)
                .MustConformTo(Convention.PropertiesShouldHavePublicGetters);

            result.IsSatisfied.Should().BeFalse();
            result.Failures.Should().HaveCount(1);
        }
    }
}
