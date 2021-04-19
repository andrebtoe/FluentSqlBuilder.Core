using System.Linq.Expressions;

namespace SqlBuilderFluent.Lambdas.Inputs
{
    internal class SingleOperationNode : Node
    {
        public SingleOperationNode(ExpressionType @operator, Node child)
        {
            Operator = @operator;
            Child = child;
        }

        public ExpressionType Operator { get; private set; }
        public Node Child { get; private set; }
    }
}