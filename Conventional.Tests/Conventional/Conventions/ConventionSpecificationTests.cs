﻿using System;
using System.Collections.Generic;
using System.Linq;
using Conventional.Conventions;
using Conventional.Tests.Conventional.Conventions.TestData;
using FluentAssertions;
using NUnit.Framework;

namespace Conventional.Tests.Conventional.Conventions
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

        private class AllPrivateSetterMock
        {
            public string PrivateSet { get; private set; }
        }

        [Test]
        public void PropertiesMustHavePublicSetters_FailsWhenPrivateSetterExists()
        {
            var result = typeof (AllPrivateSetterMock)
                .MustConformTo(Convention.PropertiesMustHavePublicSetters);

            result.IsSatisfied.Should().BeFalse();
            result.Failures.Should().HaveCount(1);
        }

        private class AllProtectedSetterMock
        {
            public string Public { get; protected set; }
        }

        [Test]
        public void PropertiesMustHaveProtectedSetters_Success()
        {
            typeof(AllProtectedSetterMock)
                .MustConformTo(Convention.PropertiesMustHaveProtectedSetters)
                .IsSatisfied
                .Should()
                .BeTrue();
        }

        [Test]
        public void PropertiesMustHaveProtectedSetters_FailsWhenOtherSetterExists()
        {
            var result = typeof(AllPrivateSetterMock)
                .MustConformTo(Convention.PropertiesMustHaveProtectedSetters);

            result.IsSatisfied.Should().BeFalse();
            result.Failures.Should().HaveCount(1);
        }

        [Test]
        public void PropertiesMustHavePrivateSetters_Success()
        {
            typeof(AllPrivateSetterMock)
                .MustConformTo(Convention.PropertiesMustHavePrivateSetters)
                .IsSatisfied
                .Should()
                .BeTrue();
        }

        [Test]
        public void PropertiesMustHavePrivateSetters_FailsWhenOtherSetterExists()
        {
            var result = typeof(AllProtectedSetterMock)
                .MustConformTo(Convention.PropertiesMustHavePrivateSetters);

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

        private interface IFakeBusCommand
        {
        }

        private interface IFakeHandleCommand<T> where T : IFakeBusCommand
        {
        }

        [Test]
        public void NameMustEndWithConventionSpecification_HandlesGenericCase()
        {
            var result = typeof(IFakeHandleCommand<>)
                .MustConformTo(Convention.NameMustEndWith("HandleCommand"));

            result.IsSatisfied.Should().BeTrue();
            result.Failures.Should().HaveCount(0);
        }

        private class NamespaceMember
        {
        }

        [Test]
        public void MustLiveInNamespaceConventionSpecification_Success()
        {
            typeof(NamespaceMember)
                .MustConformTo(Convention.MustLiveInNamespace("Conventional.Tests.Conventional.Conventions"))
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

        private class HasADefaultNonPublicConstructor
        {
            protected HasADefaultNonPublicConstructor() { }
        }

        [Test]
        public void MustHaveADefaultNonPublicConstructorConventionSpecification_Success()
        {
            typeof(HasADefaultNonPublicConstructor)
                .MustConformTo(Convention.MustHaveANonPublicDefaultConstructor)
                .IsSatisfied
                .Should()
                .BeTrue();
        }

        private class DoesNotHaveADefaultNonPublicConstructor
        {
            public DoesNotHaveADefaultNonPublicConstructor()
            {
            }
        }

        [Test]
        public void MustHaveADefaultNonPublicConstructorConventionSpecification_FailsWhenNoDefaultConstructorExists()
        {
            var result = typeof(DoesNotHaveADefaultNonPublicConstructor)
                .MustConformTo(Convention.MustHaveANonPublicDefaultConstructor);

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
                .MustConformTo(Convention.MustNotTakeADependencyOn(typeof(Dependency), "Because it shouldn't"))
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
                .MustConformTo(Convention.MustNotTakeADependencyOn(typeof (Dependency), "Because it shouldn't"));

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

        private interface ISomeGeneric<T1, T2>
        {
        }

        private class SomeClassThatRequiresSomeGenericInterfaceImplementation
        {
        }

        private class SomeGenericInterfaceImplementation : ISomeGeneric<SomeClassThatRequiresSomeGenericInterfaceImplementation, string>
        {
        }

        [Test]
        public void RequiresACorrespondingImplementationOfConventionSpecification_ImplementsGenericInterface_Success()
        {
            typeof(SomeClassThatRequiresSomeGenericInterfaceImplementation)
                 .MustConformTo(Convention.RequiresACorrespondingImplementationOf(typeof(ISomeGeneric<,>), new[] { typeof(SomeGenericInterfaceImplementation) }))
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

        private class HasEagerLoadedEnumerables
        {
            public string[] Names { get; set; }
        }

        [Test]
        public void EnumerablePropertiesMustBeEagerLoadedConventionSpecification_Success()
        {
            typeof(HasEagerLoadedEnumerables)
                 .MustConformTo(Convention.EnumerablePropertiesMustBeEagerLoadedConventionSpecification)
                 .IsSatisfied
                 .Should()
                 .BeTrue();
        }

        private class HasLazilyLoadedEnumerables
        {
            public IEnumerable<string> Names { get; set; } 
        }

        [Test]
        public void EnumerablePropertiesMustBeEagerLoadedConventionSpecification_FailsWhenTypeHasEnumerablePropertiesThatAreNotEagerlyLoaded()
        {
            var result = typeof(HasLazilyLoadedEnumerables)
                .MustConformTo(Convention.EnumerablePropertiesMustBeEagerLoadedConventionSpecification);

            result.IsSatisfied.Should().BeFalse();
            result.Failures.Should().HaveCount(1);
        }

        private class HasImmutableCollections
        {
            private readonly string[] _names;

            public HasImmutableCollections(string[] names)
            {
                _names = names;
            }

            public string[] Names { get { return _names; } }
        }

        [Test]
        public void CollectionPropertiesMustBeImmutable_Success()
        {
            typeof(HasImmutableCollections)
                 .MustConformTo(Convention.CollectionPropertiesMustBeImmutable)
                 .IsSatisfied
                 .Should()
                 .BeTrue();
        }

        private class HasMutableCollections
        {
            public string[] Names { get; set; }
        }

        [Test]
        public void CollectionPropertiesMustBeImmutable_FailsWhenAMutableCollectionPropertyExists()
        {
            var result = typeof(HasMutableCollections)
                .MustConformTo(Convention.CollectionPropertiesMustBeImmutable);

            result.IsSatisfied.Should().BeFalse();
            result.Failures.Should().HaveCount(1);
        } 
        
        private class HasImmutableProperties
        {
            private readonly string[] _names;
            private readonly int _age;

            public HasImmutableProperties(string[] names, int age)
            {
                _names = names;
                _age = age;
            }

            public string[] Names { get { return _names; } }

            public int Age { get { return _age; } }
        }

        [Test]
        public void AllPropertiesMustBeImmutable_Success()
        {
            typeof(HasImmutableProperties)
                 .MustConformTo(Convention.AllPropertiesMustBeImmutable)
                 .IsSatisfied
                 .Should()
                 .BeTrue();
        }

        private class HasMutableProperties
        {
            public string[] Names { get; set; }

            public int Age { get; set; }
        }

        [Test]
        public void AllPropertiesMustBeImmutable_FailsWhenMutablePropertiesExists()
        {
            var result = typeof(HasMutableProperties)
                .MustConformTo(Convention.AllPropertiesMustBeImmutable);

            result.IsSatisfied.Should().BeFalse();
            result.Failures.Should().HaveCount(1);
        }

        private class HasGenericAndNonGenericProperty
        {
            public IEnumerable<string> Names { get; set; }
            public string[] Nicknames { get; set; }
        }

        [Test]
        public void MustNotHaveAPropertyOfTypeIEnumerable_FailsWhenIEnumerablePropertyExists()
        {
            var result = typeof(HasGenericAndNonGenericProperty)
                .MustConformTo(Convention.MustNotHaveAPropertyOfType(typeof(IEnumerable<>), "reason"));

            result.IsSatisfied.Should().BeFalse();
            result.Failures.Should().HaveCount(1);
        }

        [Test]
        public void MustNotHaveAPropertyOfTypeStringArray_FailsWhenStringArrayPropertyExists()
        {
            var result = typeof(HasGenericAndNonGenericProperty)
                .MustConformTo(Convention.MustNotHaveAPropertyOfType(typeof(string[]), "reason"));

            result.IsSatisfied.Should().BeFalse();
            result.Failures.Should().HaveCount(1);
        }

        [Test]
        public void MustHaveMatchingEmbeddedResourcesConventionSpecification_Success_WithWildcardFileExtension()
        {
            var result = typeof(HasMatchingEmbeddedResource)
                .MustConformTo(Convention.MustHaveMatchingEmbeddedResourcesConventionSpecification("*.testdata"));

            result.IsSatisfied.Should().BeTrue();
        }

        [Test]
        public void MustHaveMatchingEmbeddedResourcesConventionSpecification_Success_WithNonWildcardFileExtension()
        {
            var result = typeof(HasMatchingEmbeddedResource)
                .MustConformTo(Convention.MustHaveMatchingEmbeddedResourcesConventionSpecification("testdata"));

            result.IsSatisfied.Should().BeTrue();
        }

        [Test]
        public void MustHaveMatchingEmbeddedResourcesConventionSpecification_Success_WithResourceNameMatcher()
        {
            var result = typeof(HasMatchingEmbeddedResource)
                .MustConformTo(Convention.MustHaveMatchingEmbeddedResourcesConventionSpecification(t =>
                    t.FullName + ".testdata"));

            result.IsSatisfied.Should().BeTrue();
        }

        [Test]
        public void MustHaveMatchingEmbeddedResourcesConventionSpecification_FailsWhenFileNotEmbeddedResource_FileExtension()
        {
            var result = typeof (HasMatchingNonEmbeddedResource)
                .MustConformTo(Convention.MustHaveMatchingEmbeddedResourcesConventionSpecification("testdata"));

            result.IsSatisfied.Should().BeFalse();
            result.Failures.Count().Should().Be(1);
        }

        [Test]
        public void MustHaveMatchingEmbeddedResourcesConventionSpecification_FailsWhenFileNotEmbeddedResource_ResourceNameMatcher()
        {
            var result = typeof (HasMatchingNonEmbeddedResource)
                .MustConformTo(Convention.MustHaveMatchingEmbeddedResourcesConventionSpecification(t =>
                    t.FullName + ".testdata"));

            result.IsSatisfied.Should().BeFalse();
            result.Failures.Count().Should().Be(1);
        }

        [Test]
        public void MustHaveMatchingEmbeddedResourcesConventionSpecification_FailsWhenFileDoesntExist_ResourceNameMatcher()
        {
            var result = typeof(HasNoMatchingFile)
                .MustConformTo(Convention.MustHaveMatchingEmbeddedResourcesConventionSpecification(t =>
                    t.FullName + ".testdata"));

            result.IsSatisfied.Should().BeFalse();
            result.Failures.Count().Should().Be(1);
        }

        private enum SourceEnum
        {
            Value1 = 1,
            Value2 = 2
        }

        public class SameValueAndNames
        {
            public enum SourceEnum
            {
                Value1 = 1,
                Value2 = 2
            }
        }

        private enum DifferentNamedEnum
        {
            Value1 = 1,
            Value2 = 2
        }

        public class MissingValue
        {
            public enum SourceEnum
            {
                Value1 = 1,
            }
        }

        public class ExtraValue
        {

            public enum SourceEnum
            {
                Value1 = 1,
                Value2 = 2,
                Value3 = 3,
            }
        }

        public class DifferenValue
        {
            public enum SourceEnum
            {
                Value1 = 2,
                Value2 = 1
            }
        }

        public class DifferentName
        {
            public enum SourceEnum
            {
                Value1 = 1,
                Value3 = 2
            }
        }


        [Test]
        public void MustHaveCorrespondingEnumConventionSpecification_SucceedsWhenValuesAndNamesMatch()
        {
            new MustHaveCorrespondingEnumConventionSpecification(typeof(SourceEnum))
                .IsSatisfiedBy(typeof(SameValueAndNames.SourceEnum))
                .IsSatisfied.Should().BeTrue();
        }

        [Test]
        public void MustHaveCorrespondingEnumConventionSpecification_FailsWhenNoMatchingName()
        {
            var result = new MustHaveCorrespondingEnumConventionSpecification(typeof(SourceEnum))
                .IsSatisfiedBy(typeof(DifferentNamedEnum));

            result.IsSatisfied.Should().BeFalse();
            result.Failures.Should().Contain("does not match any of the supplied type names");
        }

        [Test]
        public void MustHaveCorrespondingEnumConventionSpecification_FailsWhenMissingValue()
        {
            var result = new MustHaveCorrespondingEnumConventionSpecification(typeof(SourceEnum))
                .IsSatisfiedBy(typeof(MissingValue.SourceEnum));

            result.IsSatisfied.Should().BeFalse();
            result.Failures.Should().Contain("Value2 (2) does not match any values");
        }

        [Test]
        public void MustHaveCorrespondingEnumConventionSpecification_FailsWhenExtraValue()
        {
            var result = new MustHaveCorrespondingEnumConventionSpecification(typeof(SourceEnum))
                .IsSatisfiedBy(typeof(ExtraValue.SourceEnum));

            result.IsSatisfied.Should().BeFalse();
            result.Failures.Should().Contain("Value3 (3) does not match any values");
        }

        [Test]
        public void MustHaveCorrespondingEnumConventionSpecification_FailsWhenDifferentValue()
        {
            var result = new MustHaveCorrespondingEnumConventionSpecification(typeof(SourceEnum))
                .IsSatisfiedBy(typeof(DifferenValue.SourceEnum));

            result.IsSatisfied.Should().BeFalse();
            result.Failures.Should().Contain("Value1 (2) does not match names with the corresponding value");
            result.Failures.Should().Contain("Value2 (1) does not match names with the corresponding value");
        }

        [Test]
        public void MustHaveCorrespondingEnumConventionSpecification_FailsWhenDifferentName()
        {
            var result = new MustHaveCorrespondingEnumConventionSpecification(typeof(SourceEnum))
                .IsSatisfiedBy(typeof(DifferentName.SourceEnum));

            result.IsSatisfied.Should().BeFalse();
            result.Failures.Should().Contain("Value2 (2) does not match names with the corresponding value");
            result.Failures.Should().Contain("Value3 (2) does not match names with the corresponding value");
        }
    }
}