namespace Nova.CodeAnalysis.Binding
{
    public sealed class BoundNumberExpr : BoundExpr
    {
    
        public BoundNumberExpr(object value)
        {
            Value = value;
        }
    
        public object Value { get; }

        public override BoundNodeKind Kind => BoundNodeKind.NumberExpr;
        public override Type Type => Value.GetType();
    }
}

