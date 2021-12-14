using Nova.CodeAnalysis.Syntax;

namespace Nova.CodeAnalysis.Binding
{
    internal sealed class BoundBinaryOperator
    {


        public BoundBinaryOperator(SyntaxKind syntaxKind, BoundBinaryOperatorKind kind, Type leftType, Type rightType,
            Type resultType)
        {
            SyntaxKind = syntaxKind;
            Kind = kind;
            LeftType = leftType;
            RightType = rightType;
            ResultType = resultType;
        }

        public BoundBinaryOperator(SyntaxKind syntaxKind, BoundBinaryOperatorKind kind, Type type) 
            : this(syntaxKind, kind, type, type, type) { }

        public BoundBinaryOperator(SyntaxKind syntaxKind, BoundBinaryOperatorKind kind, Type operandType, Type resultType) 
            : this(syntaxKind, kind, operandType, operandType, resultType) { }

        public SyntaxKind SyntaxKind { get; }
        public BoundBinaryOperatorKind Kind { get; }
        public Type LeftType { get; }
        public Type RightType { get; }
        public Type ResultType { get; }

        private static BoundBinaryOperator[] _operators =
        {
            // + and -
            new BoundBinaryOperator(SyntaxKind.Plus, BoundBinaryOperatorKind.Addition, typeof(int)),
            new BoundBinaryOperator(SyntaxKind.Minus, BoundBinaryOperatorKind.Subtraction, typeof(int)),
            
            // * and /
            new BoundBinaryOperator(SyntaxKind.Asterisk, BoundBinaryOperatorKind.Multiplication, typeof(int)),
            new BoundBinaryOperator(SyntaxKind.Slash, BoundBinaryOperatorKind.Division, typeof(int)),
            
            // && and ||
            new BoundBinaryOperator(SyntaxKind.DoubleAmpersand, BoundBinaryOperatorKind.LogicalAnd, typeof(bool)),
            new BoundBinaryOperator(SyntaxKind.DoublePipe, BoundBinaryOperatorKind.LogicalOr, typeof(bool)),
            
            // == for booleans and integers
            new BoundBinaryOperator(SyntaxKind.DoubleEquals, BoundBinaryOperatorKind.Equality, typeof(int), typeof(bool)),
            new BoundBinaryOperator(SyntaxKind.DoubleEquals, BoundBinaryOperatorKind.Equality, typeof(bool)),
            
            // greater than and less than
            new BoundBinaryOperator(SyntaxKind.GreaterThan, BoundBinaryOperatorKind.GreaterThan, typeof(int), typeof(bool)),
            new BoundBinaryOperator(SyntaxKind.LessThan, BoundBinaryOperatorKind.LessThan, typeof(int), typeof(bool)),
            
            new BoundBinaryOperator(SyntaxKind.Caret, BoundBinaryOperatorKind.Exponent, typeof(int), typeof(bool))
        };

        public static BoundBinaryOperator Bind(SyntaxKind syntaxKind, Type leftType, Type rightType)
        {
            foreach (BoundBinaryOperator op in _operators)
            {
                if (op.SyntaxKind == syntaxKind && op.LeftType == leftType && op.RightType == rightType)
                    return op;
            }

            return null;
        }
    }
}

