using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nova.CodeAnalysis.Binding
{
    internal abstract class BoundExpr : BoundNode
    {
        public abstract Type Type { get; }
    }
}
