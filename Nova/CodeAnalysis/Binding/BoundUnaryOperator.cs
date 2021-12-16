using Nova.CodeAnalysis.Syntax;

namespace Nova.CodeAnalysis.Binding
{
    public sealed class BoundUnaryOperator
    {
        public BoundUnaryOperator(SyntaxKind syntaxKind, BoundUnaryOperatorKind kind, Type operandType, Type resultType)
        {
            SyntaxKind = syntaxKind;
            Kind = kind;
            OperandType = operandType;
            ResultType = resultType;
        }
        
        public BoundUnaryOperator(SyntaxKind syntaxKind, BoundUnaryOperatorKind kind, Type operandType) :this(syntaxKind, kind, operandType, operandType) { }

        public SyntaxKind SyntaxKind { get; }
        public BoundUnaryOperatorKind Kind { get; }
        public Type OperandType { get; }
        public Type ResultType { get; }

        private static BoundUnaryOperator[] _operators =
        {
            new BoundUnaryOperator(SyntaxKind.Bang, BoundUnaryOperatorKind.LogicalNot, typeof(bool)),
            new BoundUnaryOperator(SyntaxKind.Plus, BoundUnaryOperatorKind.Identity, typeof(int)),
            new BoundUnaryOperator(SyntaxKind.Minus, BoundUnaryOperatorKind.Negation, typeof(int)),
        };

        public static BoundUnaryOperator Bind(SyntaxKind syntaxKind, Type operandType)
        {
            foreach (BoundUnaryOperator op in _operators)
            {
                if (op.SyntaxKind == syntaxKind && op.OperandType == operandType)
                    return op;
            }

            return null;
        }
    }
}

