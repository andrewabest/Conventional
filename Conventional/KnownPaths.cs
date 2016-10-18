using System;
using System.IO;
using System.Linq;

namespace Conventional
{
    public static class KnownPaths
    {
        private static readonly string DefaultSolutionRoot = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, @"..\..\..\"));

        private static string _solutionRoot;
        public static string SolutionRoot
        {
            get { return _solutionRoot ?? DefaultSolutionRoot; }
            set
            {
                if (value.EndsWith(@"\") == false)
                {
                    value += @"\";
                }

                _solutionRoot = value;
            }
        }

        private static string DefaultPathToSolution()
        {
            return Directory.GetFiles(SolutionRoot, "*.sln", SearchOption.AllDirectories).FirstOrDefault();
        }

        private static string _fullPathToSolution;
        public static string FullPathToSolution
        {
            get { return _fullPathToSolution ?? DefaultPathToSolution(); }
            set { _fullPathToSolution = value; }
        }
    }
}