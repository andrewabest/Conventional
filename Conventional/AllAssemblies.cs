using System.IO;
using System.Linq;
using Conventional.Conventions.Assemblies;

namespace Conventional
{
    public static class AllAssemblies
    {
        public static AssemblySpecimen[] WithNamesMatching(string pattern)
        {
            if (pattern.EndsWith(".csproj") == false)
            {
                pattern += ".csproj";
            }

            var candidates = Directory.GetFiles(KnownPaths.SolutionRoot, pattern, SearchOption.AllDirectories);

            if (candidates.Any() == false)
            {
                throw new ConventionException("Could not locate a project file matching the pattern {0}".FormatWith(pattern));
            }

            return candidates.Select(x => new AssemblySpecimen(x)).ToArray();
        }
    }
}