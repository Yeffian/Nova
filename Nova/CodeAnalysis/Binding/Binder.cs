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
            switch (expr.Kind)
            {
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

        private BoundExpr BindNumberExpr(NumberExpr expr)
        {
            var value = expr.Value ?? 0;
            return new BoundNumberExpr(value);
        }

        private BoundExpr BindUnaryExpr(UnaryExpr expr)
        {
            BoundExpr boundOperand = BindExpr(expr.Operand);
            var boundOperatorKind = BindUnaryOperatorKind(expr.OperatorToken.Kind, boundOperand.Type);

            if (boundOperatorKind == null)
            {
                _diagnostics.Add($"[ERR] Unary operator '{expr.OperatorToken.Text}' is not defined for type {boundOperand.Type}");
                return boundOperand;
            }

            return new BoundUnaryExpr(boundOperatorKind.Value, boundOperand);
        }

        private BoundExpr BindBinaryExpr(BinaryExpr expr)
        {
            BoundExpr boundLeft = BindExpr(expr.Left);
            BoundExpr boundRight = BindExpr(expr.Right);
            var boundOperatorKind = BindBinaryOperatorKind(expr.OperatorToken.Kind, boundLeft.Type, boundRight.Type);
            
            if (boundOperatorKind == null)
            {
                _diagnostics.Add($"[ERR] Binary operator '{expr.OperatorToken.Text}' is not defined for types {boundLeft.Type} and {boundRight.Type}");
                return boundLeft;
            }
            
            return new BoundBinaryExpr(boundLeft, boundOperatorKind.Value, boundRight);
        }

        private BoundBinaryOperatorKind? BindBinaryOperatorKind(SyntaxKind kind, Type leftType, Type rightType)
        {
            if (leftType == typeof(int) && rightType == typeof(int))
            {
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
                }
            }

            if (leftType == typeof(bool) && rightType == typeof(bool))
            {
                switch (kind)
                {
                    case SyntaxKind.DoubleAmpersand:
                        return BoundBinaryOperatorKind.LogicalAnd;
                    case SyntaxKind.DoublePipe:
                        return BoundBinaryOperatorKind.LogicalOr;
                }
            }

            return null;
        }
        
        private BoundUnaryOperatorKind? BindUnaryOperatorKind(SyntaxKind kind, Type operandType)
        {
            if (operandType == typeof(int))
            {
                switch (kind)
                {
                    case SyntaxKind.Plus:
                        return BoundUnaryOperatorKind.Identity;
                    case SyntaxKind.Minus:
                        return BoundUnaryOperatorKind.Negation;
                }
            }

            if (operandType == typeof(bool))
            {
                switch (kind)
                {
                    case SyntaxKind.Bang:
                        return BoundUnaryOperatorKind.LogicalNot;
                }
            }

            return null;
        }
    }
}
