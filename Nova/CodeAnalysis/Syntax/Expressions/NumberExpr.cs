using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nova.CodeAnalysis.Syntax
{
    // Any number, such as
    // 21, 3, 17, etc
    public sealed class NumberExpr : Expr
    {
        public NumberExpr(Token numberToken)
        {
            NumberToken = numberToken;
        }
        
        public override SyntaxKind Type => SyntaxKind.NumberExpr;
        public Token NumberToken { get; }

        public override IEnumerable<Node> GetChildren()
        {
            yield return NumberToken;
        }
    }
}
