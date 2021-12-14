namespace Nova.CodeAnalysis.Syntax
{
    public enum SyntaxKind
    {
        // Tokens
        Number,
        Whitespace,
        Plus,
        Minus,
        Asterisk,
        Slash,
        Bang,
        GreaterThan,
        LessThan,
        DoubleAmpersand,
        DoublePipe,
        DoubleEquals,
        OpenParen,
        CloseParen,
        EndOfFile,
        Unknown,

        // Keywords
        True,
        False,
        Identifier,

        // Expressions
        NumberExpr,
        BinaryExpr,
        ParenthesizedExpr,
        UnaryExpr,
    }
}