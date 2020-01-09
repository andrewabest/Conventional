using System;
using System.Collections.Generic;
using System.Linq;

namespace Conventional.Conventions.Database
{
    public class EachRowMustHaveACorrespondingEnumConventionSpecification : DatabaseConventionSpecification
    {
        private readonly string _columnName;
        private readonly Type _enumType;
        private readonly string _tableName;

        public EachRowMustHaveACorrespondingEnumConventionSpecification(string tableName, string columnName, Type enumType)
        {
            if (!enumType.IsEnum) { throw new ArgumentException($"{nameof(enumType)} must be enum");}

            _tableName = tableName;
            _columnName = columnName;
            _enumType = enumType;
        }

        protected override string GetScript()
        {
            var enumValues = string.Join(", ", GetEnumValues());

            return base.GetScript()
                .Replace("{ENUM_VALUES}", enumValues)
                .Replace("{COLUMN_NAME}", _columnName)
                .Replace("{TABLE_NAME}", _tableName);
        }

        protected virtual IEnumerable<string> GetEnumValues()
        {
            return _enumType.GetEnumValues().Cast<int>().Select(x => x.ToString());
        }

        protected override string FailureMessage => $"{_tableName}.{_columnName} = {{value}} is not present in {_enumType}";
    }
}