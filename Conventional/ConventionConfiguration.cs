using System;

namespace Conventional
{
    public static class ConventionConfiguration
    {
        public static Action<string> DefaultFailureAssertionCallback { get; set; }
        public static Action<string> DefaultWarningAssertionCallback { get; set; }
        public static Func<DateTime> DefaultCurrentDateResolver { get; set; } = () => DateTime.UtcNow;
    }
}