using System;

namespace Conventional.Roslyn.Tests
{
    public class TestSolution : IDisposable
    {
        private readonly string _previousSolutionRoot;

        public TestSolution(string extendedPath)
        {
            _previousSolutionRoot = KnownPaths.SolutionRoot;

            KnownPaths.SolutionRoot += extendedPath;
        }

        public void Dispose()
        {
            KnownPaths.SolutionRoot = _previousSolutionRoot;
        }
    }
}