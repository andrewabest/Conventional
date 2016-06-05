namespace Conventional.Conventions.Database
{
    public class AllNamedColumnsMustBeNonNullable : DatabaseConventionSpecification
    {
        private readonly string _columnName;

        public AllNamedColumnsMustBeNonNullable(string columnName)
        {
            _columnName = columnName;
        }

        protected override string GetScript()
        {
            return base.GetScript().Replace("{COLUMN_NAME}", _columnName);
        }

        protected override string FailureMessage { get { return $"{{TableName}}.{_columnName} must be nullable"; } }
    }
}