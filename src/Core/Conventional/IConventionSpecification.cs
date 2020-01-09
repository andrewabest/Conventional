using System;

namespace Conventional
{
    public interface IConventionSpecification
    {
        ConventionResult IsSatisfiedBy(Type type);
        IConventionSpecification And(IConventionSpecification conventionSpecification);
        IConventionSpecification Or(IConventionSpecification conventionSpecification);
        IConventionSpecification Not();
    }
}