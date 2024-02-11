using System;
using System.Collections.Generic;
using System.Linq;
using Conventional.Roslyn.Analyzers;
using Microsoft.CodeAnalysis;

namespace Conventional.Roslyn.Conventions
{
    public interface ISolutionDiagnosticAnalyzerConventionSpecification
    {
        IEnumerable<ConventionResult> IsSatisfiedBy(Solution solution, int knownOffenders);
    }

    public abstract class SolutionDiagnosticAnalyzerConventionSpecification :
        ISolutionDiagnosticAnalyzerConventionSpecification
    {
        // ReSharper disable once UnusedMemberInSuper.Global
        protected abstract string FailureMessage { get; }
        private readonly string[] _fileExemptions;

        protected SolutionDiagnosticAnalyzerConventionSpecification(string[] fileExemptions)
        {
            _fileExemptions = fileExemptions;
        }

        public IEnumerable<ConventionResult> IsSatisfiedBy(Solution solution, int knownOffenders)
        {
            var results = solution.Projects.SelectMany(x => x.Documents
                    .Where(d => d.SupportsSyntaxTree)
                    .Where(d => !_fileExemptions.Any(d.FilePath.EndsWith)))
                .SelectMany(IsSatisfiedBy)
                .ToArray();
            var filteredFailures = RemoveKnownOffenders(results, knownOffenders);
            var final = results.Where(x => x.IsSatisfied).ToList();
            final.AddRange(filteredFailures);

            return final;
        }

        private static IEnumerable<ConventionResult> RemoveKnownOffenders(ConventionResult[] results, int knownOffenders)
        {
            var failures = results.Where(x => !x.IsSatisfied).ToArray();
            return failures.Take(Math.Max(failures.Length - Math.Max(knownOffenders, 0), 0)).ToArray();
        }

        private IEnumerable<ConventionResult> IsSatisfiedBy(Document document)
        {
            var node = document.GetSyntaxRootAsync().Result;
            if (document.TryGetSemanticModel(out var semanticModel))
            {
                return IsSatisfiedBy(document, node, semanticModel);
            }

            return IsSatisfiedBy(document, node);
        }

        private IEnumerable<ConventionResult> IsSatisfiedBy(Document document, SyntaxNode syntaxNode) => IsSatisfiedBy(document, syntaxNode, null);

        private IEnumerable<ConventionResult> IsSatisfiedBy(Document document, SyntaxNode node, SemanticModel semanticModel)
        {
            yield return BuildResult(document, node, semanticModel);

            foreach (var childResult in node.ChildNodes().SelectMany(x => IsSatisfiedBy(document, x)))
            {
                yield return childResult;
            }
        }

        private ConventionResult BuildResult(Document document, SyntaxNode node, SemanticModel semanticModel)
        {
            var result = CheckNode(node, document, semanticModel);

            return result.Success
                ? ConventionResult.Satisfied(document.FilePath)
                : ConventionResult.NotSatisfied(document.FilePath, result.Message);
        }

        protected abstract DiagnosticResult CheckNode(SyntaxNode node, Document document = null, SemanticModel semanticModel = null);

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