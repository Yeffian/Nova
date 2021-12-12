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
        public NumberExpr(Token numberToken, object value)
        {
            NumberToken = numberToken;
            Value = value;
        }
        
        public NumberExpr(Token numberToken) : this(numberToken, numberToken.Value) { }

        public override SyntaxKind Kind => SyntaxKind.NumberExpr;
        public Token NumberToken { get; }
        public object Value { get; }

        public override IEnumerable<Node> GetChildren()
        {
            yield return NumberToken;
        }
    }
}
