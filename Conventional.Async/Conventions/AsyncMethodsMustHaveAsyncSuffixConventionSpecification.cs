using System;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

// ReSharper disable once CheckNamespace
namespace Conventional.Conventions
{
    public class AsyncMethodsMustHaveAsyncSuffixConventionSpecification : ConventionSpecification
    {
        protected override string FailureMessage
        {
            get { return "All async methods must be suffixed with 'Async'"; }
        }

        public override ConventionResult IsSatisfiedBy(Type type)
        {
            var toInspect = type.GetMethods();

            var failures = toInspect.Where(subject => HasAttribute<AsyncStateMachineAttribute>(subject) && 
                                                      !EndsWithAsyncSuffix(subject));

            if (failures.Any())
            {
                var details = failures.Aggregate(string.Empty, (s, info) => s + "\t- " + info.Name + Environment.NewLine);
                var failureMessage = BuildFailureMessage(details);

                return ConventionResult.NotSatisfied(type.FullName, failureMessage);
            }

            return ConventionResult.Satisfied(type.FullName);
        }

        private static bool HasAttribute<TAttribute>(MethodInfo subject) where TAttribute : Attribute
        {
            return subject.GetCustomAttributes(typeof(TAttribute), false).Any();
        }

        private static bool EndsWithAsyncSuffix(MethodInfo subject)
        {
            return subject.Name.EndsWith("Async");
        }
    }
}