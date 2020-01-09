using System;
using FluentAssertions;
using NUnit.Framework;

namespace Conventional.Tests
{
    public class DoomsdayScenarios
    {
        private string _failure;
        private string _warning;
        private readonly DateTime _now = new DateTime(2015,11,18);

        [SetUp]
        public void Setup()
        {
            ConventionConfiguration.DefaultFailureAssertionCallback = x => _failure = x;
            ConventionConfiguration.DefaultWarningAssertionCallback = x => _warning = x;
            ConventionConfiguration.DefaultCurrentDateResolver = () => _now;
        }

        [TearDown]
        public void Teardown()
        {
            _failure = null;
            _warning = null;
        }

        private class OffenderOne
        {
        }

        private class OffenderTwo
        {
        }

        [Test]
        public void WhenUsingKnownOffenders_AndDefaultFailureAssertionIsNotSet_ThrowsException()
        {
            ConventionConfiguration.DefaultFailureAssertionCallback = null;

            Action action = () => new[] { typeof(OffenderOne), typeof(OffenderTwo) }
                .WithKnownOffenders(1)
                .MustConformTo(Convention.NameMustEndWith("Esquire"));

            action.ShouldThrow<Exception>();
        }

        [Test]
        public void WhenUsingKnownOffenders_AndDefaultWarningAssertionIsNotSet_ThrowsException()
        {
            ConventionConfiguration.DefaultWarningAssertionCallback = null;

            Action action = () => new[] { typeof(OffenderOne), typeof(OffenderTwo) }
                .WithKnownOffenders(1)
                .MustConformTo(Convention.NameMustEndWith("Esquire"));

            action.ShouldThrow<Exception>();
        }

        [Test]
        public void WhenUsingDoomsday_AndDefaultFailureAssertionIsNotSet_ThrowsException()
        {
            ConventionConfiguration.DefaultFailureAssertionCallback = null;

            Action action = () => new[] { typeof(OffenderOne), typeof(OffenderTwo) }
                .WithKnownOffenders(1)
                .MustConformTo(Convention.NameMustEndWith("Esquire"));

            action.ShouldThrow<Exception>();
        }

        [Test]
        public void WhenUsingDoomsday_AndDefaultWarningAssertionIsNotSet_ThrowsException()
        {
            ConventionConfiguration.DefaultWarningAssertionCallback = null;

            Action action = () => new[] { typeof(OffenderOne), typeof(OffenderTwo) }
                .WithKnownOffenders(1)
                .MustConformTo(Convention.NameMustEndWith("Esquire"));

            action.ShouldThrow<Exception>();
        }

        [Test]
        public void WhenNumberOfOffendersExceedsKnownOffenders_AssertsFailure()
        {
            new [] { typeof(OffenderOne), typeof(OffenderTwo) }
                .WithKnownOffenders(1)
                .MustConformTo(Convention.NameMustEndWith("Esquire"));

            _failure.Should().Be("Expected 1 or less offenders but found 2: \r\nConventional.Tests.DoomsdayScenarios+OffenderOne\r\n---------------------------------------------------\r\nType name does not end with Esquire\r\n\r\nConventional.Tests.DoomsdayScenarios+OffenderTwo\r\n---------------------------------------------------\r\nType name does not end with Esquire\r\n\r\n");
        }

        [Test]
        public void WhenNumberOfOffendersEqualsKnownOffenders_Succeeds()
        {
            new[] { typeof(OffenderOne), typeof(OffenderTwo) }
                .WithKnownOffenders(1)
                .MustConformTo(Convention.NameMustEndWith("One"));

            _failure.Should().BeNull();
        }

        [Test]
        public void WhenNumberOfOffendersIsLessThanKnownOffenders_Succeeds()
        {
            new[] { typeof(OffenderOne), typeof(OffenderTwo) }
                .WithKnownOffenders(1)
                .MustConformTo(Convention.NameMustStartWith("Offender"));

            _failure.Should().BeNull();
        }

        [Test]
        public void WhenDoomsdayIsSupplied_FailsIfThereAreAnyOffendersAfterDoomsday()
        {
            var doomsday = new DateTime(2015, 11, 16);

            new[] { typeof(OffenderOne), typeof(OffenderTwo) }
                .ByDoomsday(doomsday)
                .MustConformTo(Convention.NameMustEndWith("One"));

            _failure.Should().Be("Doomsday is upon us! \r\nConventional.Tests.DoomsdayScenarios+OffenderTwo\r\n---------------------------------------------------\r\nType name does not end with One\r\n\r\n");
        }

