using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nova.CodeAnalysis.Syntax;

namespace Nova.CodeAnalysis.Binding
{
    internal sealed class Binder
    {
        private List<string> _diagnostics = new List<string>();

        public IEnumerable<string> Diagnostics => _diagnostics;

        public BoundExpr BindExpr(Expr expr)
        {
            switch (expr.Type)
            {
                case SyntaxKind.NumberExpr:
                    return BindNumberExpr((NumberExpr) expr);
                case SyntaxKind.UnaryExpr:
                    return BindUnaryExpr((UnaryExpr) expr);
                case SyntaxKind.BinaryExpr:
                    return BindBinaryExpr((BinaryExpr) expr);
                default:
                    throw new Exception($"Unexpected syntax {expr.Type}");
            }
        }

        private BoundExpr BindNumberExpr(NumberExpr expr)
        {
            var value = expr.NumberToken.Value as int? ?? 0;
            return new BoundNumberExpr(value);
        }

        private BoundExpr BindUnaryExpr(UnaryExpr expr)
        {
            BoundExpr boundOperand = BindExpr(expr.Operand);
            var boundOperatorKind = BindUnaryOperatorKind(expr.OperatorToken.Type, boundOperand.Type);

            if (boundOperatorKind == null)
            {
                _diagnostics.Add($"[ERR] Unary operator '{expr.OperatorToken.Text}' is not defined for type {boundOperand.Kind}");
                return boundOperand;
            }

            return new BoundUnaryExpr(boundOperatorKind.Value, boundOperand);
        }

        private BoundExpr BindBinaryExpr(BinaryExpr expr)
        {
            BoundExpr boundLeft = BindExpr(expr.Left);
            BoundExpr boundRight = BindExpr(expr.Right);
            var boundOperatorKind = BindBinaryOperatorKind(expr.OperatorToken.Type, boundLeft.Type, boundRight.Type);
            
            if (boundOperatorKind == null)
            {
                _diagnostics.Add($"[ERR] Binary operator '{expr.OperatorToken.Text}' is not defined for types {boundLeft.Kind} and {boundRight.Kind}");
                return boundLeft;
            }
            
            return new BoundBinaryExpr(boundLeft, boundOperatorKind.Value, boundRight);
        }

        private BoundBinaryOperatorKind? BindBinaryOperatorKind(SyntaxKind kind, Type leftType, Type rightType)
        {
            if (leftType != typeof(int) || rightType != typeof(int))
                return null;

            switch (kind)
            {
                case SyntaxKind.Plus:
                    return BoundBinaryOperatorKind.Addition;
                case SyntaxKind.Minus:
                    return BoundBinaryOperatorKind.Subtraction;
                case SyntaxKind.Asterisk:
                    return BoundBinaryOperatorKind.Multiplication;
                case SyntaxKind.Slash:
                    return BoundBinaryOperatorKind.Division;
                default:
                    throw new Exception($"Unexpected unary operator {kind}");
            }
        }
        
        private BoundUnaryOperatorKind? BindUnaryOperatorKind(SyntaxKind kind, Type operandType)
        {
            if (operandType != typeof(int))
                return null;

            switch (kind)
            {
                case SyntaxKind.Plus:
                    return BoundUnaryOperatorKind.Identity;
                case SyntaxKind.Minus:
                    return BoundUnaryOperatorKind.Negation;
                default:
                    throw new Exception($"Unexpected unary operator {kind}");
            }
        }
    }
}
