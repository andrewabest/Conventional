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
        public void PropertiesMustHavePublicGetters_Success()
        {
            typeof(AllPublicGetterMock)
                .MustConformTo(Convention.PropertiesMustHavePublicGetters)
                .IsSatisfied
                .Should()
                .BeTrue();
        }

        private class PrivateGetterMock
        {
            public string PrivateGet { private get; set; }
        }

        [Test]
        public void PropertiesMustHavePublicGetters_FailsWhenPrivateGetterExists()
        {
            var result = typeof (PrivateGetterMock)
                .MustConformTo(Convention.PropertiesMustHavePublicGetters);

            result.IsSatisfied.Should().BeFalse();
            result.Failures.Should().HaveCount(1);
        }
        
        private class AllPublicSetterMock
        {
            public string Public { get; set; }
        }

        [Test]
        public void PropertiesMustHavePublicSetters_Success()
        {
            typeof(AllPublicSetterMock)
                .MustConformTo(Convention.PropertiesMustHavePublicSetters)
                .IsSatisfied
                .Should()
                .BeTrue();
        }

        private class PrivateSetterMock
        {
            public string PrivateSet { get; private set; }
        }

        [Test]
        public void PropertiesMustHavePublicSetters_FailsWhenPrivateSetterExists()
        {
            var result = typeof (PrivateSetterMock)
                .MustConformTo(Convention.PropertiesMustHavePublicSetters);

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
        public void MustHaveAttributeConventionSpecification_Success()
        {
            typeof(MockWithAttribute)
                .MustConformTo(Convention.MustHaveAttribute(typeof(MockAttribute)))
                .IsSatisfied
                .Should()
                .BeTrue();
        }

        private class MockWithoutAttribute
        {
        }

        [Test]
        public void MustHaveAttributeConventionSpecification_FailsIfAttributeDoesNotExist()
        {
            var result = typeof (MockWithoutAttribute)
                .MustConformTo(Convention.MustHaveAttribute(typeof (MockAttribute)));

            result.IsSatisfied.Should().BeFalse();
            result.Failures.Should().HaveCount(1);
        }

        private class PrefixClass
        {
        }

        [Test]
        public void NameMustStartWithConventionSpecification_Success()
        {
            typeof (PrefixClass)
                .MustConformTo(Convention.NameMustStartWith("Prefix"))
                .IsSatisfied
                .Should()
                .BeTrue();
        }
        
        [Test]
        public void NameMustStartWithConventionSpecification_FailsIfNameDoesNotStartWithSuppliedPrefix()
        {
            var result = typeof (PrefixClass)
                .MustConformTo(Convention.NameMustStartWith("NotPrefix"));

            result.IsSatisfied.Should().BeFalse();
            result.Failures.Should().HaveCount(1);
        }
        
        private class ClassSuffix
        {
        }

        [Test]
        public void NameMustEndWithConventionSpecification_Success()
        {
            typeof(ClassSuffix)
                .MustConformTo(Convention.NameMustEndWith("Suffix"))
                .IsSatisfied
                .Should()
                .BeTrue();
        }
        
        [Test]
        public void NameMustEndWithConventionSpecification_FailsIfNameDoesNotEndWithSuppliedPrefix()
        {
            var result = typeof(ClassSuffix)
                .MustConformTo(Convention.NameMustEndWith("NotSuffix"));

            result.IsSatisfied.Should().BeFalse();
            result.Failures.Should().HaveCount(1);
        }

        private class NamespaceMember
        {
        }

        [Test]
        public void MustLiveInNamespaceConventionSpecification_Success()
        {
            typeof(NamespaceMember)
                .MustConformTo(Convention.MustLiveInNamespace("Conventional.Tests.Conventions"))
                .IsSatisfied
                .Should()
                .BeTrue();
        }

        [Test]
        public void MustLiveInNamespaceConventionSpecification_FailsIfTypeDoesNotLiveInTheGivenNamespace()
        {
            var result = typeof (ClassSuffix)
                .MustConformTo(Convention.MustLiveInNamespace("Another.Namespace"));

            result.IsSatisfied.Should().BeFalse();
            result.Failures.Should().HaveCount(1);
        }

        private class HasADefaultConstructor
        {
        }

        [Test]
        public void MustHaveADefaultConstructorConventionSpecification_Success()
        {
            typeof(HasADefaultConstructor)
                .MustConformTo(Convention.MustHaveADefaultConstructor)
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
        public void MustHaveADefaultConstructorConventionSpecification_FailsWhenNoDefaultConstructorExists()
        {
            var result = typeof(DoesNotHaveADefaultConstructor)
                .MustConformTo(Convention.MustHaveADefaultConstructor);

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
        public void MustNotTakeADependencyOnConventionSpecification_Success()
        {
            typeof(HasNoIllegalDependencies)
                .MustConformTo(Convention.MustNotTakeADependencyOn(typeof(Dependency)))
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
        public void MustNotTakeADependencyOnConventionSpecification_FailsIfTheIdentifiedConstructorParameterExists()
        {
            var result = typeof (HasIllegalDependencies)
                .MustConformTo(Convention.MustNotTakeADependencyOn(typeof (Dependency)));

            result.IsSatisfied.Should().BeFalse();
            result.Failures.Should().HaveCount(1);
        }

        private class HasAppropriateConstructors
        {
            protected HasAppropriateConstructors()
            {
            }

            public HasAppropriateConstructors(string name)
            {
            }
        }

        [Test]
        public void MustHaveAppropriateConstructorsConventionSpecification_Success()
        {
            typeof(HasAppropriateConstructors)
                 .MustConformTo(Convention.MustHaveAppropriateConstructors)
                 .IsSatisfied
                 .Should()
                 .BeTrue(); 
        }

        private class DoesNotHaveAppropriateConstructors
        {
            public DoesNotHaveAppropriateConstructors()
            {
            }

            public DoesNotHaveAppropriateConstructors(string name)
            {
            }
        }

        [Test]
        public void MustHaveAppropriateConstructorsConventionSpecification_FailsWhenTypeDoesNotHaveAppropriateConstructors()
        {
            var result = typeof (DoesNotHaveAppropriateConstructors)
                .MustConformTo(Convention.MustHaveAppropriateConstructors);

            result.IsSatisfied.Should().BeFalse();
            result.Failures.Should().HaveCount(1);
        }

        private class SomeGeneric<T1, T2>
        {
        }

        private class SomeClassThatRequiresSomeGenericImplementation
        {
        }

        private class SomeGenericImplementation : SomeGeneric<SomeClassThatRequiresSomeGenericImplementation, string>
        {
        }

        [Test]
        public void RequiresACorrespondingImplementationOfConventionSpecification_Success()
        {
            typeof(SomeClassThatRequiresSomeGenericImplementation)
                 .MustConformTo(Convention.RequiresACorrespondingImplementationOf(typeof(SomeGeneric<,>), new [] { typeof(SomeGenericImplementation) }))
                 .IsSatisfied
                 .Should()
                 .BeTrue(); 
        }

        private class SomeClassWithoutASomeGenericImpelemntation
        {
        }

        [Test]
        public void RequiresACorrespondingImplementationOfConventionSpecification_FailsIfImplementationDoesNotExist()
        {
            var result = typeof (SomeClassWithoutASomeGenericImpelemntation)
                .MustConformTo(Convention.RequiresACorrespondingImplementationOf(typeof (SomeGeneric<,>),
                    new[] {typeof (SomeGenericImplementation)}));

            result.IsSatisfied.Should().BeFalse();
            result.Failures.Should().HaveCount(1);
        }
    }
}
