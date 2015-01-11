using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace Conventional.Tests
{
    public class ConformistScenarios
    {
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
        public void CompositeSpecification_OutputsExpectedFailuresInCorrectOrder()
        {
            var results = typeof (AbjectConformanceFailure)
                .MustConformTo(
                    Convention.PropertiesMustHavePublicGetters.And(
                    Convention.PropertiesMustHavePublicSetters.And(
                    Convention.MustHaveADefaultConstructor)));

            results.Failures.Should().HaveCount(3);
            results.Failures[0].Should().StartWith("All properties should have public getters");
            results.Failures[1].Should().StartWith("All properties should have public setters");
            results.Failures[2].Should().StartWith("Does not have a default constructor");
        }

        [Test]
        public void FluentSyntax_OutputsExpectedFailuresInCorrectOrder()
        {
            var results = new [] { typeof(AbjectConformanceFailure) }
                .MustConformTo(Convention.PropertiesMustHavePublicGetters)
                .AndMustConformTo(Convention.PropertiesMustHavePublicSetters)
                .AndMustConformTo(Convention.MustHaveADefaultConstructor);

            results.Failures.Should().HaveCount(3);
            results.Failures[0].Should().StartWith("All properties should have public getters");
            results.Failures[1].Should().StartWith("All properties should have public setters");
            results.Failures[2].Should().StartWith("Does not have a default constructor");
        }

        [Test]
        public void Combination_CompositeSpecificationAndFluentSyntax_OutputsExpectedFailuresInCorrectOrder()
        {
            var results = new [] { typeof(AbjectConformanceFailure) }
                .MustConformTo(
                    Convention.PropertiesMustHavePublicGetters.And(
                    Convention.PropertiesMustHavePublicSetters))
                .AndMustConformTo(Convention.MustHaveADefaultConstructor);

            results.Failures.Should().HaveCount(3);
            results.Failures[0].Should().StartWith("All properties should have public getters");
            results.Failures[1].Should().StartWith("All properties should have public setters");
            results.Failures[2].Should().StartWith("Does not have a default constructor");
        }
    }
}