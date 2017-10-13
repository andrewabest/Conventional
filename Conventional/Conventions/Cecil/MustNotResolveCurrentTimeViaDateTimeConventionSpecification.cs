using System;
using System.Linq.Expressions;

namespace Conventional.Conventions.Cecil
{
    public class MustNotResolveCurrentTimeViaDateTimeConventionSpecification : MustNotUsePropertyGetterSpecification<DateTime, DateTime>
    {
        public MustNotResolveCurrentTimeViaDateTimeConventionSpecification()
            : base(new Expression<Func<DateTime, DateTime>>[]
            {
                x=>DateTime.Now,
                x=>DateTime.UtcNow,
                x=>DateTime.Today
            }, "Must not use DateTime directly. It is used {0} times in {1}. ")
        {

        }
    }
}