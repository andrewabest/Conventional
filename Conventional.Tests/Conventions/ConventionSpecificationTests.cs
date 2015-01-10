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

        private class PrefixClass
        {
        }

        [Test]
        public void NameShouldStartWithConventionSpecification_Success()
        {
            typeof (PrefixClass)
                .MustConformTo(Convention.NameShouldStartWith("Prefix"))
                .IsSatisfied
                .Should()
                .BeTrue();
        }
        
        [Test]
        public void NameShouldStartWithConventionSpecification_FailsIfNameDoesNotStartWithSuppliedPrefix()
        {
            var result = typeof (PrefixClass)
                .MustConformTo(Convention.NameShouldStartWith("NotPrefix"));

            result.IsSatisfied.Should().BeFalse();
            result.Failures.Should().HaveCount(1);
        }
        
        private class ClassSuffix
        {
        }

        [Test]
        public void NameShouldEndWithConventionSpecification_Success()
        {
            typeof(ClassSuffix)
                .MustConformTo(Convention.NameShouldEndWith("Suffix"))
                .IsSatisfied
                .Should()
                .BeTrue();
        }
        
        [Test]
        public void NameShouldEndWithConventionSpecification_FailsIfNameDoesNotEndWithSuppliedPrefix()
        {
            var result = typeof(ClassSuffix)
                .MustConformTo(Convention.NameShouldEndWith("NotSuffix"));

            result.IsSatisfied.Should().BeFalse();
            result.Failures.Should().HaveCount(1);
        }

        private class NamespaceMember
        {
        }

        [Test]
        public void ShouldLiveInNamespaceConventionSpecification_Success()
        {
            typeof(NamespaceMember)
                .MustConformTo(Convention.ShouldLiveInNamespace("Conventional.Tests.Conventions"))
                .IsSatisfied
                .Should()
                .BeTrue();
        }

        [Test]
        public void ShouldLiveInNamespaceConventionSpecification_FailsIfTypeDoesNotLiveInTheGivenNamespace()
        {
            var result = typeof (ClassSuffix)
                .MustConformTo(Convention.ShouldLiveInNamespace("Another.Namespace"));

            result.IsSatisfied.Should().BeFalse();
            result.Failures.Should().HaveCount(1);
        }

        private class HasADefaultConstructor
        {
        }

        [Test]
        public void ShouldHaveADefaultConstructorConventionSpecification_Success()
        {
            typeof(HasADefaultConstructor)
                .MustConformTo(Convention.ShouldHaveADefaultConstructor)
                .IsSatisfied
                .Should()
                .BeTrue();
        }

        private class DoesNotHaveADefaultConstructor
        {
            public DoesNotHaveADefaultConstructor(string name)
            {
            }
        }

        [Test]
        public void ShouldHaveADefaultConstructorConventionSpecification_FailsWhenNoDefaultConstructorExists()
        {
            var result = typeof(DoesNotHaveADefaultConstructor)
                .MustConformTo(Convention.ShouldHaveADefaultConstructor);

            result.IsSatisfied.Should().BeFalse();
            result.Failures.Should().HaveCount(1);
        }

        private class Dependency
        {
        }

        private class HasNoIllegalDependencies
        {
            public HasNoIllegalDependencies(string name)
            {
            }
        }

        [Test]
        public void ShouldNotTakeADependencyOnConventionSpecification_Success()
        {
            typeof(HasNoIllegalDependencies)
                .MustConformTo(Convention.ShouldNotTakeADependencyOn(typeof(Dependency)))
                .IsSatisfied
                .Should()
                .BeTrue();
        }

        private class HasIllegalDependencies
        {
            public HasIllegalDependencies(Dependency dependency)
            {
            }
        }
        
        [Test]
        public void ShouldNotTakeADependencyOnConventionSpecification_FailsIfTheIdentifiedConstructorParameterExists()
        {
            var result = typeof (HasIllegalDependencies)
                .MustConformTo(Convention.ShouldNotTakeADependencyOn(typeof (Dependency)));

            result.IsSatisfied.Should().BeFalse();
            result.Failures.Should().HaveCount(1);
        }
    }
}
