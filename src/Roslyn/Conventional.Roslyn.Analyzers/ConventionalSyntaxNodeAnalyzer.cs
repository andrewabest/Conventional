using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Conventional.Roslyn.Analyzers
{
    /// <summary>
    /// Register an action to be executed at completion of semantic analysis of a SyntaxNode with an appropriate Kind. A syntax node action can report Diagnostics about SyntaxNodes, and can also collect state information to be used by other syntax node actions or code block end actions
    /// </summary>
    public abstract class ConventionalSyntaxNodeAnalyzer : DiagnosticAnalyzer
    {
        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSyntaxNodeAction(Analyze, SyntaxKind.IfStatement, SyntaxKind.ElseClause);
            context.EnableConcurrentExecution();
            // Generate diagnostic reports, but do not allow analyzer actions
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.ReportDiagnostics);
        }

        void Analyze(SyntaxNodeAnalysisContext context)
        {
            var result = CheckNode(context.Node);

            if (result.Success == false)
            {
                var loc = context.Node.GetLocation();
                var diagnostic = Diagnostic.Create(Rule, loc, $"{result.Message} must have braces");
                context.ReportDiagnostic(diagnostic);
            }
        }

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        protected abstract DiagnosticDescriptor Rule { get; }

        public abstract DiagnosticResult CheckNode(SyntaxNode node);
    }
}