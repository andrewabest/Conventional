using System;

namespace Conventional
{
    public static class ConventionConfiguration
    {
        public static Action<string> DefaultFailureAssertionCallback { get; set; }
        public static Action<string> DefaultWarningAssertionCallback { get; set; }
        public static Func<DateTime> DefaultCurrentDateResolver { get; set; } = () => DateTime.UtcNow;
        private static readonly Func<Type, bool> DefaultGlobalTypeFilter = t => true;
        public static Func<Type, bool> GlobalTypeFilter { get; set; } = DefaultGlobalTypeFilter;
        public static void ResetGlobalTypeFilter()
        {
            GlobalTypeFilter = DefaultGlobalTypeFilter;
        }
    }
}