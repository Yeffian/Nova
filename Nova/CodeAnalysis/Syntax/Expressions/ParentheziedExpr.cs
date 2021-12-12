using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nova.CodeAnalysis.Syntax
{

    // Basically anything in parenthesis, such as
    // (1 + 2), (3 * 2 + 7), etc
    internal class ParenthesizedExpr : Expr
    { 
        public ParenthesizedExpr(Token openParenToken, Expr expr, Token closeParenToken)
        {
            OpenParenToken = openParenToken;
            Expr = expr;
            CloseParenToken = closeParenToken;
        }

        public override SyntaxKind Type => SyntaxKind.ParenthesizedExpr;

        public Token OpenParenToken { get; }
        public Expr Expr { get; }
        public Token CloseParenToken { get; }

        public override IEnumerable<Node> GetChildren()
        {
            yield return OpenParenToken;
            yield return Expr;
            yield return CloseParenToken;
        }
    }
}
