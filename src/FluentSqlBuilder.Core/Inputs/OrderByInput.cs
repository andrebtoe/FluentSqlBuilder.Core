namespace SqlBuilderFluent.Core.Inputs
{
    public class OrderByInput
    {
        public OrderByInput(string columnName, bool descending)
        {
            ColumnName = columnName;
            Descending = descending;
        }

        public string ColumnName { get; private set; }
        public bool Descending { get; private set; }
    }
}