using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Conventional.Conventions;
using Conventional.Tests.Conventional.Conventions.TestData;
using FluentAssertions;
using NUnit.Framework;

namespace Conventional.Tests.Conventional.Conventions
{
    public class ConventionSpecificationTests
    {
        private class PublicPropertyMock
        {
            public string Public { get; set; }
            public string AnotherPublic { get; private set; }
        }

        [Test]
        public void PropertiesMustBePublic_Success()
        {
            typeof(PublicPropertyMock)
                .MustConformTo(Convention.PropertiesMustBePublic)
                .IsSatisfied
                .Should()
                .BeTrue();
        }

        private class PrivatePropertyMock
        {
            string Private { get; set; }
            public string Public { get; set; }
        }

        [Test]
        public void PropertiesMustBePublic_FailsWhenPrivatePropertyExists()
        {
            var result = typeof(PrivatePropertyMock)
                .MustConformTo(Convention.PropertiesMustBePublic);

            result.IsSatisfied.Should().BeFalse();
            result.Failures.Should().HaveCount(1);
        }

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

        private class AllPublicGetterMockBase
        {
            public string Private { private get; set; }
        }

        private class AllPublicGetterMockDerived : AllPublicGetterMockBase
        {
            public string Public { get; set; }
        }

        [Test]
        public void PropertiesMustHavePublicGetters_IgnoresInheritedProperties()
        {
            typeof(AllPublicGetterMockDerived)
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

        private class AllPublicSetterMockBase
        {
            public string Private { get; private set; }
        }

        private class AllPublicSetterMockDerived : AllPublicSetterMockBase
        {
            public string Public { get; set; }
        }

        [Test]
        public void PropertiesMustHavePublicSetters_IgnoresInheritedProperties()
        {
            typeof(AllPublicSetterMockDerived)
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

        private class AllNoSetterMock
        {
            // ReSharper disable once UnusedMember.Local
            public string PrivateSet { get; }
        }

        [Test]
        public void PropertiesMustHavePublicSetters_FailsWhenNoSetterExists()
        {
            var result = typeof (AllNoSetterMock)
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

        private class AllProtectedSetterMockBase
        {
            public string Private { get; private set; }
        }

        private class AllProtectedSetterMockDerived : AllProtectedSetterMockBase
        {
            public string Public { get; protected set; }
        }

        [Test]
        public void PropertiesMustHaveProtectedSetters_IgnoresInheritedProperties()
        {
            typeof(AllProtectedSetterMockDerived)
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
        public void PropertiesMustHaveProtectedSetters_FailsWhenNoSetterExists()
        {
            var result = typeof(AllNoSetterMock)
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

        private class AllPrivateSetterMockBase
        {
            public string Public { get; set; }
        }

        private class AllPrivateSetterMockDerived : AllPrivateSetterMockBase
        {
            public string Private { get; private set; }
        }

        [Test]
        public void PropertiesMustHavePrivateSetters_IgnoresInheritedProperties()
        {
            typeof(AllPrivateSetterMockDerived)
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

        [Test]
        public void PropertiesMustHavePrivateSetters_FailsWhenNoSetterExists()
        {
            var result = typeof(AllNoSetterMock)
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

        [Test]
        public void NamespaceMustStartWithConventionSpecification_Success()
        {
            typeof(NamespaceMember)
                .MustConformTo(Convention.NamespaceMustStartWith("Conventional.Tests.Conven"))
                .IsSatisfied
                .Should()
                .BeTrue();
        }

        [Test]
        public void NamespaceMustStartWithConventionSpecification_FailsIfTypeDoesNotLiveInANamespaceStartingWithPrefix()
        {
            var result = typeof (NamespaceMember)
                .MustConformTo(Convention.NamespaceMustStartWith("Conventional.Potato"));

            result.IsSatisfied.Should().BeFalse();
            result.Failures.Should().HaveCount(1);
        }

        [Test]
        public void NamespaceMustEndWithConventionSpecification_Success()
        {
            typeof(NamespaceMember)
                .MustConformTo(Convention.NamespaceMustEndWith(".Conventional.Conventions"))
                .IsSatisfied
                .Should()
                .BeTrue();
        }

        [Test]
        public void NamespaceMustEndWithConventionSpecification_FailsIfTypeDoesNotLiveInANamespaceEndingWithSuffix()
        {
            var result = typeof (NamespaceMember)
                .MustConformTo(Convention.NamespaceMustEndWith(".Conventional.Potato"));

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

        private class MockWithoutPropertyAttributeBase
        {
            public string Description { get; set; }
        }

        private class MockWithPropertyAttributeDerived : MockWithoutPropertyAttributeBase
        {
            [MaxLength(255)]
            public string Name { get; set; }
        }

        private class MockWithPropertyAttribute
        {
            [MaxLength(255)]
            public string Name { get; set; }
        }

        private class MockWithGetOnlyPropertyAttribute
        {
            [MaxLength(255)]
            public string Name { get; private set; }
        }

        private class MockWithoutPropertyAttribute
        {
            public string Name { get; set; }
        }

        [Test]
        public void PropertiesOfTypeMustHaveAttributeConventionSpecification_Success()
        {
            typeof(MockWithPropertyAttribute)
                .MustConformTo(Convention.PropertiesOfTypeMustHaveAttribute(typeof(string), typeof(MaxLengthAttribute)))
                .IsSatisfied
                .Should()
                .BeTrue();
        }

        [Test]
        public void PropertiesOfTypeMustHaveAttributeConventionSpecification_IgnoresInheritedProperties()
        {
            typeof(MockWithPropertyAttributeDerived)
                .MustConformTo(Convention.PropertiesOfTypeMustHaveAttribute(typeof(string), typeof(MaxLengthAttribute)))
                .IsSatisfied
                .Should()
                .BeTrue();
        }

        [Test]
        public void PropertiesOfTypeMustHaveAttributeConventionSpecification_WriteOnlySuccess()
        {
            typeof(MockWithGetOnlyPropertyAttribute)
                .MustConformTo(Convention.PropertiesOfTypeMustHaveAttribute(typeof(string), typeof(MaxLengthAttribute), false))
                .IsSatisfied
                .Should()
                .BeTrue();
        }

        [Test]
        public void PropertiesOfTypeMustHaveAttributeConventionSpecification_Fails()
        {
            typeof(MockWithoutPropertyAttribute)
                .MustConformTo(Convention.PropertiesOfTypeMustHaveAttribute(typeof(string), typeof(MockAttribute)))
                .IsSatisfied
                .Should()
                .BeFalse();
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
                 .MustConformTo(Convention.EnumerablePropertiesMustBeEagerLoaded)
                 .IsSatisfied
                 .Should()
                 .BeTrue();
        }

        private class HasLazilyLoadedEnumerablesBase
        {
            public IEnumerable<string> Descriptions { get; set; }
        }

        private class HasEagerLoadedEnumerablesDerived : HasLazilyLoadedEnumerablesBase
        {
            public string[] Names { get; set; }
        }

        [Test]
        public void EnumerablePropertiesMustBeEagerLoadedConventionSpecification_IgnoresInheritedProperties()
        {
            typeof(HasEagerLoadedEnumerablesDerived)
                .MustConformTo(Convention.EnumerablePropertiesMustBeEagerLoaded)
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
                .MustConformTo(Convention.EnumerablePropertiesMustBeEagerLoaded);

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

            public string[] Names => _names;
        }

        [Test]
        public void CollectionPropertiesMustNotHaveSetters_Success()
        {
            typeof(HasImmutableCollections)
                 .MustConformTo(Convention.CollectionPropertiesMustNotHaveSetters)
                 .IsSatisfied
                 .Should()
                 .BeTrue();
        }

        private class HasMutableCollectionsBase
        {
            public string[] Colours { get; set; }
        }

        private class HasImmutableCollectionsDerived : HasMutableCollectionsBase
        {
            public HasImmutableCollectionsDerived(string[] names)
            {
                Names = names;
            }

            public string[] Names { get; }
        }

        [Test]
        public void CollectionPropertiesMustNotHaveSetters_IgnoresInheritedProperties()
        {
            typeof(HasImmutableCollectionsDerived)
                .MustConformTo(Convention.CollectionPropertiesMustNotHaveSetters)
                .IsSatisfied
                .Should()
                .BeTrue();
        }

        private class HasMutableCollections
        {
            public string[] Names { get; set; }
        }

        [Test]
        public void CollectionPropertiesMustNotHaveSetters_FailsWhenAMutableCollectionPropertyExists()
        {
            var result = typeof(HasMutableCollections)
                .MustConformTo(Convention.CollectionPropertiesMustNotHaveSetters);

            result.IsSatisfied.Should().BeFalse();
            result.Failures.Should().HaveCount(1);
        }

        private class HasImmutableProperties
        {
            public HasImmutableProperties(string[] names, int age)
            {
                Names = names;
                Age = age;
            }

            public string[] Names { get; }

            public int Age { get; }
        }

        [Test]
        public void PropertiesMustNotHaveSetters_Success()
        {
            typeof(HasImmutableProperties)
                 .MustConformTo(Convention.PropertiesMustNotHaveSetters)
                 .IsSatisfied
                 .Should()
                 .BeTrue();
        }

        private class HasMutablePropertiesBase
        {
            public int Age { get; set; }
        }

        private class HasImmutablePropertiesDerived : HasMutablePropertiesBase
        {
            public HasImmutablePropertiesDerived(string[] names, int age)
            {
                Names = names;
                Age = age;
            }

            public string[] Names { get; }

            public new int Age { get; }
        }

        [Test]
        public void PropertiesMustNotHaveSetters_IgnoresInheritedProperties()
        {
            typeof(HasImmutablePropertiesDerived)
                .MustConformTo(Convention.PropertiesMustNotHaveSetters)
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
        public void PropertiesMustNotHaveSetters_FailsWhenMutablePropertiesExists()
        {
            var result = typeof(HasMutableProperties)
                .MustConformTo(Convention.PropertiesMustNotHaveSetters);

            result.IsSatisfied.Should().BeFalse();
            result.Failures.Should().HaveCount(1);
        }

        private class DoesNotHaveEnumerableProperty
        {
            public long Ticks { get; set; }
        }

        [Test]
        public void MustNotHaveAPropertyOfType_Success()
        {
            var result = typeof(DoesNotHaveEnumerableProperty)
                .MustConformTo(Convention.MustNotHaveAPropertyOfType(typeof(IEnumerable<>), "reason"))
                .IsSatisfied
                .Should()
                .BeTrue();
        }

        private class HasEnumerablePropertyBase
        {
            public IEnumerable<int> Numbers { get; set; }
        }

        private class DoesNotHaveEnumerablePropertyDerived : HasEnumerablePropertyBase
        {
            public long Ticks { get; set; }
        }

        [Test]
        public void MustNotHaveAPropertyOfType_IgnoresInheritedProperties()
        {
            var result = typeof(DoesNotHaveEnumerablePropertyDerived)
                .MustConformTo(Convention.MustNotHaveAPropertyOfType(typeof(IEnumerable<>), "reason"))
                .IsSatisfied
                .Should()
                .BeTrue();
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
                .MustConformTo(Convention.MustHaveMatchingEmbeddedResources("*.testdata"));

            result.IsSatisfied.Should().BeTrue();
        }

        [Test]
        public void MustHaveMatchingEmbeddedResourcesConventionSpecification_Success_WithNonWildcardFileExtension()
        {
            var result = typeof(HasMatchingEmbeddedResource)
                .MustConformTo(Convention.MustHaveMatchingEmbeddedResources("testdata"));

            result.IsSatisfied.Should().BeTrue();
        }

        [Test]
        public void MustHaveMatchingEmbeddedResourcesConventionSpecification_Success_WithResourceNameMatcher()
        {
            var result = typeof(HasMatchingEmbeddedResource)
                .MustConformTo(Convention.MustHaveMatchingEmbeddedResources(t =>
                    t.FullName + ".testdata"));

            result.IsSatisfied.Should().BeTrue();
        }

        [Test]
        public void MustHaveMatchingEmbeddedResourcesConventionSpecification_FailsWhenFileNotEmbeddedResource_FileExtension()
        {
            var result = typeof (HasMatchingNonEmbeddedResource)
                .MustConformTo(Convention.MustHaveMatchingEmbeddedResources("testdata"));

            result.IsSatisfied.Should().BeFalse();
            result.Failures.Count().Should().Be(1);
        }

        [Test]
        public void MustHaveMatchingEmbeddedResourcesConventionSpecification_FailsWhenFileNotEmbeddedResource_ResourceNameMatcher()
        {
            var result = typeof (HasMatchingNonEmbeddedResource)
                .MustConformTo(Convention.MustHaveMatchingEmbeddedResources(t =>
                    t.FullName + ".testdata"));

            result.IsSatisfied.Should().BeFalse();
            result.Failures.Count().Should().Be(1);
        }

        [Test]
        public void MustHaveMatchingEmbeddedResourcesConventionSpecification_FailsWhenFileDoesntExist_ResourceNameMatcher()
        {
            var result = typeof(HasNoMatchingFile)
                .MustConformTo(Convention.MustHaveMatchingEmbeddedResources(t =>
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

        private class HasANonAsyncVoidMethod
        {
            public void NonAsyncVoidMethod()
            {
            }
        }

        [Test]
        public void VoidMethodsMustNotBeAsync_Success()
        {
            typeof(HasANonAsyncVoidMethod)
                .MustConformTo(Convention.VoidMethodsMustNotBeAsync)
                .IsSatisfied
                .Should()
                .BeTrue();
        }

        private class HasAnAsyncVoidMethod
        {
            // Note: disable "This async method lacks 'await' operators and will run synchronously." warning
#pragma warning disable 1998
            public async void AsyncVoidMethod()
            {
            }
#pragma warning restore 1998

        }

        [Test]
        public void VoidMethodsMustNotBeAsync_FailsWhenAsyncVoidMethodExists()
        {
            var result = typeof(HasAnAsyncVoidMethod)
                .MustConformTo(Convention.VoidMethodsMustNotBeAsync);

            result.IsSatisfied.Should().BeFalse();
            result.Failures.Should().HaveCount(1);
        }

        private class HasAnAsyncMethodWithAsyncSuffix
        {
            // Note: disable "This async method lacks 'await' operators and will run synchronously." warning
#pragma warning disable 1998
            public async void AsyncMethodWithSuffixOfAsync()
            {
            }
#pragma warning restore 1998
        }

        [Test]
        public void AsyncMethodsMustHaveAsyncSuffix_Success()
        {
            typeof(HasAnAsyncMethodWithAsyncSuffix)
                .MustConformTo(Convention.AsyncMethodsMustHaveAsyncSuffix)
                .IsSatisfied
                .Should()
                .BeTrue();
        }

        private class HasAnAsyncMethodWithoutAnAsyncSuffix
        {
            // Note: disable "This async method lacks 'await' operators and will run synchronously." warning
#pragma warning disable 1998
            public async void AsyncMethodWithoutAsyncSuffix()
            {
            }
#pragma warning restore 1998
        }

        [Test]
        public void AsyncMethodsMustHaveAsyncSuffix_FailsWhenMethodIsNotSuffixedWithAsync()
        {
            var result = typeof(HasAnAsyncMethodWithoutAnAsyncSuffix)
                .MustConformTo(Convention.AsyncMethodsMustHaveAsyncSuffix);

            result.IsSatisfied.Should().BeFalse();
            result.Failures.Should().HaveCount(1);
        }
    }
}