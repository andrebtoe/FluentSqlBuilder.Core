namespace SqlBuilderFluent.Lambdas.Inputs
{
    internal class ValueNode : Node
    {
        public ValueNode(object value)
        {
            Value = value;
        }

        public object Value { get; private set; }
    }
}