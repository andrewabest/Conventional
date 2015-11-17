using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Conventional.Conventions
{
    public static class TypeExtensions
    {
        public static bool IsGenericImplementation(this Type type)
        {
            return (type.BaseType != null && type.BaseType.IsGenericType) ||
                    type.GetInterfaces().Any(i => i.IsGenericType);
        }
    }


    public class RequiresACorrespondingImplementationOfConventionSpecification : ConventionSpecification
    {
        private readonly Type _required;
        private readonly Type[] _subjects;

        public RequiresACorrespondingImplementationOfConventionSpecification(Type required, Type[] subjects)
        {
            if (required.IsGenericTypeDefinition == false)
            {
                throw new ArgumentException("The required type must be a generic", "required");
            }

            _required = required;
            _subjects = subjects;
        }

        protected override string FailureMessage
        {
            get { return "Could not find required corresponding implementation of {0}"; }
        }

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