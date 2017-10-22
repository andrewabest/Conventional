using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace Conventional.Tests.Conventional.Conventions.Cecil
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
                .MustConformTo(Convention.MustNotResolveCurrentTimeViaDateTime)
                .IsSatisfied
                .Should()
                .BeTrue();
        }

        private class OffendingDateTimeNowCitizen
        {
            private DateTime _current;

            public OffendingDateTimeNowCitizen()
            {
                _current = DateTime.Now;
            }
        }

        private class OffendingDateTimeTodayCitizen
        {
            private DateTime _current;

            public OffendingDateTimeTodayCitizen()
            {
                _current = DateTime.Today;
            }
        }

        private class OffendingDateTimeUtcNowCitizen
        {
            private DateTime _current;

            public OffendingDateTimeUtcNowCitizen()
            {
                _current = DateTime.UtcNow;
            }
        }

        [Test]
        [TestCase(typeof(OffendingDateTimeNowCitizen))]
        [TestCase(typeof(OffendingDateTimeTodayCitizen))]
        [TestCase(typeof(OffendingDateTimeUtcNowCitizen))]
        public void MustNotUseDateTimeNowConventionSpecification_FailsWhenACallToDateTimeExists(Type offendingType)
        {
            var result = offendingType.MustConformTo(Convention.MustNotResolveCurrentTimeViaDateTime);

            result.IsSatisfied.Should().BeFalse();
            result.Failures.Should().HaveCount(1);
            result.Failures.First().Should().Contain(offendingType.FullName);
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
                .MustConformTo(Convention.MustNotUseDateTimeOffsetNow)
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
                .MustConformTo(Convention.MustNotUseDateTimeOffsetNow);

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
                .MustConformTo(Convention.ExceptionsThrownMustBeDerivedFrom(typeof(DomainException)))
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
                .MustConformTo(Convention.ExceptionsThrownMustBeDerivedFrom(typeof(DomainException)));

            result.IsSatisfied.Should().BeFalse();
            result.Failures.Should().HaveCount(1);
        }

        private class Money
        {
        }

        private class InstantiatesPropertiesProperly
        {
            public InstantiatesPropertiesProperly()
            {
                Names = new String[0];
                Amount = new Money();
            }

            public IEnumerable<string> Names { get; set; } 

            public Money Amount { get; set; }
        }

        [Test]
        public void MustInstantiatePropertiesOfSpecifiedTypeInDefaultConstructorConventionSpecification_Success()
        {
            typeof(InstantiatesPropertiesProperly)
                .MustConformTo(Convention.MustInstantiatePropertiesOfSpecifiedTypeInDefaultConstructor(typeof(IEnumerable<>)))
                .IsSatisfied
                .Should()
                .BeTrue();
        }

        [Test]
        public void MustInstantiatePropertiesOfSpecifiedTypeInDefaultConstructorConventionSpecification_SuccessWithMultipleTypes()
        {
            typeof(InstantiatesPropertiesProperly)
                .MustConformTo(Convention.MustInstantiatePropertiesOfSpecifiedTypeInDefaultConstructor(new [] { typeof(IEnumerable<>), typeof(Money) }))
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

            public Money Amount { get; set; }
        }


        [Test]
        public void MustInstantiatePropertiesOfSpecifiedTypeInDefaultConstructorConventionSpecification_FailsWhenPropertiesAreNotInstantiateInTheDefaultConstructor()
        {
            var result = typeof(DoesNotInstantiatePropertiesProperly)
                .MustConformTo(Convention.MustInstantiatePropertiesOfSpecifiedTypeInDefaultConstructor(typeof(IEnumerable<>)));

            result.IsSatisfied.Should().BeFalse();
            result.Failures.Should().HaveCount(1);
        }

        [Test]
        public void MustInstantiatePropertiesOfSpecifiedTypeInDefaultConstructorConventionSpecification_FailsWhenPropertiesAreNotInstantiateInTheDefaultConstructor_WithMultipleTypes()
        {
            var result = typeof(DoesNotInstantiatePropertiesProperly)
                .MustConformTo(Convention.MustInstantiatePropertiesOfSpecifiedTypeInDefaultConstructor(new[] { typeof(IEnumerable<>), typeof(Money) }));

            result.IsSatisfied.Should().BeFalse();
            result.Failures.Should().HaveCount(1);
        }

        private class AssignsAllPropertiesDuringConstruction
        {
            public AssignsAllPropertiesDuringConstruction(int id)
            {
                Id = id;
            }

            public int Id { get; set; }
        }

        [Test]
        public void AllPropertiesMustBeAssignedDuringConstructionConventionSpecification_Success()
        {
            typeof (AssignsAllPropertiesDuringConstruction)
                .MustConformTo(Convention.AllPropertiesMustBeAssignedDuringConstruction())
                .IsSatisfied
                .Should()
                .BeTrue();
        }

        private class DoesNotAssignAllPropertiesDuringConstruction
        {
            public DoesNotAssignAllPropertiesDuringConstruction(int id)
            {
                Id = id;
            }

            public int Id { get; set; }
            public string Name { get; set; }
        }

        [Test]
        public void
            AllPropertiesMustBeAssignedDuringConstructionConventionSpecification_FailsWhenNotAllPropertiesAreAssignedDuringConstruction
            ()
        {
            var result = typeof(DoesNotAssignAllPropertiesDuringConstruction)
                .MustConformTo(Convention.AllPropertiesMustBeAssignedDuringConstruction());

            result.IsSatisfied.Should().BeFalse();
            result.Failures.Should().HaveCount(1);
        }

        private class DoesNotHaveAParametereizedConstructor
        {
            public int Id { get; set; }
        }

        [Test]
        public void
            AllPropertiesMustBeAssignedDuringConstructionConventionSpecification_FailsWhenNoParameterizedConstructorExists
            ()
        {
            var result = typeof(DoesNotHaveAParametereizedConstructor)
                .MustConformTo(Convention.AllPropertiesMustBeAssignedDuringConstruction());

            result.IsSatisfied.Should().BeFalse();
            result.Failures.Should().HaveCount(1);
        }
        

        private class HasMoreThanOneParameterizedConstructor
        {
            public HasMoreThanOneParameterizedConstructor(int id)
            {
                Id = id;
            }

            public HasMoreThanOneParameterizedConstructor(int id, string name)
            {
                Id = id;
                Name = name;
            }

            public int Id { get; set; }
            public string Name { get; set; }
        }
        
        private class HasNoConstructors
        {
        }

        [Test]
        public void
            AllPropertiesMustBeAssignedDuringConstructionConventionSpecification_FailsWhenMoreThanOneParameterizedConstructorExists
            ()
        {
            var result = typeof(HasMoreThanOneParameterizedConstructor)
                .MustConformTo(Convention.AllPropertiesMustBeAssignedDuringConstruction());

            result.IsSatisfied.Should().BeFalse();
            result.Failures.Should().HaveCount(1);
        }

        [Test]
        public void
            AllPropertiesMustBeAssignedDuringConstructionConventionSpecification_PassesWhenNoConstructorsExistsAndIsReopinionated
            ()
        {
            var result = typeof(HasNoConstructors)
                .MustConformTo(Convention.AllPropertiesMustBeAssignedDuringConstruction(true));

            result.IsSatisfied.Should().BeTrue();
            result.Failures.Should().HaveCount(0);
        }

        [Test]
        public void
            AllPropertiesMustBeAssignedDuringConstructionConventionSpecification_FailsWhenNoConstructorsExists
            ()
        {
            var result = typeof(HasNoConstructors)
                .MustConformTo(Convention.AllPropertiesMustBeAssignedDuringConstruction());

            result.IsSatisfied.Should().BeFalse();
            result.Failures.Should().HaveCount(1);
        }

        private class HasAnAsyncMethodThatAwaitsATaskAndCallsConfigureAwait
        {
            public async Task MethodThatAwaitsATaskAndCallsConfigureAwait()
            {
                await Task.FromResult(0).ConfigureAwait(false);
            }
        }

        [Test]
        public void LibraryCodeShouldCallConfigureAwaitWhenAwaitingTasks_Success()
        {
            typeof(HasAnAsyncMethodThatAwaitsATaskAndCallsConfigureAwait)
                .MustConformTo(Convention.LibraryCodeShouldCallConfigureAwaitWhenAwaitingTasks)
                .IsSatisfied
                .Should()
                .BeTrue();
        }

        private class HasAnAsyncMethodThatAwaitsATaskAndDoesNotCallConfigureAwait
        {
            public async Task MethodThatAwaitsATaskAndDoesNotCallConfigureAwait()
            {
                await Task.FromResult(0);
            }
        }

        [Test]
        public void LibraryCodeShouldCallConfigureAwaitWhenAwaitingTasks_FailsWhenConfigureAwaitIsNotCalled()
        {

            const string expectedFailureMessage = @"Libraries must call Task.ConfigureAwait(false) to prevent deadlocks
	- HasAnAsyncMethodThatAwaitsATaskAndDoesNotCallConfigureAwait.MethodThatAwaitsATaskAndDoesNotCallConfigureAwait
";

            var result = typeof(HasAnAsyncMethodThatAwaitsATaskAndDoesNotCallConfigureAwait)
                .MustConformTo(Convention.LibraryCodeShouldCallConfigureAwaitWhenAwaitingTasks);

            result.IsSatisfied.Should().BeFalse();
            result.Failures.Single().Should().Be(expectedFailureMessage);
        }

        private class HasAnAsyncMethodThatAwaitsATaskAndDoesNotCallConfigureAwaitAndAnotherThatDoes
        {
            public async Task MethodThatAwaitsATaskAndDoesNotCallConfigureAwait()
            {
                await Task.FromResult(0);
            }

            public async Task MethodThatAwaitsATaskAndCallsConfigureAwait()
            {
                await Task.FromResult(0).ConfigureAwait(false);
            }
        }

        [Test]
        public void LibraryCodeShouldCallConfigureAwaitWhenAwaitingTasks_FailsWhenConfigureAwaitIsNotCalledButIsCalledForOtherMethods()
        {
            const string expectedFailureMessage = @"Libraries must call Task.ConfigureAwait(false) to prevent deadlocks
	- HasAnAsyncMethodThatAwaitsATaskAndDoesNotCallConfigureAwaitAndAnotherThatDoes.MethodThatAwaitsATaskAndDoesNotCallConfigureAwait
";
  
            var result = typeof(HasAnAsyncMethodThatAwaitsATaskAndDoesNotCallConfigureAwaitAndAnotherThatDoes)
                .MustConformTo(Convention.LibraryCodeShouldCallConfigureAwaitWhenAwaitingTasks);

            result.IsSatisfied.Should().BeFalse();
            result.Failures.Single().Should().Be(expectedFailureMessage);
        }

        private class HasAnAsyncMethodThatAwaitsMultipleTasksAndDoesNotCallConfigureAwaitForOne
        {
            public async Task MethodThatAwaitsMultipleTasksAndDoesNotCallConfigureAwaitForOne()
            {
                await Task.FromResult(0).ConfigureAwait(false);
                await Task.FromResult(0);
            }
        }

        [Test]
        public void LibraryCodeShouldCallConfigureAwaitWhenAwaitingTasks_FailsWhenConfigureAwaitIsNotCalledForASingleTask()
        {
            const string expectedFailureMessage = @"Libraries must call Task.ConfigureAwait(false) to prevent deadlocks
	- HasAnAsyncMethodThatAwaitsMultipleTasksAndDoesNotCallConfigureAwaitForOne.MethodThatAwaitsMultipleTasksAndDoesNotCallConfigureAwaitForOne
";

            var result = typeof(HasAnAsyncMethodThatAwaitsMultipleTasksAndDoesNotCallConfigureAwaitForOne)
                .MustConformTo(Convention.LibraryCodeShouldCallConfigureAwaitWhenAwaitingTasks);

            result.IsSatisfied.Should().BeFalse();
            result.Failures.Single().Should().Be(expectedFailureMessage);
        }
    }
}