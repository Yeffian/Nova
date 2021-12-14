using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nova.CodeAnalysis.Syntax
{
    internal static class SyntaxFacts
    {
        public static int GetBinaryOperatorPrecedence(this SyntaxKind kind)
        {
            switch (kind)
            {
                case SyntaxKind.Caret:
                    return 7;
                case SyntaxKind.Asterisk:
                case SyntaxKind.Slash:
                    return 6;
                case SyntaxKind.Plus:
                case SyntaxKind.Minus:
                    return 5;
                case SyntaxKind.DoubleEquals:
                    return 4;
                case SyntaxKind.GreaterThan:
                case SyntaxKind.LessThan:
                    return 3;
                case SyntaxKind.DoubleAmpersand:
                    return 2;
                case SyntaxKind.DoublePipe:
                    return 1;
                default:
                    return 0;
            }
        }

        public static int GetUnaryOperatorPrecedence(this SyntaxKind kind)
        {
            switch (kind)
            {
                case SyntaxKind.Plus:
                case SyntaxKind.Minus:
                case SyntaxKind.Bang:
                    return 8;
                default:
                    return 0;
            }
        }

        public static SyntaxKind GetKeywordKind(string text)
        {
            switch (text)
            {
                case "true":
                    return SyntaxKind.True;
                case "false":
                    return SyntaxKind.False;
                default:
                    return SyntaxKind.Identifier;
            }
        }
    }
}
