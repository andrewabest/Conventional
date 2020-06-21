using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Conventional.Roslyn.Analyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class IfAndElseMustHaveBracesAnalyzer : ConventionalSyntaxNodeAnalyzer
    {
        protected override DiagnosticDescriptor Rule => new DiagnosticDescriptor("Conventional.IfAndElseMustHaveBracesAnalyzer",
            "IfAndElseMustHaveBracesAnalyzer",
            "{0}",
            "",
            DiagnosticSeverity.Warning,
            true);

        public override DiagnosticResult CheckNode(SyntaxNode node)
        {
            switch (node)
            {
                case IfStatementSyntax ifStatement when (ifStatement.Statement.IsKind(SyntaxKind.Block) == false):
                    return DiagnosticResult.Failed("If");
                case ElseClauseSyntax elseSyntax when (elseSyntax.Statement.IsKind(SyntaxKind.IfStatement) == false) &&
                                                      (elseSyntax.Statement.IsKind(SyntaxKind.Block) == false):
                    return DiagnosticResult.Failed("Else");
                default:
                    return DiagnosticResult.Succeeded();
            }
        }
    }
}