using System.Collections.Generic;
using System.Linq;
using Nova.CodeAnalysis.Syntax;
using Xunit;

namespace Nova.Tests
{
    public class LexerTests
    {
        private IEnumerable<Token> GetTokens(string src)
        {
            NovaLexer lexer = new NovaLexer(src);
            
            Token token;
            do
            {
                token = lexer.NextToken();

                if (token.Kind != SyntaxKind.Whitespace && token.Kind != SyntaxKind.Unknown && token.Kind != SyntaxKind.EndOfFile)
                    yield return token;
            } while (token.Kind != SyntaxKind.EndOfFile);
        }

        [Fact]
        public void Lexer_LexNumbers_AreNotEmpty()
        {
            var tokens = GetTokens("12345");

            Assert.True(tokens.Any());
        }

        [Fact]
        public void Lexer_LexNumbers_AreCorrectTypeAndValue()
        {
            var tokens = GetTokens("12345");

            foreach (Token t in tokens)
            {
                Assert.Equal(SyntaxKind.Number, t.Kind);
                Assert.Equal("12345", t.Text);
            }
        }

        [Fact]
        public void Lexer_LexBooleans_AreNotEmpty()
        {
            var tokens = GetTokens("true");
            
            Assert.True(tokens.Any());
        }

        [Fact]
        public void Lexer_LexBooleans_AreCorrectTypeAndValue()
        {
            var tokens = GetTokens("true");
            
            foreach (Token t in tokens)
            {
                Assert.Equal(SyntaxKind.True, t.Kind);
            }
        }

        [Fact]
        public void Lexer_LexBinaryNumberExprs_AreNotEmpty()
        {
            var tokens = GetTokens("1 + 3 - 2 * 9 / 3 ");
            
            Assert.True(tokens.Any());
        }

        [Fact]
        public void Lexer_LexBinaryNumberExprs_AreCorrectTypeAndValue()
        {
            var tokens = GetTokens("1 + 3 - 2 * 9 / 3 ");

            foreach (Token t in tokens)
            {
                if (int.TryParse(t.Text, out var _))
                    Assert.Equal(SyntaxKind.Number, t.Kind);
                else
                {
                    switch (t.Text)
                    {
                        case "+":
                            Assert.Equal(SyntaxKind.Plus, t.Kind);
                            break;
                        case "-":
                            Assert.Equal(SyntaxKind.Minus, t.Kind);
                            break;
                        case "*":
                            Assert.Equal(SyntaxKind.Asterisk, t.Kind);
                            break;
                        case "/":
                            Assert.Equal(SyntaxKind.Slash, t.Kind);
                            break;
                    }
                }
            }
        }

        [Fact]
        public void Lexer_LexBinaryBooleanExprs_AreNotEmpty()
        {
            var tokens = GetTokens("(true && true) || (false && true)");
            
            Assert.True(tokens.Any());
        }

        [Fact]
        public void Lexer_LexBinaryBooleanExprs_AreCorrectTypeAndValue()
        {
            var tokens = GetTokens("(true && true) || (false && true)");

            foreach (Token t in tokens)
            {
                switch (t.Text)
                {
                    case "(":
                        Assert.Equal(SyntaxKind.OpenParen, t.Kind);
                        break;
                    case ")":
                        Assert.Equal(SyntaxKind.CloseParen, t.Kind);
                        break;
                    case "true":
                        Assert.Equal(SyntaxKind.True, t.Kind);
                        break;
                    case "false":
                        Assert.Equal(SyntaxKind.False, t.Kind);
                        break;
                    case "&&":
                        Assert.Equal(SyntaxKind.DoubleAmpersand, t.Kind);
                        break;
                    case "||":
                        Assert.Equal(SyntaxKind.DoublePipe, t.Kind);
                        break;
                }
            }
        }

        [Fact]
        public void Lexer_LexUnaryNumberExprs_AreNotEmpty()
        {
            var tokens = GetTokens("-12");
            
            Assert.True(tokens.Any());
        }

        [Fact]
        public void Lexer_LexUnaryNumberExprs_AreCorrectTypeAndValue()
        {
            var tokens = GetTokens("-12");

            foreach (Token t in tokens)
            {
                if (int.TryParse(t.Text, out var _))
                    Assert.Equal(SyntaxKind.Number, t.Kind);
                
                switch (t.Text)
                {
                    case "-":
                        Assert.Equal(SyntaxKind.Minus, t.Kind);
                        break;
                }
            }
        }

        [Fact]
        public void Lexer_LexUnaryBooleanExprs_AreNotEmpty()
        {
            var tokens = GetTokens("!true");
            
            Assert.True(tokens.Any());
        }

        [Fact]
        public void Lexer_LexUnaryBoolean_Exprs_AreCorrectTypeAndValue()
        {
            var tokens = GetTokens("!true");

            foreach (Token t in tokens)
            {
                if (t.Text == "!")
                    Assert.Equal(SyntaxKind.Bang, t.Kind);
                else if (t.Text == "true")
                    Assert.Equal(SyntaxKind.True, t.Kind);
            }
        }
    }
}
