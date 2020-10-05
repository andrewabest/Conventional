using System.Linq;
using Conventional.Roslyn.Analyzers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Conventional.Roslyn.Conventions
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class IfAndElseMustHaveBracesConventionSpecification : SolutionDiagnosticAnalyzerConventionSpecification
    {
        private readonly IfAndElseMustHaveBracesAnalyzer _analyzer;
        protected override string FailureMessage => "If and else must have braces, and {0} statement on line {1} does not";

        public IfAndElseMustHaveBracesConventionSpecification(string[] fileExemptions) : base(fileExemptions)
        {
            _analyzer = new IfAndElseMustHaveBracesAnalyzer();
        }

        protected override DiagnosticResult CheckNode(SyntaxNode node, Document document = null)
        {
            var result = _analyzer.CheckNode(node);

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