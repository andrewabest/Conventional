using System;
using FluentAssertions;
using NUnit.Framework;

namespace Conventional.Tests.Conventions
{
    public class ConventionSpecificationTests
    {
        private class AllPublicGetterMock
        {
            public string Public { get; set; }
        }

        [Test]
        public void PropertiesShouldHavePublicGetters_Success()
        {
            typeof(AllPublicGetterMock)
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
        
        private class AllPublicSetterMock
        {
            public string Public { get; set; }
        }

        [Test]
        public void PropertiesShouldHavePublicSetters_Success()
        {
            typeof(AllPublicSetterMock)
                .MustConformTo(Convention.PropertiesShouldHavePublicSetters)
                .IsSatisfied
                .Should()
                .BeTrue();
        }

        private class PrivateSetterMock
        {
            public string PrivateSet { get; private set; }
        }

        [Test]
        public void PropertiesShouldHavePublicSetters_FailsWhenPrivateSetterExists()
        {
            var result = typeof (PrivateSetterMock)
                .MustConformTo(Convention.PropertiesShouldHavePublicSetters);

            result.IsSatisfied.Should().BeFalse();
            result.Failures.Should().HaveCount(1);
        }

        private class MockAttribute : Attribute
        {
        }

        [Mock]
        private class MockWithAttribute
        {
        }

        [Test]
        public void ShouldHaveAttributeConventionSpecification_Success()
        {
            typeof(MockWithAttribute)
                .MustConformTo(Convention.ShouldHaveAttribute(typeof(MockAttribute)))
                .IsSatisfied
                .Should()
                .BeTrue();
        }

        private class MockWithoutAttribute
        {
        }

        [Test]
        public void ShouldHaveAttributeConventionSpecification_FailsIfAttributeDoesNotExist()
        {
            var result = typeof (MockWithoutAttribute)
                .MustConformTo(Convention.ShouldHaveAttribute(typeof (MockAttribute)));

            result.IsSatisfied.Should().BeFalse();
            result.Failures.Should().HaveCount(1);
        }


    }
}
