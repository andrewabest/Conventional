using System;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Conventional.Conventions.Assemblies
{
    public class MustSetPropertyValueAssemblyConventionSpecification : AssemblyConventionSpecification
    {
        private string ExpectedPropertyName { get; }
        private string ExpectedPropertyValue { get; }

        public MustSetPropertyValueAssemblyConventionSpecification(string expectedPropertyName, string expectedPropertyValue)
        {
            ExpectedPropertyName = expectedPropertyName;
            ExpectedPropertyValue = expectedPropertyValue;
        }

        protected override ConventionResult IsSatisfiedByLegacyCsprojFormat(string assemblyName, XDocument projectDocument)
        {
            return IsSatisfiedBy(assemblyName, projectDocument);
        }

        protected override ConventionResult IsSatisfiedBy(string assemblyName, XDocument projectDocument)
        {
            // Note: The Project element (and descendants) are namespaced in legacy csproj files, so our XPath ignores the
            // Note: namespace by considering the local element name only. Once we no-longer need to support legacy csproj
            // Note: files, the XPath can be simplified to /Project/PropertyGroup/{ExpectedPropertyName}
            var matchingProperties = projectDocument.XPathSelectElements($"/*[local-name() = 'Project']/*[local-name() = 'PropertyGroup']/*[local-name() = '{ExpectedPropertyName}']")
                .Select(propertyElement => propertyElement.Value)
                .Where(propertyValue => string.Equals(ExpectedPropertyValue, propertyValue, StringComparison.InvariantCulture));

            return matchingProperties.Count() == 1
                ? ConventionResult.Satisfied(assemblyName)
                : ConventionResult.NotSatisfied(assemblyName, string.Format(FailureMessage, assemblyName));
        }

        protected override string FailureMessage => "{0} should have property " + ExpectedPropertyName + " with value " + ExpectedPropertyValue;
    }
}