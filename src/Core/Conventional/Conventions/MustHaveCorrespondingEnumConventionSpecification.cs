using System;
using System.Collections.Generic;
using System.Linq;

namespace Conventional.Conventions
{
    public class MustHaveCorrespondingEnumConventionSpecification : ConventionSpecification
    {
        private readonly Type[] _sourceTypes;

        public MustHaveCorrespondingEnumConventionSpecification(IEnumerable<Type> sourceTypes)
        {
            _sourceTypes = sourceTypes.ToArray();
        }

        public MustHaveCorrespondingEnumConventionSpecification(params Type[] sourceTypes)
        {
            _sourceTypes = sourceTypes;
        }

        public override ConventionResult IsSatisfiedBy(Type type)
        {
            var sourceType = _sourceTypes.FirstOrDefault(t => t.IsEnum && t.Name == type.Name);
            if (sourceType == null)
            {
                return ConventionResult.NotSatisfied(type.Name, "does not match any of the supplied type names");
            }

            var subjectValueNames = ToValueNameDictionary(type);
            var sourceValueNames = ToValueNameDictionary(sourceType);

            ConventionResult result = ConventionResult.Satisfied(type.Name);
            result = CompareEnums(subjectValueNames, sourceValueNames, result);
            result = CompareEnums(sourceValueNames, subjectValueNames, result);

            return result;
        }

        private static ConventionResult CompareEnums(IDictionary<int, string> left, IDictionary<int, string> right, ConventionResult result)
        {
            foreach (var leftValue in left)
            {
                if (!right.ContainsKey(leftValue.Key))
                {
                    result = result.And(ConventionResult.NotSatisfied(leftValue.Value, string.Format("{0} ({1}) does not match any values", leftValue.Value, leftValue.Key)));
                }
                else if (right[leftValue.Key] != leftValue.Value)
                {
                    result = result.And(ConventionResult.NotSatisfied(leftValue.Value, string.Format("{0} ({1}) does not match names with the corresponding value", leftValue.Value, leftValue.Key)));
                }
            }
            return result;
        }

        private static IDictionary<int, string> ToValueNameDictionary(Type type)
        {
            return Enum.GetValues(type).Cast<object>().ToDictionary(
                v => (int)v,
                v => Enum.GetName(type, v)
                );
        }

        protected override string FailureMessage => "Must have a corresponding type";
    }
}