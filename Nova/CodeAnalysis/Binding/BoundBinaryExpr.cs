namespace Nova.CodeAnalysis.Binding;

internal sealed class BoundBinaryExpr : BoundExpr
{
    public BoundBinaryExpr(BoundExpr left, BoundBinaryOperatorKind operatorKind, BoundExpr right)
    {
        Left = left;
        OperatorKind = operatorKind;
        Right = right;
    }
    
    public BoundExpr Left { get; }
    public BoundBinaryOperatorKind OperatorKind { get; }
    public BoundExpr Right { get; }

    public override BoundNodeKind Kind => BoundNodeKind.BinaryExpr;
    public override Type Type => Left.Type;
}