using System.Linq;
using Conventional.Roslyn.Analyzers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Conventional.Roslyn.Conventions
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class UsingsStatementsMustNotBeNestedConventionSpecification : SolutionDiagnosticAnalyzerConventionSpecification
    {
        private readonly UsingsStatementsMustNotBeNestedAnalyzer _analyzer;
        protected override string FailureMessage => "{0} statements must not be nested within the namespace, using on line {1} does not conform";

        public UsingsStatementsMustNotBeNestedConventionSpecification(string[] fileExemptions) : base(fileExemptions)
        {
            _analyzer = new UsingsStatementsMustNotBeNestedAnalyzer();
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