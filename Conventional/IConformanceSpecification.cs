using System;

namespace Conventional
{
    public interface IConformanceSpecification
    {
        ConformanceResult IsSatisfiedBy(Type type);
    }
}