        [Test]
        public void WhenDoomsdayIsSupplied_AndANumberOfKnownOffendersIsSupplied_FailsWithDoomsdayMessageRegardlessOfNumberOfOffenders()
        {
            var doomsday = new DateTime(2015, 11, 16);

            new[] { typeof(OffenderOne), typeof(OffenderTwo) }
                .WithKnownOffenders(1)
                .ByDoomsday(doomsday)
                .MustConformTo(Convention.NameMustEndWith("Esquire"));

            _failure.Should().Be("Doomsday is upon us! \r\nConventional.Tests.DoomsdayScenarios+OffenderOne\r\n---------------------------------------------------\r\nType name does not end with Esquire\r\n\r\nConventional.Tests.DoomsdayScenarios+OffenderTwo\r\n---------------------------------------------------\r\nType name does not end with Esquire\r\n\r\n");
        }

        [Test]
        public void WhenDoomsdayIsSupplied_SucceedsIfThereAreNoOffendersAfterDoomsday()
        {
            var doomsday = new DateTime(2015, 11, 16);

            new[] { typeof(OffenderOne), typeof(OffenderTwo) }
                .ByDoomsday(doomsday)
                .MustConformTo(Convention.NameMustStartWith("Offender"));

            _failure.Should().BeNull();
        }

        [Test]
        public void WhenWarnWithinIsSupplied_ButDoomsdayIsNot_ThrowsException()
        {
            Action action = () => new[] { typeof(OffenderOne), typeof(OffenderTwo) }
                .WithKnownOffenders(1)
                .WithWarningWithin(TimeSpan.FromDays(3))
                .MustConformTo(Convention.NameMustEndWith("Esquire"));

            action.ShouldThrow<Exception>();
        }

        [Test]
        public void WhenWarnWithinIsSupplied_WarnsWhenWithinTheTimespanOfDoomsdayAndOffendersStillExist()
        {
            var doomsday = new DateTime(2015, 11, 20);

            new[] { typeof(OffenderOne), typeof(OffenderTwo) }
                .WithKnownOffenders(1)
                .ByDoomsday(doomsday)
                .WithWarningWithin(TimeSpan.FromDays(3))
                .MustConformTo(Convention.NameMustEndWith("Two"));

            _warning.Should().Be("Doomsday approaches! \r\nConventional.Tests.DoomsdayScenarios+OffenderOne\r\n---------------------------------------------------\r\nType name does not end with Two\r\n\r\n");
        }

        [Test]
        public void WhenWarningWithinIsSupplied_DoesNotWarnWhenOutsideTheTimespanOfDoomsday()
        {
            var doomsday = new DateTime(2015, 11, 25);

            new[] { typeof(OffenderOne), typeof(OffenderTwo) }
                .WithKnownOffenders(1)
                .ByDoomsday(doomsday)
                .WithWarningWithin(TimeSpan.FromDays(3))
                .MustConformTo(Convention.NameMustEndWith("One"));

            _warning.Should().BeNull();
        }

        [Test]
        public void WhenWarnWithinIsSupplied_AndWhenWithinTheTimespanOfDoomsday_FailsIfOffendersHaveIncreased()
        {
            var doomsday = new DateTime(2015, 11, 20);

            new[] { typeof(OffenderOne), typeof(OffenderTwo) }
                .WithKnownOffenders(1)
                .ByDoomsday(doomsday)
                .WithWarningWithin(TimeSpan.FromDays(3))
                .MustConformTo(Convention.NameMustEndWith("Esquire"));

            _failure.Should().Be("Expected 1 or less offenders but found 2: \r\nConventional.Tests.DoomsdayScenarios+OffenderOne\r\n---------------------------------------------------\r\nType name does not end with Esquire\r\n\r\nConventional.Tests.DoomsdayScenarios+OffenderTwo\r\n---------------------------------------------------\r\nType name does not end with Esquire\r\n\r\n");
        }

        [Test]
        public void WhenMessageIsSupplied_OutputsTheMessgeOnFailures()
        {
            var doomsday = new DateTime(2015, 11, 20);

            new[] { typeof(OffenderOne), typeof(OffenderTwo) }
                .ByDoomsday(doomsday)
                .WithWarningWithin(TimeSpan.FromDays(3))
                .WithMessage("Things should really end with Esquire, so they sound fancier.")
                .MustConformTo(Convention.NameMustEndWith("Esquire"));

            _warning.Should().Be("Doomsday approaches! Things should really end with Esquire, so they sound fancier.\r\nConventional.Tests.DoomsdayScenarios+OffenderOne\r\n---------------------------------------------------\r\nType name does not end with Esquire\r\n\r\nConventional.Tests.DoomsdayScenarios+OffenderTwo\r\n---------------------------------------------------\r\nType name does not end with Esquire\r\n\r\n");
        }
    }
}