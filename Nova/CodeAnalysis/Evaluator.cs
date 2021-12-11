using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nova.CodeAnalysis
{
    public class Evaluator
    {
        private readonly Expr _root;

        public Evaluator(Expr root)
        {
            _root = root;
        }

        public float Evaluate()
        {
            return EvaluateExpr(_root);
        }

        private float EvaluateExpr(Expr node)
        {
            // Number Expressions - 1
            if (node is NumberExpr n)
                return (float)n.NumberToken.Value;

            // Binary Expressions - 1 + 2
            if (node is BinaryExpr b)
            {
                float right = EvaluateExpr(b.Left);
                float left = EvaluateExpr(b.Right);

                switch (b.OperatorToken.Type)
                {
                    case SyntaxKind.Plus:
                        return (float)left + right;
                    case SyntaxKind.Minus:
                        return (float)left - right;
                    case SyntaxKind.Asterisk:
                        return (float)left * right;
                    case SyntaxKind.Slash:
                        return (float)left / right;
                    default:
                        throw new Exception($"unexpected operator {b.OperatorToken.Type}");
                        
                }
            }

            // Parenthesized Expressions - (1 + 2)
            if (node is ParenthesizedExpr p)
                 return EvaluateExpr(p.Expr);

            throw new Exception($"Unexpected node {node.Type}");
        }
    }
}
