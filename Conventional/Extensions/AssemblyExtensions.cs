using System.IO;
using System.Linq;
using System.Reflection;
using Conventional.Conventions;

namespace Conventional.Extensions
{
    public static class AssemblyExtensions
    {
        public static string ResolveProjectFilePath(this Assembly assembly)
        {
            var projectFile = assembly.GetName().Name + ".csproj";

            var candidates = Directory.GetFiles(KnownPaths.SolutionRoot, "*" + projectFile, SearchOption.AllDirectories);

            if (candidates.Any() == false)
            {
                throw new ConventionException("Could not locate a project file with the name {0}".FormatWith(projectFile));
            }

            if (candidates.Length > 1)
            {
                throw new ConventionException("More than one project file was located with the name {0}".FormatWith(projectFile));
            }

            return candidates[0];
        }
    }
}
