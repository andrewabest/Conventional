namespace Conventional
{
    public class ConformanceResult
    {
        public ConformanceResult(string typeName)
        {
            TypeName = typeName;
        }

        public string TypeName { get; set; }

        public bool IsSatisfied { get; set; }

        public string FailureMessage { get; set; }

        public static ConformanceResult Satisfied(string typeName)
        {
            return new ConformanceResult(typeName) { IsSatisfied = true };
        }
        
        public static ConformanceResult NotSatisfied(string typeName, string failureMessage)
        {
            return new ConformanceResult(typeName) { FailureMessage = failureMessage };
        }
    }
}