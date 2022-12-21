using System;
using System.Linq.Expressions;

namespace Conventional.Conventions.Cecil
{
    public class MustNotUseDateTimeOffsetNowConventionSpecification : MustNotUsePropertyGetterSpecification<DateTimeOffset, DateTimeOffset>
    {
        public MustNotUseDateTimeOffsetNowConventionSpecification()
            : base(new Expression<Func<DateTimeOffset, DateTimeOffset>>[] { x => DateTimeOffset.Now, x=>DateTimeOffset.UtcNow})
        {
        }
    }
}