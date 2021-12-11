using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nova.CodeAnalysis
{
    internal static class SyntaxFacts
    {
        public static int GetBinaryOperatorPrecedence(this SyntaxKind kind)
        {
            switch (kind)
            {
                case SyntaxKind.Asterisk:
                case SyntaxKind.Slash:
                    return 2;
                case SyntaxKind.Plus:
                case SyntaxKind.Minus:
                    return 1;
                default:
                    return 0;
            }
        }
    }

    public class NovaParser
    {
        private readonly Token[] _tokens;
        private int _pos;

        private List<string> _diagnostics = new List<string>();

        public IEnumerable<string> Diagnostics => _diagnostics;

        public NovaParser(string source)
        {
            _pos = 0;

            NovaLexer lexer = new NovaLexer(source);
            List<Token> tokens = new List<Token>();

            Token token;
            do
            {
                token = lexer.NextToken();

                if (token.Type != SyntaxKind.Whitespace && token.Type != SyntaxKind.Unknown) tokens.Add(token);
            } while (token.Type != SyntaxKind.EndOfFile);

            _tokens = tokens.ToArray();
            
            // Get all the diagnostics from the lexer and add them to the parsers diagnostics as well
            _diagnostics.AddRange(lexer.Diagnostics);
        }

        private Token Peek(int offset)
        {
            int index = _pos - offset;

            if (index >= _tokens.Length)
                return _tokens[_tokens.Length - 1];

            return _tokens[index];
        }

        private Token Current => Peek(0);

        private Token NextToken()
        {
            Token current = Current;
            _pos++;
            return current;
        }

        private Token Match(SyntaxKind kind)
        {
            if (Current.Type == kind)
                return NextToken();

            _diagnostics.Add($"[ERR] Unexpected token <{Current}>, expected <{kind}>.");

            return new Token(kind, Current.Position, null, null);
        }

        private Expr ParseExpr(int parentPrecedence = 0)
        {
            Expr left = ParsePrimaryExpr();

            while (true)
            {
                int precedence = Current.Type.GetBinaryOperatorPrecedence();

                if (precedence == 0 || precedence <= parentPrecedence)
                    break;

                Token operatorToken = NextToken();
                Expr right = ParseExpr(precedence);

                left = new BinaryExpr(left, operatorToken, right);
            }

            return left;
        }

        private Expr ParsePrimaryExpr()
        {
            if (Current.Type == SyntaxKind.OpenParen)
            {
                var left = NextToken();
                var expr = ParseExpr();
                var right = Match(SyntaxKind.CloseParen);

                return new ParenthesizedExpr(left, expr, right);
            }


            var numberToken = Match(SyntaxKind.Number);

            return new NumberExpr(numberToken); 
        }

        public SyntaxTree Parse()
        {
            Expr expr = ParseExpr();
            Token endOfFile = Match(SyntaxKind.EndOfFile);

            return new SyntaxTree(_diagnostics, expr, endOfFile);
        }
    }
}
