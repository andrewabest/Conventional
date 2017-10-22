using System;

namespace Conventional.Conventions
{
    public class ConventionException : Exception
    {
        public ConventionException(string message) : base(message)
        {
        }
    }
}