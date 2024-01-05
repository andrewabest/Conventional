using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Conventional.Conventions.Assemblies
{
    public abstract class PackageReferenceAssemblyConventionSpecification : AssemblyConventionSpecification
    {
        protected IEnumerable<string> GetPackageReferences(XDocument projectDocument)
        {
            // Note: The Project element (and descendants) are namespaced in legacy csproj files, so our XPath ignores the
            // Note: namespace by considering the local element name only. Once we no-longer need to support legacy csproj
            // Note: files, the XPath can be simplified to /Project/ItemGroup/PackageReference
            return projectDocument.XPathSelectElements("/*[local-name() = 'Project']/*[local-name() = 'ItemGroup']/*[local-name() = 'PackageReference']")
                .Select(referenceElement => referenceElement.Attribute("Include")?.Value)
                .Where(value => value != null);
        }

        protected override ConventionResult IsSatisfiedByLegacyCsprojFormat(string assemblyName, XDocument projectDocument)
        {
            return IsSatisfiedBy(assemblyName, projectDocument);
        }
    }
}