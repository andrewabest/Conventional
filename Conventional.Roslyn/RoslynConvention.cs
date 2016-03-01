using Conventional.Roslyn.Conventions;

namespace Conventional.Roslyn
{
    public static class RoslynConvention
    {
        public static IfAndElseMustHaveBracesConventionSpecification IfAndElseMustHaveBraces()
        {
            return new IfAndElseMustHaveBracesConventionSpecification();
        }
    }
}
