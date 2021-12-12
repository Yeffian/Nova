using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nova.CodeAnalysis.Syntax
{
    public abstract class Node
    {
        public abstract SyntaxKind Type { get; }

        public abstract IEnumerable<Node> GetChildren(); 
    }
}
