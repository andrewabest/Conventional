﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Conventional.Roslyn.Analyzers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Conventional.Roslyn.Conventions
{
    public interface ISolutionDiagnosticAnalyzerConventionSpecification
    {
        IEnumerable<ConventionResult> IsSatisfiedBy(Solution solution);
    }

    public abstract class SolutionDiagnosticAnalyzerConventionSpecification : ISolutionDiagnosticAnalyzerConventionSpecification
    {
        // ReSharper disable once UnusedMemberInSuper.Global
        protected abstract string FailureMessage { get; }
        protected readonly ConventionalSyntaxNodeAnalyzer Analyzer;
        private readonly string[] _fileExemptions;

        protected SolutionDiagnosticAnalyzerConventionSpecification(ConventionalSyntaxNodeAnalyzer diagnosticAnalyzer, string[] fileExemptions)
        {
            Analyzer = diagnosticAnalyzer;
            _fileExemptions = fileExemptions;
        }

        public IEnumerable<ConventionResult> IsSatisfiedBy(Solution solution)
        {
            if (Analyzer.EnableConcurrentExecution())
            {
                var tasks = solution
                    .Projects.SelectMany(x =>
                        x.Documents
                            .Where(d => d.SupportsSyntaxTree)
                            .Where(FilterFileExceptions)
                            .Select(doc => Task.Run(() => IsSatisfiedBy(doc))));
                var result = Task.WhenAll(tasks).GetAwaiter().GetResult();
                return result.SelectMany(x => x);
            }

            return solution
                .Projects.SelectMany(x =>
                    x.Documents
                        .Where(d => d.SupportsSyntaxTree)
                        .Where(FilterFileExceptions)
                        .SelectMany(IsSatisfiedBy));
        }

        private bool FilterFileExceptions(Document document) => !_fileExemptions.Any(exemption => document.FilePath?.EndsWith(exemption) ?? true);

        private IEnumerable<ConventionResult> IsSatisfiedBy(Document document)
        {
            var node = document.GetSyntaxRootAsync().Result;
            var semanticModel = document.GetSemanticModelAsync().GetAwaiter().GetResult();
            if (semanticModel is not null)
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
            var kinds = Analyzer.SyntaxKinds();
            if (kinds.Length > 0 && !kinds.Contains(node!.Kind())) return ConventionResult.Satisfied(document.FilePath);

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