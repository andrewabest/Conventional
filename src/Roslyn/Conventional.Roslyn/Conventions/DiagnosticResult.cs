namespace Conventional.Roslyn.Conventions
{
    public class DiagnosticResult
    {
        public string Message { get; private set; }
        public int LineNumber { get; private set; }
        public bool Success { get; private set; }

        public static DiagnosticResult Succeeded()
        {
            return new DiagnosticResult() { Success = true };
        }

        public static DiagnosticResult Failed(string failureMessage, int lineNumber)
        {
            return new DiagnosticResult() { Message = failureMessage, LineNumber = lineNumber };
        }
    }
}