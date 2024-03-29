﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Nova.CodeAnalysis.Syntax
{
    public class NovaLexer
    {
        private readonly string _source;
        private int _pos;

        private List<string> _diagnostics = new List<string>();

        public IEnumerable<string> Diagnostics => _diagnostics;

        private char Current => Peek(0);
        private char Lookahead => Peek(1);

        private char Peek(int offset)
        {
            int index = _pos - offset;

            if (index >= _source.Length)
                return '\0';

            return _source[index];
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

            // Numbers
            if (Char.IsDigit(Current))
            {
                int start = _pos;

                while (Char.IsDigit(Current))
                    Advance();

                int length = _pos - start;
                string text = _source.Substring(start, length);

                if (!(Int32.TryParse(text, out var value)))
                    _diagnostics.Add($"[ERR] The number {Current} cannot be represented as a Float32.");

                return new Token(SyntaxKind.Number, start, text, value);
            }
            
            // Booleans
            if (Char.IsLetter(Current))
            {
                int start = _pos;
                
                while (Char.IsLetter(Current))
                    Advance();

                int length = _pos - start;
                string text = _source.Substring(start, length);

                SyntaxKind kind = SyntaxFacts.GetKeywordKind(text);

                return new Token(kind, _pos, text, null);
            }

            // Operators and Whitespace
            switch (Current)
            {
                case '+':
                    return new Token(SyntaxKind.Plus, _pos++, "+", null);
                case '-':
                    return new Token(SyntaxKind.Minus, _pos++, "-", null);
                case '*':
                    return new Token(SyntaxKind.Asterisk, _pos++, "*", null);
                case '^':
                    return new Token(SyntaxKind.Caret, _pos++, "^", null);
                case '/':
                    return new Token(SyntaxKind.Slash, _pos++, "/", null);
                case '(':
                    return new Token(SyntaxKind.OpenParen, _pos++, "(", null);
                case ')':
                    return new Token(SyntaxKind.CloseParen, _pos++, ")", null);
                case '>':
                    return new Token(SyntaxKind.LessThan, _pos++, "<", null);
                case '<':
                    return new Token(SyntaxKind.GreaterThan, _pos++, ">", null);
                case '&':
                    if (Lookahead == '&')
                        return new Token(SyntaxKind.DoubleAmpersand, _pos += 2, "&&", null);
                    break;
                case '|':
                    if (Lookahead == '|')
                        return new Token(SyntaxKind.DoublePipe, _pos += 2, "||", null);
                    break;
                case '=':
                    if (Lookahead == '=')
                        return new Token(SyntaxKind.DoubleEquals, _pos += 2, "==", null);
                    break;
                case '!':
                    return new Token(SyntaxKind.Bang, _pos++, "!", null);
                case ' ':
                case '\0':
                case '\n':
                    return new Token(SyntaxKind.Whitespace, _pos++, String.Empty, null);
            }

            _diagnostics.Add($"[ERR] Bad character in input: '{Current}'");
            return new Token(SyntaxKind.Unknown, _pos++, _source.Substring(_pos - 1, 1), null);
        }
    }
}
