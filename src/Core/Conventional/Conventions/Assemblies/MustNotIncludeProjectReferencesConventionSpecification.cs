using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Conventional.Conventions.Assemblies
{
    public class MustNotIncludeProjectReferencesConventionSpecification : AssemblyConventionSpecification
    {
        protected override ConventionResult IsSatisfiedBy(string assemblyName, XDocument projectDocument)
        {
            var projectReferences = GetProjectReferences(projectDocument).ToArray();

            if (projectReferences.Any())
            {
                return ConventionResult.NotSatisfied(assemblyName, string.Format(FailureMessage, assemblyName, projectReferences.First()));
            }

            return ConventionResult.Satisfied(assemblyName);
        }

        protected override ConventionResult IsSatisfiedByLegacyCsprojFormat(string assemblyName, XDocument projectDocument)
        {
            return IsSatisfiedBy(assemblyName, projectDocument);
        }

        private IEnumerable<string> GetProjectReferences(XDocument projectDocument)
        {
            // Note: The Project element (and descendants) are namespaced in legacy csproj files, so our XPath ignores the
            // Note: namespace by considering the local element name only. Once we no-longer need to support legacy csproj
            // Note: files, the XPath can be simplified to /Project/ItemGroup/ProjectReference
            return projectDocument.XPathSelectElements("/*[local-name() = 'Project']/*[local-name() = 'ItemGroup']/*[local-name() = 'ProjectReference']")
                .Select(referenceElement => referenceElement.Attribute("Include")?.Value)
                .Where(value => value != null);
        }

        protected override string FailureMessage => "{0} includes reference to project {1}";
    }
}