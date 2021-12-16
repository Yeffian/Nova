using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nova.CodeAnalysis.Syntax;

namespace Nova.CodeAnalysis.Binding
{
    public sealed class Binder
    {
        private List<string> _diagnostics = new List<string>();

        public IEnumerable<string> Diagnostics => _diagnostics;

        public BoundExpr BindExpr(Expr expr)
        {
            switch (expr.Kind)
            {
                case SyntaxKind.ParenthesizedExpr:
                    return BindParenthisizedExpr((ParenthesizedExpr) expr);
                case SyntaxKind.NumberExpr:
                    return BindNumberExpr((NumberExpr) expr);
                case SyntaxKind.UnaryExpr:
                    return BindUnaryExpr((UnaryExpr) expr);
                case SyntaxKind.BinaryExpr:
                    return BindBinaryExpr((BinaryExpr) expr);
                default:
                    throw new Exception($"Unexpected syntax {expr.Kind}");
            }
        }

        private BoundExpr BindParenthisizedExpr(ParenthesizedExpr expr) => BindExpr(expr.Expr);

        private BoundExpr BindNumberExpr(NumberExpr expr)
        {
            var value = expr.Value ?? 0;
            return new BoundNumberExpr(value);
        }

        private BoundExpr BindUnaryExpr(UnaryExpr expr)
        {
            BoundExpr boundOperand = BindExpr(expr.Operand);
            var boundOperatorKind = BoundUnaryOperator.Bind(expr.OperatorToken.Kind, boundOperand.Type);

            if (boundOperatorKind == null)
            {
                _diagnostics.Add($"[ERR] Unary operator '{expr.OperatorToken.Text}' is not defined for type {boundOperand.Type}");
                return boundOperand;
            }

            return new BoundUnaryExpr(boundOperatorKind, boundOperand);
        }

        private BoundExpr BindBinaryExpr(BinaryExpr expr)
        {
            BoundExpr boundLeft = BindExpr(expr.Left);
            BoundExpr boundRight = BindExpr(expr.Right);
            var boundOperator = BoundBinaryOperator.Bind(expr.OperatorToken.Kind, boundLeft.Type, boundRight.Type);
            
            if (boundOperator == null)
            {
                _diagnostics.Add($"[ERR] Binary operator '{expr.OperatorToken.Text}' is not defined for types {boundLeft.Type} and {boundRight.Type}");
                return boundLeft;
            }
            
            return new BoundBinaryExpr(boundLeft, boundOperator, boundRight);
        }
    }
}
