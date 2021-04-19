namespace SqlBuilderFluent.Lambdas.Inputs
{
    internal class LikeNode : Node
    {
        public LikeNode(LikeMethod method, MemberNode memberNode, string value)
        {
            Method = method;
            MemberNode = memberNode;
            Value = value;
        }

        public LikeMethod Method { get; private set; }
        public MemberNode MemberNode { get; private set; }
        public string Value { get; private set; }
    }
}