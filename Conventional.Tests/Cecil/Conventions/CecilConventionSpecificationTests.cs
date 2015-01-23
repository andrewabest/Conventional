using System;
using System.Collections.Generic;
using Conventional.Cecil;
using FluentAssertions;
using NUnit.Framework;

namespace Conventional.Tests.Cecil.Conventions
{
    public class CecilConventionSpecificationTests
    {
        private interface IClock
        {
            DateTime GetCurrent();
        }

        private class GoodDateTimeCitizen
        {
            private DateTime _current;

            public GoodDateTimeCitizen(IClock clock)
            {
                _current = clock.GetCurrent();
            }
        }

        [Test]
        public void MustNotUseDateTimeNowConventionSpecification_Success()
        {
            typeof(GoodDateTimeCitizen)
                .MustConformTo(CecilConvention.MustNotUseDateTimeNow)
                .IsSatisfied
                .Should()
                .BeTrue();
        }

        private class OffendingDateTimeCitizen
        {
             private DateTime _current;

             public OffendingDateTimeCitizen()
            {
                _current = DateTime.Now;
            }
        }

        [Test]
        public void MustNotUseDateTimeNowConventionSpecification_FailsWhenACallToDateTimeNowExists()
        {
            var result = typeof (OffendingDateTimeCitizen)
                .MustConformTo(CecilConvention.MustNotUseDateTimeNow);

            result.IsSatisfied.Should().BeFalse();
            result.Failures.Should().HaveCount(1);
        } 
        
        private class GoodDateTimeOffsetCitizen
        {
            private DateTimeOffset _current;

            public GoodDateTimeOffsetCitizen(IClock clock)
            {
                _current = clock.GetCurrent();
            }
        }

        [Test]
        public void MustNotUseDateTimeOffsetNowConventionSpecification_Success()
        {
            typeof(GoodDateTimeOffsetCitizen)
                .MustConformTo(CecilConvention.MustNotUseDateTimeOffsetNow)
                .IsSatisfied
                .Should()
                .BeTrue();
        }

        private class OffendingDateTimeOffsetCitizen
        {
             private DateTimeOffset _current;

             public OffendingDateTimeOffsetCitizen()
            {
                _current = DateTimeOffset.Now;
            }
        }
        
        private class AnotherOffendingDateTimeOffsetCitizen
        {
             private DateTimeOffset _current;

             public AnotherOffendingDateTimeOffsetCitizen()
            {
                _current = DateTimeOffset.UtcNow;
            }
        }

        [Test]
        public void MustNotUseDateTimeOffsetNowConventionSpecification_FailsWhenACallToDateTimeOffsetNowOrDateTimeOffsetUtcNowExists()
        {
            var result = new [] {  typeof (OffendingDateTimeOffsetCitizen), typeof(AnotherOffendingDateTimeOffsetCitizen) }
                .MustConformTo(CecilConvention.MustNotUseDateTimeOffsetNow);

            result.Results.Should().OnlyContain(x => x.IsSatisfied == false);
            result.Failures.Should().HaveCount(2);
        }

        private class SpecificException : DomainException
        {
        }

        internal class DomainException : Exception
        {
        }

        private class GoodExceptionThrower
        {
            public GoodExceptionThrower()
            {
                throw new SpecificException();
            }
        }

        [Test]
        public void ExceptionsThrownMustBeDerivedFromConventionSpecification_Success()
        {
            typeof(GoodExceptionThrower)
                .MustConformTo(CecilConvention.ExceptionsThrownMustBeDerivedFrom(typeof(DomainException)))
                .IsSatisfied
                .Should()
                .BeTrue();
        }

        private class BadExceptionThrower
        {
            public BadExceptionThrower()
            {
                throw new Exception();
            }
        }
        

        [Test]
        public void ExceptionsThrownMustBeDerivedFromConventionSpecification_FailsIfExceptionDoesNotDeriveFromCorrectBase()
        {
            var result = typeof(BadExceptionThrower)
                .MustConformTo(CecilConvention.ExceptionsThrownMustBeDerivedFrom(typeof(DomainException)));

            result.IsSatisfied.Should().BeFalse();
            result.Failures.Should().HaveCount(1);
        }

        private class InstantiatesPropertiesProperly
        {
            public InstantiatesPropertiesProperly()
            {
                Names = new String[0];
            }

            public IEnumerable<string> Names { get; set; } 
        }

        [Test]
        public void MustInstantiatePropertiesOfSpecifiedTypeInDefaultConstructorConventionSpecification_Success()
        {
            typeof(InstantiatesPropertiesProperly)
                .MustConformTo(CecilConvention.MustInstantiatePropertiesOfSpecifiedTypeInDefaultConstructor(typeof(IEnumerable<>)))
                .IsSatisfied
                .Should()
                .BeTrue();
        }

        private class DoesNotInstantiatePropertiesProperly
        {
            public DoesNotInstantiatePropertiesProperly()
            {
            }

            public IEnumerable<string> Names { get; set; } 
        }


        [Test]
        public void MustInstantiatePropertiesOfSpecifiedTypeInDefaultConstructorConventionSpecification_FailsWhenPropertiesAreNotInstantiateInTheDefaultConstructor()
        {
            var result = typeof(DoesNotInstantiatePropertiesProperly)
                .MustConformTo(CecilConvention.MustInstantiatePropertiesOfSpecifiedTypeInDefaultConstructor(typeof(IEnumerable<>)));

            result.IsSatisfied.Should().BeFalse();
            result.Failures.Should().HaveCount(1);
        } 
    }
}