using System.Linq;
using Conventional.Roslyn.Analyzers;
using Microsoft.CodeAnalysis;

namespace Conventional.Roslyn.Conventions
{
    public class IfAndElseMustHaveBracesConventionSpecification : SolutionDiagnosticAnalyzerConventionSpecification
    {
        protected override string FailureMessage => "If and else must have braces, and {0} statement on line {1} does not";

        public IfAndElseMustHaveBracesConventionSpecification(string[] fileExemptions) : base(new IfAndElseMustHaveBracesAnalyzer(), fileExemptions)
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