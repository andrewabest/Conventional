using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace Conventional.Conventions.Assemblies
{
    public class MustBeIncludedInSetOfAssembliesConventionSpecification : AssemblyConventionSpecification
    {
        private readonly ISet<string> _assemblyNames;
        private readonly string _assemblySetName;

        public MustBeIncludedInSetOfAssembliesConventionSpecification(IEnumerable<Assembly> assemblyNames, string assemblySetName)
        {
            _assemblyNames = new HashSet<string>(assemblyNames.Select(assembly => assembly.GetName().Name));
            _assemblySetName = assemblySetName;
        }

        protected override ConventionResult IsSatisfiedByLegacyCsprojFormat(string assemblyName, XDocument projectDocument)
        {
            return IsSatisfiedBy(assemblyName, projectDocument);
        }

        protected override ConventionResult IsSatisfiedBy(string assemblyName, XDocument projectDocument)
        {
            if (_assemblyNames.Contains(assemblyName))
            {
                return ConventionResult.Satisfied(assemblyName);
            }

            return ConventionResult.NotSatisfied(assemblyName, string.Format(FailureMessage, assemblyName));
        }

        protected override string FailureMessage => "{0} is not included in " + _assemblySetName;
    }
}