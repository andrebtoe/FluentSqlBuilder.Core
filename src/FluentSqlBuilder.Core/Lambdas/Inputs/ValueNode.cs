namespace SqlBuilderFluent.Lambdas.Inputs
{
    public class ValueNode : Node
    {
        public ValueNode(object value, bool literalValue)
        {
            Value = value;
            LiteralValue = literalValue;
        }

        public object Value { get; private set; }
        public bool LiteralValue { get; set; }
    }

    public class ValueMethodNode : Node
    {
        public ValueMethodNode(object value, bool literalValue)
        {
            Value = value;
            LiteralValue = literalValue;
        }

        public object Value { get; private set; }
        public bool LiteralValue { get; set; }
    }
}