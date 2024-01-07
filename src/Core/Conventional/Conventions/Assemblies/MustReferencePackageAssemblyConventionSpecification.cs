using System.Linq;
using System.Xml.Linq;

namespace Conventional.Conventions.Assemblies
{
    public class MustReferencePackageAssemblyConventionSpecification : PackageReferenceAssemblyConventionSpecification
    {
        private readonly string _needlePackage;

        public MustReferencePackageAssemblyConventionSpecification(string needlePackage)
        {
            _needlePackage = needlePackage;
        }

        protected override ConventionResult IsSatisfiedBy(string assemblyName, XDocument projectDocument)
        {
            if (GetPackageReferences(projectDocument).Contains(_needlePackage))
            {
                return ConventionResult.Satisfied(assemblyName);
            }

            return ConventionResult.NotSatisfied(assemblyName, string.Format(FailureMessage, assemblyName));
        }

        protected override string FailureMessage => "{0} should reference package " + _needlePackage;
    }
}