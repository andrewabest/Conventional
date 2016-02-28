using System;

namespace Conventional.Tests.Net45
{
    public class TestSolution : IDisposable
    {
        private readonly string _previousSolutionRoot;

        public TestSolution(string extendedPath)
        {
            _previousSolutionRoot = KnownPaths.SolutionRoot;

            KnownPaths.SolutionRoot = KnownPaths.SolutionRoot + "TestSolution";
        }

        public void Dispose()
        {
            KnownPaths.SolutionRoot = _previousSolutionRoot;
        }
    }
}