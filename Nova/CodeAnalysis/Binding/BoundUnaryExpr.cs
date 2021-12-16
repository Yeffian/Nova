using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nova.CodeAnalysis.Binding
{
    public class BoundUnaryExpr : BoundExpr
    {
        public BoundUnaryExpr(BoundUnaryOperator op, BoundExpr operand)
        {
            Op = op;
            Operand = operand;
        }
        
        public BoundUnaryOperator Op { get; }
        public BoundExpr Operand { get; }

        public override BoundNodeKind Kind => BoundNodeKind.UnaryExpr;
        public override Type Type => Op.ResultType;
    }
}
