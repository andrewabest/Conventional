using System.IO;
using System.Linq;
using Conventional.Conventions.Assemblies;

namespace Conventional
{
    public static class TheAssembly
    {
        public static AssemblySpecimen WithNameMatching(string pattern)
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

            if (candidates.Length > 1)
            {
                throw new ConventionException("More than one project file was found matching the pattern {0}".FormatWith(pattern));
            }

            return new AssemblySpecimen(candidates[0]);

        }
    }
}