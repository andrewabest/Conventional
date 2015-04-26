using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace Conventional.Tests.Net45.Conventional.Conventions.Cecil
{
    public class CecilConventionSpecificationTests
    {
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
            var expectedFailureMessage = @"
Libraries should call Task.ConfigureAwait(false) to prevent deadlocks
	- HasAnAsyncMethodThatAwaitsATaskAndDoesNotCallConfigureAwait.MethodThatAwaitsATaskAndDoesNotCallConfigureAwait
".TrimStart();

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
            var expectedFailureMessage = @"
Libraries should call Task.ConfigureAwait(false) to prevent deadlocks
	- HasAnAsyncMethodThatAwaitsATaskAndDoesNotCallConfigureAwaitAndAnotherThatDoes.MethodThatAwaitsATaskAndDoesNotCallConfigureAwait
".TrimStart();

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
            var expectedFailureMessage = @"
Libraries should call Task.ConfigureAwait(false) to prevent deadlocks
	- HasAnAsyncMethodThatAwaitsMultipleTasksAndDoesNotCallConfigureAwaitForOne.MethodThatAwaitsMultipleTasksAndDoesNotCallConfigureAwaitForOne
".TrimStart();

            var result = typeof(HasAnAsyncMethodThatAwaitsMultipleTasksAndDoesNotCallConfigureAwaitForOne)
                .MustConformTo(Convention.LibraryCodeShouldCallConfigureAwaitWhenAwaitingTasks);

            result.IsSatisfied.Should().BeFalse();
            result.Failures.Single().Should().Be(expectedFailureMessage);
        }
    }
}