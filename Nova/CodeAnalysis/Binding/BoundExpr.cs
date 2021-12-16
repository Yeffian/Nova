using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nova.CodeAnalysis.Binding
{
    public abstract class BoundExpr : BoundNode
    {
        public abstract Type Type { get; }
    }
}
