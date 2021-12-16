using System.Collections.Generic;
using System.Linq;
using Nova.CodeAnalysis.Syntax;
using Xunit;
using Xunit.Sdk;

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
        public void Lexer_LexOperatorsAndNumbers_AreNotEmpty()
        {
            var tokens = GetTokens("1 + 3 - 2 * 9 / 3 ");
            
            Assert.True(tokens.Any());
        }

        [Fact]
        public void Lexer_LexBinaryExprs_AreCorrectTypeAndValue()
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
    }
}
