using System;

namespace Conventional
{
    public static class ConventionConfiguration
    {
        public static Action<string> DefaultFailureAssertionCallback { get; set; }
    }
}