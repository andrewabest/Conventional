using Microsoft.CodeAnalysis;

namespace Conventional.Roslyn.Analyzers
{
    public class DiagnosticResult
    {
        public string Message { get; private set; }
        public SyntaxNode[] FailedNodes { get; private set; }
        public bool Success { get; private set; }

        public static DiagnosticResult Succeeded()
        {
            return new DiagnosticResult() { Success = true };
        }

        public static DiagnosticResult Failed(string failureMessage, params SyntaxNode[] failedNodes)
        {
            return new DiagnosticResult() { Message = failureMessage, FailedNodes = failedNodes };
        }

        public void UpdateFailureMessage(string failureMessage)
        {
            Message = failureMessage;
        }
    }
}