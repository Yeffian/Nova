using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nova.CodeAnalysis.Syntax
{
    // A binary expression consists of two expressions on the left and right, and one operator in the middle, such as
    // 1 + 2, (2 * 3) + (4 / 2)
    public sealed class BinaryExpr : Expr
    {
        public BinaryExpr(Expr left, Token operatorToken, Expr right)
        {
            Left = left;
            OperatorToken = operatorToken;
            Right = right;
        }

        public override SyntaxKind Type => SyntaxKind.BinaryExpr;

        public Expr Left { get; }
        public Token OperatorToken { get; }
        public Expr Right { get; }

        public override IEnumerable<Node> GetChildren()
        {
            yield return Left;
            yield return OperatorToken;
            yield return Right;
        }
    }
}
