using Nova.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nova.CodeAnalysis.Binding;

namespace Nova.CodeAnalysis
{
    public class Evaluator
    {
        private readonly BoundExpr _root;

        public Evaluator(BoundExpr root)
        {
            _root = root;
        }

        public object Evaluate()
        {
            return EvaluateExpr(_root);
        }

        private object EvaluateExpr(BoundExpr node)
        {
            // Number Expressions - 1
            if (node is BoundNumberExpr n)
                return n.Value;

            if (node is BoundUnaryExpr u)
            {
                var operand = EvaluateExpr(u.Operand);

                switch (u.Op.Kind)
                {
                    case BoundUnaryOperatorKind.Identity:
                        return (int) operand;
                    case BoundUnaryOperatorKind.Negation:
                        return -(int) operand;
                    case BoundUnaryOperatorKind.LogicalNot:
                        return !(bool) operand;
                    default:
                        throw new Exception($"Unexpected unary operator {u.Op}");
                }
            }

            // Binary Expressions - 1 + 2
            if (node is BoundBinaryExpr b)
            {
                var right = EvaluateExpr(b.Left);
                var left = EvaluateExpr(b.Right);

                switch (b.Op.Kind)
                {
                    case BoundBinaryOperatorKind.Addition:
                        return (int) left + (int) right;
                    case BoundBinaryOperatorKind.Subtraction:
                        return (int) left - (int) right;
                    case BoundBinaryOperatorKind.Multiplication:
                        return (int) left * (int) right;
                    case BoundBinaryOperatorKind.Division:
                        return (int) left / (int) right;
                    case BoundBinaryOperatorKind.GreaterThan:
                        return (int) left > (int) right;
                    case BoundBinaryOperatorKind.LessThan:
                        return (int) left < (int) right;
                    case BoundBinaryOperatorKind.LogicalAnd:
                        return (bool) left && (bool) right;
                    case BoundBinaryOperatorKind.LogicalOr:
                        return (bool) left || (bool) right;
                    case BoundBinaryOperatorKind.Equality:
                        return Equals(left, right);
                    case BoundBinaryOperatorKind.Exponent:
                        return Math.Pow((int) right, (int) left);
                    default:
                        throw new Exception($"unexpected operator {b.Op}");
                        
                }
            }

            throw new Exception($"Unexpected node {node.Type}");
        }
    }
}
