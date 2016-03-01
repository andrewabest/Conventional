using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Conventional.Roslyn.Conventions
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class IfAndElseMustHaveBracesConventionSpecification : SolutionDiagnosticAnalyzerConventionSpecification
    {
        protected override string FailureMessage => "If and else must have braces, and {0} statement on line {1} does not";

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor("Conventional.IfAndElseMustHaveBracesAnalyzer",
            "IfAndElseMustHaveBracesAnalyzer",
            "{0}",
            "",
            DiagnosticSeverity.Warning, 
            true);


        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSyntaxNodeAction(AnalyzeIfOrElseStatement, SyntaxKind.IfStatement, SyntaxKind.ElseClause);
        }

        private void AnalyzeIfOrElseStatement(SyntaxNodeAnalysisContext context)
        {
            var result = CheckNode(context.Node);

            if (result.Success == false)
            {
                Location loc = context.Node.GetLocation();
                Diagnostic diagnostic = Diagnostic.Create(Rule, loc, "if");
                context.ReportDiagnostic(diagnostic);
            }
        }

        protected override DiagnosticResult CheckNode(SyntaxNode node, Document document = null)
        {
            var ifStatement = node as IfStatementSyntax;

            if ((ifStatement?.Statement != null) &&
                (ifStatement.Statement.IsKind(SyntaxKind.Block) == false))
            {
                return DiagnosticResult.Failed("if", GetLineNumber(document, node));
            }

            var elseSyntax = node as ElseClauseSyntax;
            if (elseSyntax?.Statement != null && 
                (elseSyntax.Statement.IsKind(SyntaxKind.IfStatement) == false) && 
                (elseSyntax.Statement.IsKind(SyntaxKind.Block) == false))
            {
                return DiagnosticResult.Failed("else", GetLineNumber(document, node));
            }

            return DiagnosticResult.Succeeded();
        }
    }
}