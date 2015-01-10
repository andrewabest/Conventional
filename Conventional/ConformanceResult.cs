using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Conventional
{
    public class ConventionResult
    {
        public ConventionResult(string typeName)
        {
            TypeName = typeName;
            Failures = new string[0];
        }

        public string TypeName { get; set; }

        public bool IsSatisfied { get; set; }

        public string[] Failures { get; set; }

        public static ConventionResult Satisfied(string typeName)
        {
            return new ConventionResult(typeName) { IsSatisfied = true };
        }

        public static ConventionResult NotSatisfied(string typeName, string failureMessage)
        {
            return new ConventionResult(typeName) { Failures = new[] { failureMessage } };
        }
    }

    public static class ConventionResultExtensions
    {
        public static ConventionResult And(this ConventionResult left, ConventionResult right)
        {
            return new ConventionResult(left.TypeName)
            {
                IsSatisfied = left.IsSatisfied && right.IsSatisfied,
                Failures = left.Failures.Union(right.Failures).ToArray()
            };
        }
        
        public static ConventionResult Or(this ConventionResult left, ConventionResult right)
        {
            return new ConventionResult(left.TypeName)
            {
                IsSatisfied = left.IsSatisfied || right.IsSatisfied,
                Failures = left.Failures.Union(right.Failures).ToArray()
            };
        }
        
        public static ConventionResult Not(this ConventionResult left)
        {
            return new ConventionResult(left.TypeName)
            {
                IsSatisfied = !left.IsSatisfied,
                Failures = left.Failures.ToArray()
            };
        }
    }
}