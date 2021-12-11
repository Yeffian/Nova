using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nova.CodeAnalysis
{
    public class NovaLexer
    {
        private readonly string _source;
        private int _pos;

        private List<string> _diagnostics = new List<string>();

        public IEnumerable<string> Diagnostics => _diagnostics;

        private char _curr
        {
            get
            {
                if (_pos >= _source.Length)
                    return '\0';

                return _source[_pos];
            }
        }

        public NovaLexer(string source)
        {
            _source = source;
            _pos = 0;
        }

        private void Advance() => _pos++;

        public Token NextToken()
        {
            // EOF or End-Of-File
            if (_pos >= _source.Length)
                return new Token(SyntaxKind.EndOfFile, _pos, "\0", null);

            if (Char.IsDigit(_curr))
            {
                int start = _pos;

                while (Char.IsDigit(_curr))
                    Advance();

                int length = _pos - start;
                string text = _source.Substring(start, length);

                if (!(float.TryParse(text, out var value)))
                    _diagnostics.Add($"[ERR] The number {_curr} cannot be represented as a Float32.");

                return new Token(SyntaxKind.Number, start, text, value);
            }

            // Operators and Whitespace

            switch (_curr)
            {
                case '+':
                    return new Token(SyntaxKind.Plus, _pos++, "+", null);
                case '-':
                    return new Token(SyntaxKind.Minus, _pos++, "-", null);
                case '*':
                    return new Token(SyntaxKind.Asterisk, _pos++, "*", null);
                case '/':
                    return new Token(SyntaxKind.Slash, _pos++, "/", null);
                case '(':
                    return new Token(SyntaxKind.OpenParen, _pos++, "(", null);
                case ')':
                    return new Token(SyntaxKind.CloseParen, _pos++, ")", null);
                case ' ':
                case '\0':
                case '\n':
                    return new Token(SyntaxKind.Whitespace, _pos++, String.Empty, null);
            }

            _diagnostics.Add($"[ERR] Bad character in input: '{_curr}'");
            return new Token(SyntaxKind.Unknown, _pos++, _source.Substring(_pos - 1, 1), null);
        }
    }
}
