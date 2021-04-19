using System.Linq.Expressions;

namespace SqlBuilderFluent.Lambdas.Inputs
{
    internal class OperationNode : Node
    {
        public OperationNode(ExpressionType @operator, Node left, Node right)
        {
            Operator = @operator;
            Left = left;
            Right = right;
        }

        public ExpressionType Operator { get; private set; }
        public Node Left { get; private set; }
        public Node Right { get; private set; }
    }
}