using System.IO;

namespace Conventional
{
    public static class KnownPaths
    {
        private static readonly string DefaultSolutionRoot = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), @"../../../"));

        private static string _solutionRoot;
        public static string SolutionRoot
        {
            get { return _solutionRoot ?? DefaultSolutionRoot; }
            set { _solutionRoot = value; }
        }
    }
}