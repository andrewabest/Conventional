using Conventional.Conventions;

namespace Conventional.Net45
{
    public static class AsyncConvention
    {
        public static VoidMethodsMustNotBeAsyncConventionSpecification VoidMethodsMustNotBeAsync
        {
            get { return new VoidMethodsMustNotBeAsyncConventionSpecification(); }
        }

        public static AsyncMethodsMustHaveAsyncSuffixConventionSpecification AsyncMethodsMustHaveAsyncSuffix
        {
            get { return new AsyncMethodsMustHaveAsyncSuffixConventionSpecification(); }
        }
    }
}