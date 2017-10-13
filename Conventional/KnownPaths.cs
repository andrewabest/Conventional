using System;
using System.IO;
using System.Linq;

namespace Conventional
{
    public static class KnownPaths
    {
        private static readonly Func<string, string> DefaultSolutionRootFinder = x => x.Substring(0, x.LastIndexOf("\\bin\\", StringComparison.Ordinal));
        private static readonly string DefaultSolutionRoot = Path.GetFullPath(Path.Combine(SolutionRootFinder(AppContext.BaseDirectory), @"..\"));
        private static readonly Func<string> DefaultPathToSolutionRoot = () => Directory.GetFiles(SolutionRoot, "*.sln", SearchOption.AllDirectories).FirstOrDefault();

        private static Func<string, string> _solutionRootFinder;
        public static Func<string, string> SolutionRootFinder
        {
            get => _solutionRootFinder ?? DefaultSolutionRootFinder;
            set => _solutionRootFinder = value;
        }

        private static string _solutionRoot;
        public static string SolutionRoot
        {
            get => _solutionRoot ?? DefaultSolutionRoot;
            set
            {
                if (value.EndsWith(@"\") == false)
                {
                    value += @"\";
                }

                _solutionRoot = value;
            }
        }

        private static string _fullPathToSolution;
        public static string FullPathToSolution
        {
            get => _fullPathToSolution ?? DefaultPathToSolutionRoot();
            set => _fullPathToSolution = value;
        }
    }
}