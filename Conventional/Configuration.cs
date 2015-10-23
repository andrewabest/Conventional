using System;

namespace Conventional
{
    public static class Configuration
    {
        public static Action<string> DefaultFailureAssertionCallback { get; set; }
    }
}