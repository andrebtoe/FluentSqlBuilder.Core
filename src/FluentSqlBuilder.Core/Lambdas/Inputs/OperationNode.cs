using System.Linq.Expressions;

namespace SqlBuilderFluent.Lambdas.Inputs
{
    internal class OperationNode : Node
    {
        public OperationNode(OperationNodeType operationNodeType, ExpressionType @operator, Node left, Node right, OperationNodeResolveType? operationNodeResolveType)
        {
            OperationNodeType = operationNodeType;
            Operator = @operator;
            Left = left;
            Right = right;
            OperationNodeResolveType = operationNodeResolveType;
        }

        public OperationNodeType OperationNodeType { get; private set; }
        public OperationNodeResolveType? OperationNodeResolveType { get; private set; }
        public ExpressionType Operator { get; private set; }
        public Node Left { get; private set; }
        public Node Right { get; private set; }
    }

    internal enum OperationNodeType
    {
        Common = 1,
        ColumnWithFunction = 2
    }

    public enum OperationNodeResolveType
    {
        DateTimeWithYear = 1
    }
}