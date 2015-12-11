using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.MSBuild;
using Microsoft.CodeAnalysis.Text;


namespace Conventional.Conventions.Roslyn
{
    public interface ISolutionDiagnosticAnalyzerConventionSpecification
    {
        IEnumerable<ConventionResult> IsSatisfiedBy(Microsoft.CodeAnalysis.Solution solution);
    }

    public abstract class SolutionDiagnosticAnalyzerConventionSpecification : DiagnosticAnalyzer, ISolutionDiagnosticAnalyzerConventionSpecification
    {
        protected abstract string FailureMessage { get; }

        public IEnumerable<ConventionResult> IsSatisfiedBy(Microsoft.CodeAnalysis.Solution solution)
        {
            return solution.Projects.SelectMany(x => x.Documents.Where(d => d.SupportsSyntaxTree)).SelectMany(IsSatisfiedBy);
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
                : ConventionResult.NotSatisfied(document.FilePath, FailureMessage.FormatWith(result.Message, result.LineNumber));
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