using System.IO;

namespace Conventional
{
    public static class ThisSolution
    {
        public static ConventionResult MustConformTo(ISolutionConventionSpecification solutionConventionSpecification)
        {
            var solutionRoot = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), @"../../../"));

            return solutionConventionSpecification.IsSatisfiedBy(solutionRoot);
        }
    }
}