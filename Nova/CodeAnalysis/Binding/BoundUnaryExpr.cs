using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nova.CodeAnalysis.Binding
{
    internal class BoundUnaryExpr : BoundExpr
    {
        public BoundUnaryExpr(BoundUnaryOperatorKind operatorKind, BoundExpr operand)
        {
            OperatorKind = operatorKind;
            Operand = operand;
        }
        
        public BoundUnaryOperatorKind OperatorKind { get; }
        public BoundExpr Operand { get; }

        public override BoundNodeKind Kind => BoundNodeKind.UnaryExpr;
        public override Type Type => Operand.Type;
    }
}
