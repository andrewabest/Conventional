using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Conventional.Roslyn.Conventions
{
    public interface ISolutionDiagnosticAnalyzerConventionSpecification
    {
        IEnumerable<ConventionResult> IsSatisfiedBy(Solution solution);
    }

    public abstract class SolutionDiagnosticAnalyzerConventionSpecification : DiagnosticAnalyzer,
        ISolutionDiagnosticAnalyzerConventionSpecification
    {
        protected abstract string FailureMessage { get; }
        private readonly string[] _fileExemptions;

        protected SolutionDiagnosticAnalyzerConventionSpecification(string[] fileExemptions)
        {
            _fileExemptions = fileExemptions;
        }

        public IEnumerable<ConventionResult> IsSatisfiedBy(Solution solution)
        {
            return solution.Projects.SelectMany(x => x.Documents
                    .Where(d => d.SupportsSyntaxTree)
                    .Where(d => !_fileExemptions.Any(d.FilePath.EndsWith)))
                .SelectMany(IsSatisfiedBy);
        }

        private IEnumerable<ConventionResult> IsSatisfiedBy(Document document)
        {
            var node = document.GetSyntaxRootAsync().Result;

            return IsSatisfiedBy(document, node);
        }

        private IEnumerable<ConventionResult> IsSatisfiedBy(Document document, SyntaxNode node)
        {
            yield return BuildResult(document, node);

            foreach (var childResult in node.ChildNodes().SelectMany(x => IsSatisfiedBy(document, x)))
            {
                yield return childResult;
            }
        }

        private ConventionResult BuildResult(Document document, SyntaxNode node)
        {
            var result = CheckNode(node, document);

            return result.Success
                ? ConventionResult.Satisfied(document.FilePath)
                : ConventionResult.NotSatisfied(document.FilePath,
                    FailureMessage.FormatWith(result.Message, result.LineNumber));
        }

        protected abstract DiagnosticResult CheckNode(SyntaxNode node, Document document = null);

        protected static int GetLineNumber(Document document, SyntaxNode node)
        {
            int lineNumber;
            if (document != null)
            {
                lineNumber =
                    document.GetSyntaxTreeAsync()
                        .Result.GetText()
                        .Lines.Last(x => x.Span.Start <= node.SpanStart)
                        .LineNumber + 1;
            }
            else
            {
                return node.SpanStart;
            }

            return lineNumber;
        }
    }
}