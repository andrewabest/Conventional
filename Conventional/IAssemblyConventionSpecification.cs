using System.Reflection;

namespace Conventional.Conventions.Solution
{
    public interface IAssemblyConventionSpecification
    {
        ConventionResult IsSatisfiedBy(string projectFilePath);
        ConventionResult IsSatisfiedBy(Assembly assembly);
    }
}