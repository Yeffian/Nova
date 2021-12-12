using Nova.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nova.CodeAnalysis.Binding;

namespace Nova.CodeAnalysis
{
    internal class Evaluator
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
                int operand = (int) EvaluateExpr(u.Operand);

                if (u.OperatorKind == BoundUnaryOperatorKind.Identity)
                    return +operand;
                else if (u.OperatorKind == BoundUnaryOperatorKind.Negation)
                    return -operand;
                else
                    throw new Exception($"Unexpected unary operator {u.OperatorKind}");
            }

            // Binary Expressions - 1 + 2
            if (node is BoundBinaryExpr b)
            {
                int right = (int) EvaluateExpr(b.Left);
                int left = (int) EvaluateExpr(b.Right);

                switch (b.OperatorKind)
                {
                    case BoundBinaryOperatorKind.Addition:
                        return left + right;
                    case BoundBinaryOperatorKind.Subtraction:
                        return left - right;
                    case BoundBinaryOperatorKind.Multiplication:
                        return left * right;
                    case BoundBinaryOperatorKind.Division:
                        return left / right;
                    default:
                        throw new Exception($"unexpected operator {b.OperatorKind}");
                        
                }
            }

            throw new Exception($"Unexpected node {node.Type}");
        }
    }
}
