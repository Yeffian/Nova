﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nova.CodeAnalysis.Syntax
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

                if (token.Kind != SyntaxKind.Whitespace && token.Kind != SyntaxKind.Unknown) tokens.Add(token);
            } while (token.Kind != SyntaxKind.EndOfFile);

            _tokens = tokens.ToArray();
            
            // Get all the diagnostics from the lexer and add them to the parsers diagnostics as well
            _diagnostics.AddRange(lexer.Diagnostics);
        }

        private Token Peek(int offset)
        {
            int index = _pos - offset;

            if (index >= _tokens.Length)
                return _tokens[^1];

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
            if (Current.Kind == kind)
                return NextToken();

            _diagnostics.Add($"[ERR] Unexpected token <{Current}>, expected <{kind}>.");

            return new Token(kind, Current.Position, null, null);
        }

        private Expr ParseExpr(int parentPrecedence = 0)
        {
            Expr left;

            int unaryPrecdence = Current.Kind.GetUnaryOperatorPrecedence();

            if (unaryPrecdence != 0 && unaryPrecdence >= parentPrecedence)
            {
                Token operatorToken = NextToken();
                Expr operand = ParseExpr();

                left = new UnaryExpr(operatorToken, operand);
            }
            else
            {
                left = ParsePrimaryExpr();
            }

            while (true)
            {
                int precedence = Current.Kind.GetBinaryOperatorPrecedence();

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
            switch (Current.Kind)
            {
                case SyntaxKind.OpenParen:
                {
                    var left = NextToken();
                    var expr = ParseExpr();
                    var right = Match(SyntaxKind.CloseParen);

                    return new ParenthesizedExpr(left, expr, right);
                }
                case SyntaxKind.True:
                case SyntaxKind.False:
                {
                    Token keywordToken = NextToken();
                    bool value = keywordToken.Kind == SyntaxKind.True;

                    return new NumberExpr(keywordToken, value);
                }
                default:
                {
                    var numberToken = Match(SyntaxKind.Number);

                    return new NumberExpr(numberToken);
                }
            }
        }

        public SyntaxTree Parse()
        {
            Expr expr = ParseExpr();
            Token endOfFile = Match(SyntaxKind.EndOfFile);

            return new SyntaxTree(_diagnostics, expr, endOfFile);
        }
    }
}
