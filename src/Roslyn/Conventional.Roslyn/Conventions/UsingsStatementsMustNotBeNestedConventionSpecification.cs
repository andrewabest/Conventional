using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Conventional.Roslyn.Conventions
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class UsingsStatementsMustNotBeNestedConventionSpecification : SolutionDiagnosticAnalyzerConventionSpecification
    {
        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSyntaxNodeAction(AnalyzeUsingsAndNamespace, SyntaxKind.NamespaceDeclaration);
        }

        private void AnalyzeUsingsAndNamespace(SyntaxNodeAnalysisContext context)
        {
            var result = CheckNode(context.Node);

            if (result.Success == false)
            {
                Location loc = context.Node.GetLocation();
                Diagnostic diagnostic = Diagnostic.Create(Rule, loc);
                context.ReportDiagnostic(diagnostic);
            }
        }

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor("Conventional.UsingsStatementsMustNotBeNestedAnalyzer",
             "UsingsStatementsMustNotBeNestedAnalyzer",
             "{0}",
             "",
             DiagnosticSeverity.Warning,
             true);


        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        protected override string FailureMessage => "{0} statements must not be nested within the namespace, using on line {1} does not conform";

        public UsingsStatementsMustNotBeNestedConventionSpecification(string[] fileExemptions) : base(fileExemptions)
        {
        }

        protected override DiagnosticResult CheckNode(SyntaxNode node, Document document = null)
        {
            if (node is NamespaceDeclarationSyntax)
            {
                var usings = node.DescendantNodes().OfType<UsingDirectiveSyntax>().ToArray();
                if (usings.Any())
                {
                    return DiagnosticResult.Failed("Using", GetLineNumber(document, usings.First()));
                }
            }

            return DiagnosticResult.Succeeded();
        }
    }
}