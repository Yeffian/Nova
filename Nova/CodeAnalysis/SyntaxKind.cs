namespace Nova.CodeAnalysis
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
        OpenParen,
        CloseParen,
        EndOfFile,
        Unknown,

        // Expressions
        NumberExpr,
        BinaryExpr,
        ParenthesizedExpr,
        UnaryExpr
    }
}