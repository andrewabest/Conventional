﻿using System;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Conventional.Roslyn.Analyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class UsingsStatementsMustNotBeNestedAnalyzer : ConventionalSyntaxNodeAnalyzer
    {
        protected override DiagnosticDescriptor Rule => new DiagnosticDescriptor(DiagnosticDescriptorIdentifiers.UsingsStatementsMustNotBeNestedAnalyzer,
            "UsingsStatementsMustNotBeNestedAnalyzer",
            "{0}",
            "Conventional Analyzers",
            DiagnosticSeverity.Warning,
            true);

        public override DiagnosticResult CheckNode(SyntaxNode node, SemanticModel semanticModel)
        {
            if (node is NamespaceDeclarationSyntax)
            {
                var usings = node.DescendantNodes().OfType<UsingDirectiveSyntax>().ToArray();
                if (usings.Any())
                {
                    return DiagnosticResult.Failed("Using", usings.First());
                }
            }

            return DiagnosticResult.Succeeded();
        }

        public override SyntaxKind[] SyntaxKinds() => Array.Empty<SyntaxKind>();
    }
}