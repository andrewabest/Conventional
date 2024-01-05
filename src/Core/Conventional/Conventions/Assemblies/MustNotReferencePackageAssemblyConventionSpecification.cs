using System.Linq;
using System.Xml.Linq;

namespace Conventional.Conventions.Assemblies
{
    public class MustNotReferencePackageAssemblyConventionSpecification : PackageReferenceAssemblyConventionSpecification
    {
        private readonly string _needlePackage;

        public MustNotReferencePackageAssemblyConventionSpecification(string needlePackage)
        {
            _needlePackage = needlePackage;
        }

        protected override ConventionResult IsSatisfiedBy(string assemblyName, XDocument projectDocument)
        {
            if (GetPackageReferences(projectDocument).Contains(_needlePackage))
            {
                return ConventionResult.NotSatisfied(assemblyName, string.Format(FailureMessage, assemblyName));
            }

            return ConventionResult.Satisfied(assemblyName);
        }

        protected override string FailureMessage => "{0} should not reference package " + _needlePackage;
    }
}