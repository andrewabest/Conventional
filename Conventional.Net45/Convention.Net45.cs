using Conventional.Conventions;

namespace Conventional
{
    public static partial class Convention
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
