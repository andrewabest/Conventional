using System.IO;

namespace Conventional
{
    public static class KnownPaths
    {
        public static string SolutionRoot
        {
            get { return Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), @"../../../")); }
        }
    }
}