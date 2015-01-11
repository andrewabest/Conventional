using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Conventional
{
    public class WrappedConventionResult : IEnumerable<ConventionResult>
    {
        public WrappedConventionResult(IEnumerable<Type> types, IEnumerable<ConventionResult> results)
        {
            Types = types;
            Results = results;
        }

        public IEnumerable<Type> Types { get; private set; }
        public IEnumerable<ConventionResult> Results { get; private set; }
        public string[] Failures { get { return Results.Where(x => x.IsSatisfied == false).SelectMany(x => x.Failures).ToArray(); } }

        public IEnumerator<ConventionResult> GetEnumerator()
        {
            return Results.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}