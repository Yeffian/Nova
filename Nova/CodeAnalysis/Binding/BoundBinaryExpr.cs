namespace Nova.CodeAnalysis.Binding
{
    internal sealed class BoundBinaryExpr : BoundExpr
    {
        public BoundBinaryExpr(BoundExpr left, BoundBinaryOperator op, BoundExpr right)
        {
            Left = left;
            Op = op;
            Right = right;
        }
    
        public BoundExpr Left { get; }
        public BoundBinaryOperator Op { get; }
        public BoundExpr Right { get; }

        public override BoundNodeKind Kind => BoundNodeKind.BinaryExpr;
        public override Type Type => Op.ResultType;
    }
}

