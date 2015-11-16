using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Conventional.Conventions;
using Mono.Cecil;
using Mono.Cecil.Rocks;

namespace Conventional.Cecil.Conventions
{
    public class MustInstantiatePropertiesOfSpecifiedTypeInDefaultConstructorConventionSpecification : ConventionSpecification
    {
        private readonly Type[] _propertyTypes;

        public MustInstantiatePropertiesOfSpecifiedTypeInDefaultConstructorConventionSpecification(Type propertyType)
        {
            _propertyTypes = new [] { propertyType };
        }

        public MustInstantiatePropertiesOfSpecifiedTypeInDefaultConstructorConventionSpecification(Type[] propertyTypes)
        {
            _propertyTypes = propertyTypes;
        }

        protected override string FailureMessage
        {
            get { return "Properties of type {0} must be instantiated in the default constructor"; }
        }

        public override ConventionResult IsSatisfiedBy(Type type)
        {
            var typeDefinition = type.ToTypeDefinition();

            var subjects = _propertyTypes.SelectMany(x => typeDefinition.GetPropertiesOfType(x));

            var subjectPropertySetters = subjects
                .Where(p => p.SetMethod != null)
                .Select(p => p.SetMethod.Name)
                .ToArray();

            var defaultConstructor =
                typeDefinition
                    .GetConstructors()
                    .SingleOrDefault(c => c.Parameters.Any() == false && c.IsStatic == false);

            if (defaultConstructor == null)
            {
                return ConventionResult.Satisfied(type.FullName);
            };

            var setters =
                defaultConstructor
                    .Body
                    .Instructions
                    .Select(i => i.Operand)
                    .OfType<MethodDefinition>()
                    .Select(m => m.Name);

            if (setters.Count(subjectPropertySetters.Contains) == subjectPropertySetters.Count())
            {
                return ConventionResult.Satisfied(type.FullName);
            }

            var propertyTypeNames = _propertyTypes.Aggregate(string.Empty, (x, t) => t.Name + ", " + x).TrimEnd(' ', ',');

            return ConventionResult.NotSatisfied(type.FullName, FailureMessage.FormatWith(propertyTypeNames));
        }
    }
}