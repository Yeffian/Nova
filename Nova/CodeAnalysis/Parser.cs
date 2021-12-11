using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nova.CodeAnalysis
{
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

        public SyntaxTree Parse()
        {
            Expr expr = ParseTerm();
            Token endOfFile = Match(SyntaxKind.EndOfFile);

            return new SyntaxTree(_diagnostics, expr, endOfFile);
        }

        private Expr ParseTerm()
        {
            var left = ParseFactor();

            while (Current.Type == SyntaxKind.Plus
                  || Current.Type == SyntaxKind.Minus)
            {
                Token operatorToken = NextToken();

                var right = ParseFactor();

                left = new BinaryExpr(left, operatorToken, right);
            }

            return left;
        }

        private Expr ParseFactor()
        {
            var left = ParsePrimaryExpr();

            while (Current.Type == SyntaxKind.Asterisk
                  || Current.Type == SyntaxKind.Slash)
            {
                Token operatorToken = NextToken();

                var right = ParsePrimaryExpr();

                left = new BinaryExpr(left, operatorToken, right);
            }

            return left;
        }

        private Expr ParseExpr() => ParseTerm();


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
    }
}
