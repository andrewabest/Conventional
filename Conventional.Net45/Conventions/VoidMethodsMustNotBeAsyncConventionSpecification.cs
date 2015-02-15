using System;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

// ReSharper disable once CheckNamespace
namespace Conventional.Conventions
{
    public class VoidMethodsMustNotBeAsyncConventionSpecification : ConventionSpecification
    {
        protected override string FailureMessage
        {
            get { return "All void methods must not be async"; }
        }

        public override ConventionResult IsSatisfiedBy(Type type)
        {
            var toInspect = type.GetMethods().Where(m => m.ReturnType == typeof (void));

            var failures = toInspect.Where(HasAttribute<AsyncStateMachineAttribute>);

            if (Enumerable.Any<MethodInfo>(failures))
            {
                var failureMessage =
                    BuildFailureMessage(Enumerable.Aggregate<MethodInfo, string>(failures, string.Empty,
                        (s, info) => s + "\t- " + info.Name + Environment.NewLine));

                return ConventionResult.NotSatisfied(type.FullName, failureMessage);
            }

            return ConventionResult.Satisfied(type.FullName);
        }

        private static bool HasAttribute<TAttribute>(MethodInfo subject) where TAttribute : Attribute
        {
            return subject.GetCustomAttributes(typeof(TAttribute), false).Any();
        }
    }
}