﻿using Conventional.Roslyn.Conventions;

namespace Conventional.Roslyn
{
    public static class RoslynConvention
    {
        public static IfAndElseMustHaveBracesConventionSpecification IfAndElseMustHaveBraces()
        {
            return new IfAndElseMustHaveBracesConventionSpecification(new[] { ".Designer.cs" });
        }

        public static UsingsStatementsMustNotBeNestedConventionSpecification UsingStatementsMustNotBeNested()
        {
            return new UsingsStatementsMustNotBeNestedConventionSpecification(new[] { ".Designer.cs" });
        }
    }
}
