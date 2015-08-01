using Conventional.Conventions.Assemblies;

namespace Conventional
{
    public static partial class Convention
    {
        public static MustNotReferenceDllsFromBinOrObjDirectoriesConventionSpecification MustNotReferenceDllsFromBinOrObjDirectories
        {
            get {  return new MustNotReferenceDllsFromBinOrObjDirectoriesConventionSpecification(); }
        }
    }
}