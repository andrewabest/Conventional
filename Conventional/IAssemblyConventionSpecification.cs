using System.Reflection;

namespace Conventional
{
    public interface IAssemblyConventionSpecification
    {
        ConventionResult IsSatisfiedBy(string projectFilePath);
        ConventionResult IsSatisfiedBy(Assembly assembly);
    }
}