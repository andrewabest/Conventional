using FluentAssertions;
using NUnit.Framework;

namespace Conventional.Tests.Net45.Conventional.Conventions
{
    public class ConventionSpecificationTests
    {
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