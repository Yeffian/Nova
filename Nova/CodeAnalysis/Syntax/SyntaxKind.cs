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
        DoubleAmpersand,
        DoublePipe,
        DoubleEquals,
        BangEquals,
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