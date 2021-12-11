using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nova.CodeAnalysis
{
    public sealed class SyntaxTree
    {
        public SyntaxTree(IEnumerable<string> diagnostics, Expr root, Token endOfFileToken)
        {
            Diagnostics = diagnostics.ToArray();
            Root = root;
            EndOfFileToken = endOfFileToken;
        }

        public static SyntaxTree Parse(string text)
        {
            NovaParser parser = new NovaParser(text);
            return parser.Parse();
        }

        public IReadOnlyList<string> Diagnostics { get; }
        public Expr Root { get; }
        public Token EndOfFileToken { get; }
    }
}
