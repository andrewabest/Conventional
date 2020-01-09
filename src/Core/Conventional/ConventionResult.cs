using System.Linq;

namespace Conventional
{
    public class ConventionResult
    {
        public ConventionResult(string subjectName)
        {
            SubjectName = subjectName;
            Failures = new string[0];
        }

        public string SubjectName { get; }

        public bool IsSatisfied { get; set; }

        public string[] Failures { get; set; }

        public static ConventionResult Satisfied(string subjectName)
        {
            return new ConventionResult(subjectName) { IsSatisfied = true };
        }

        public static ConventionResult NotSatisfied(string subjectName, string failureMessage)
        {
            return new ConventionResult(subjectName) { Failures = new[] { failureMessage } };
        }
    }

    public static class ConventionResultExtensions
    {
        public static ConventionResult And(this ConventionResult left, ConventionResult right)
        {
            return new ConventionResult(left.SubjectName)
            {
                IsSatisfied = left.IsSatisfied && right.IsSatisfied,
                Failures = left.Failures.Union(right.Failures).ToArray()
            };
        }
        
        public static ConventionResult Or(this ConventionResult left, ConventionResult right)
        {
            return new ConventionResult(left.SubjectName)
            {
                IsSatisfied = left.IsSatisfied || right.IsSatisfied,
                Failures = left.Failures.Union(right.Failures).ToArray()
            };
        }
        
        public static ConventionResult Not(this ConventionResult left)
        {
            return new ConventionResult(left.SubjectName)
            {
                IsSatisfied = !left.IsSatisfied,
                Failures = left.Failures.ToArray()
            };
        }
    }
}