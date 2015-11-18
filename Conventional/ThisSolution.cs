using System.IO;

namespace Conventional
{
    public static class ThisSolution
    {
        public static ConventionResult MustConformTo(ISolutionConventionSpecification solutionConventionSpecification)
        {
            return Conformist.EnforceConformance(
                solutionConventionSpecification.IsSatisfiedBy(KnownPaths.SolutionRoot));
        }
    }
}