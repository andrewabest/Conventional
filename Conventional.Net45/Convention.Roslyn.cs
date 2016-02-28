using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Conventional.Conventions.Roslyn;

namespace Conventional
{
    public static partial class Convention
    {
        public static IfAndElseMustHaveBracesConventionSpecification IfAndElseMustHaveBraces()
        {
            return new IfAndElseMustHaveBracesConventionSpecification();
        }
    }
}
