using System.Linq;
using Conventional.Roslyn.Analyzers;
using Microsoft.CodeAnalysis;

namespace Conventional.Roslyn.Conventions
{
    public class UsingsStatementsMustNotBeNestedConventionSpecification : SolutionDiagnosticAnalyzerConventionSpecification
    {
        protected override string FailureMessage => "{0} statements must not be nested within the namespace, using on line {1} does not conform";

        public UsingsStatementsMustNotBeNestedConventionSpecification(string[] fileExemptions) : base(new UsingsStatementsMustNotBeNestedAnalyzer(), fileExemptions)
        {
        }

        protected override DiagnosticResult CheckNode(SyntaxNode node, Document document = null, SemanticModel semanticModel = null)
        {
            var result = Analyzer.CheckNode(node, semanticModel);

            if (result.Success == false)
            {
                var lineNumbers = result.FailedNodes.Any()
                    ? string.Join(",", result.FailedNodes.Select(x => GetLineNumber(document, x)))
                    : GetLineNumber(document, node).ToString();

                result.UpdateFailureMessage(FailureMessage.FormatWith(result.Message, lineNumbers));
            }

            return result;
        }
    }
}