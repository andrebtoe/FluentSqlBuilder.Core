namespace SqlBuilderFluent.Lambdas.Inputs
{
    internal class MemberNode : Node
    {
        public MemberNode(string tableName, string columnName)
        {
            TableName = tableName;
            ColumnName = columnName;
        }

        public string TableName { get; private set; }
        public string ColumnName { get; private set; }
    }
}