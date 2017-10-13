namespace Conventional.Conventions.Database
{
    public class AllNamedColumnsMustBeNonNullableConventionSpecification : DatabaseConventionSpecification
    {
        private readonly string _columnName;

        public AllNamedColumnsMustBeNonNullableConventionSpecification(string columnName)
        {
            _columnName = columnName;
        }

        protected override string GetScript()
        {
            return base.GetScript().Replace("{COLUMN_NAME}", _columnName);
        }

        protected override string FailureMessage => $"{{TableName}}.{_columnName} must be nullable";
    }
}