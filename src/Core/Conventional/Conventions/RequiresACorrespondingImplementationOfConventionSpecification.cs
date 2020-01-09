using System;
using System.Linq;
using Conventional.Extensions;

namespace Conventional.Conventions
{
    public class RequiresACorrespondingImplementationOfConventionSpecification : ConventionSpecification
    {
        private readonly Type _required;
        private readonly Type[] _subjects;

        public RequiresACorrespondingImplementationOfConventionSpecification(Type required, Type[] subjects)
        {
            if (required.IsGenericTypeDefinition == false)
            {
                throw new ArgumentException("The required type must be a generic", nameof(required));
            }

            _required = required;
            _subjects = subjects;
        }

        protected override string FailureMessage => "Could not find required corresponding implementation of {0}";

        public override ConventionResult IsSatisfiedBy(Type type)
        {
            if (_subjects
                .Where(x => x.IsGenericImplementation())
                .Any(x =>  
                    (x.GetInterfaces().Any(i => i.IsGenericType && (i.GetGenericTypeDefinition() == _required.GetGenericTypeDefinition()) && i.GetGenericArguments().Any(g => g == type))) ||
                    (x.BaseType != null && x.BaseType.IsGenericType && x.BaseType.GetGenericTypeDefinition() == _required.GetGenericTypeDefinition() && x.BaseType.GetGenericArguments().Any(g => g == type))))
            {
                return ConventionResult.Satisfied(type.FullName);
            }

            return ConventionResult.NotSatisfied(type.FullName, FailureMessage.FormatWith(_required.FullName));
        }
    }


}