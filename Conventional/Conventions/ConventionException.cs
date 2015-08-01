using System;

namespace Conventional.Conventions.Assemblies
{
    public class ConventionException : Exception
    {
        public ConventionException(string message) : base(message)
        {
        }
    }
}