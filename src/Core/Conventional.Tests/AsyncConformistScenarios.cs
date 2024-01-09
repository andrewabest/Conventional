using System;
using System.Threading.Tasks;
using Conventional.Conventions;
using FluentAssertions;
using NUnit.Framework;

namespace Conventional.Tests
{
    public class AlwaysSuccessfulAsyncConvention : AsyncConventionSpecification
    {
        public override async Task<ConventionResult> IsSatisfiedBy(Type type)
        {
            await Task.Delay(1);

            return ConventionResult.Satisfied(type.FullName);
        }

        protected override string FailureMessage => "I never fail!";
    }

    public class NeverSuccessfulAsyncConvention : AsyncConventionSpecification
    {
        public override async Task<ConventionResult> IsSatisfiedBy(Type type)
        {
            await Task.Delay(1);

            return ConventionResult.NotSatisfied(type.FullName, FailureMessage);
        }

        protected override string FailureMessage => "I failed!";
    }

    public class AlsoNeverSuccessfulAsyncConvention : AsyncConventionSpecification
    {
        public override async Task<ConventionResult> IsSatisfiedBy(Type type)
        {
            await Task.Delay(1);

            return ConventionResult.NotSatisfied(type.FullName, FailureMessage);
        }

        protected override string FailureMessage => "I also failed!";
    }

    public class AsyncConformistScenarios
    {
        [SetUp]
        public void Setup()
        {
            ConventionConfiguration.GlobalTypeFilter = type => type != typeof(YouCantSeeMe);
        }

        [Test]
        public void HappyPath_DoesNotThrowExceptions()
        {
            Action action = async () => await typeof(String).MustConformTo(new AlwaysSuccessfulAsyncConvention());

            action.ShouldNotThrow();
        }

        [Test]
        public async Task FluentSyntax_OutputsExpectedFailuresInCorrectOrder()
        {
            var results = await new[] { typeof(String) }
                .MustConformTo(new NeverSuccessfulAsyncConvention())
                .AndMustConformTo(new AlsoNeverSuccessfulAsyncConvention());

            results.Failures.Should().HaveCount(2);
            results.Failures[0].Should().StartWith("I failed!");
            results.Failures[1].Should().StartWith("I also failed!");
        }

        private class YouCantSeeMe
        {
            public string Name { get; set; }
        }

        private class YouCanSeeMe
        {
            public string Name { get; set; }
        }

        [Test]
        public async Task WhenDefaultTypeFilterIsSet_ItIsAppliedToAllConventions()
        {
            var result = await new[] { typeof(YouCantSeeMe), typeof(YouCanSeeMe) }
                .MustConformTo(new NeverSuccessfulAsyncConvention());

            result.Failures.Should().HaveCount(1);
        }

        [TearDown]
        public void TearDown()
        {
            ConventionConfiguration.ResetGlobalTypeFilter();
        }
    }
}