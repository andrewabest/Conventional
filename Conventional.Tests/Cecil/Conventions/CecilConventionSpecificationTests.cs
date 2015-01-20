using System;
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
    }
}