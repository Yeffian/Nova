using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nova.CodeAnalysis.Syntax
{
    // Unary expressions are expressions with a leading operator, and an expression, such as
    // -1, -(2 * 2), etc
    public sealed class UnaryExpr : Expr
    {
        public UnaryExpr(Token operatorToken, Expr operand)
        {
            OperatorToken = operatorToken;
            Operand = operand;
        }

        public override SyntaxKind Kind => SyntaxKind.UnaryExpr;

        public Token OperatorToken { get; }
        public Expr Operand { get; }

        public override IEnumerable<Node> GetChildren()
        {
            yield return OperatorToken;
            yield return Operand;
        }
    }
}
