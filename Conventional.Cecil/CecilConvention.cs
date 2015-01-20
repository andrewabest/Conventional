using Conventional.Cecil.Conventions;

namespace Conventional.Cecil
{
    public static class CecilConvention
    {
        public static MustNotUseDateTimeNowConventionSpecification MustNotUseDateTimeNow
        {
            get { return new MustNotUseDateTimeNowConventionSpecification(); }
        }
        
        public static MustNotUseDateTimeOffsetNowConventionSpecification MustNotUseDateTimeOffsetNow
        {
            get { return new MustNotUseDateTimeOffsetNowConventionSpecification(); }
        }
    }
}