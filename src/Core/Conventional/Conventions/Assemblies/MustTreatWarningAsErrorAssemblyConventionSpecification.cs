using System;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Conventional.Conventions.Assemblies
{
    public class MustTreatWarningAsErrorAssemblyConventionSpecification : AssemblyConventionSpecification
    {
        private readonly string _warning;

        public MustTreatWarningAsErrorAssemblyConventionSpecification(string warning)
        {
            _warning = warning;
        }

        protected override ConventionResult IsSatisfiedByLegacyCsprojFormat(string assemblyName, XDocument projectDocument)
        {
            return IsSatisfiedBy(assemblyName, projectDocument);
        }

        protected override ConventionResult IsSatisfiedBy(string assemblyName, XDocument projectDocument)
        {
            // Note: The Project element (and descendants) are namespaced in legacy csproj files, so our XPath ignores the
            // Note: namespace by considering the local element name only. Once we no-longer need to support legacy csproj
            // Note: files, the XPath can be simplified to /Project/ItemGroup/ProjectReference
            var warningsAsErrors = projectDocument.XPathSelectElements("/*[local-name() = 'Project']/*[local-name() = 'PropertyGroup']/*[local-name() = 'WarningsAsErrors']")
                .SingleOrDefault()
                ?.Value ?? "";

            var warnings = warningsAsErrors.Split(new[] {' ', ','}, StringSplitOptions.RemoveEmptyEntries);
            if (!warnings.Contains(_warning))
            {
                return ConventionResult.NotSatisfied(assemblyName, string.Format(FailureMessage, assemblyName, _warning));
            }

            return ConventionResult.Satisfied(assemblyName);
        }

        protected override string FailureMessage => "Assembly {0} should treat warning {1} as an error but does not";
    }
}