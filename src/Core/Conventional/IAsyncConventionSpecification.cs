using System;
using System.Threading.Tasks;

namespace Conventional
{
    public interface IAsyncConventionSpecification
    {
        Task<ConventionResult> IsSatisfiedBy(Type type);
    }
}