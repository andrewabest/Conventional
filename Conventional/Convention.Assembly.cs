using System.Text.RegularExpressions;
using Conventional.Conventions.Assemblies;

namespace Conventional
{
    public static partial class Convention
    {
        public static MustNotReferenceDllsFromBinOrObjDirectoriesConventionSpecification MustNotReferenceDllsFromBinOrObjDirectories
        {
            get {  return new MustNotReferenceDllsFromBinOrObjDirectoriesConventionSpecification(); }
        }

        public static MustHaveAllFilesBeEmbeddedResourcesConventionSpecification MustHaveFilesBeEmbeddedResources(string fileExtension)
        {
           return new MustHaveAllFilesBeEmbeddedResourcesConventionSpecification(fileExtension);
        }

        public static MustHaveAllFilesBeEmbeddedResourcesConventionSpecification MustHaveFilesBeEmbeddedResources(Regex fileMatchRegex)
        {
            return new MustHaveAllFilesBeEmbeddedResourcesConventionSpecification(fileMatchRegex);
        }
    }
}