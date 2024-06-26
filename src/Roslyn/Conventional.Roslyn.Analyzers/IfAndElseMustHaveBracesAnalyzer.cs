﻿using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Conventional.Roslyn.Analyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class IfAndElseMustHaveBracesAnalyzer : ConventionalSyntaxNodeAnalyzer
    {
        protected override DiagnosticDescriptor Rule => new DiagnosticDescriptor(DiagnosticDescriptorIdentifiers.IfAndElseMustHaveBracesAnalyzer,
            "IfAndElseMustHaveBracesAnalyzer",
            "{0}",
            "Conventional Analyzers",
            DiagnosticSeverity.Warning,
            true);

        public override DiagnosticResult CheckNode(SyntaxNode node, SemanticModel semanticModel)
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

        public override SyntaxKind[] SyntaxKinds() => new[] { SyntaxKind.IfStatement, SyntaxKind.ElseClause };
    }
}