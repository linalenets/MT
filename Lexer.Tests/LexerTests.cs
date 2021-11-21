using Lexer;
using Xunit;
namespace LexerTests
{

    public class LexerTests
    {
        [Fact]
        public void CheckErrorStringWithoutEnd()
        {
            var lexer = new Lexer.Lexer();
            var tokens = lexer.Tokenize("\"asdasdasdasd");
            Assert.True(tokens[0].Type == TokenType.String);
            Assert.Single(lexer.Errors);
            Assert.Equal("String does not have an end", lexer.Errors[0].Message);
        }
        [Theory]
        [InlineData("1")]
        [InlineData("436.123")]
        [InlineData("0.5")]
        public void CheckCorrectNumbers(string number)
        {
            var lexer = new Lexer.Lexer();
            lexer.Tokenize(number);
            Assert.Equal(0, lexer.Errors.Count);
        }
        [Theory]
        [InlineData("0.5.5")]
        [InlineData("1.1.1.1.1")]
        public void CheckInvalidNumbers(string number)
        {
            var lexer = new Lexer.Lexer();
            lexer.Tokenize(number);
            Assert.Single(lexer.Errors);
        }
        [Theory]
        [InlineData("+")]
        [InlineData("-")]
        [InlineData("*")]
        [InlineData("/")]
        [InlineData("=")]
        [InlineData("!")]
        [InlineData("!=")]
        [InlineData("==")]
        [InlineData(">=")]
        [InlineData("<=")]
        [InlineData("OR")]
        [InlineData("AND")]
        public void CheckOperators(string op)
        {
            var lexer = new Lexer.Lexer();
            var tokens = lexer.Tokenize(op);
            Assert.Single(tokens);
            Assert.Equal(TokenType.Operator, tokens[0].Type);
            Assert.Equal(op, tokens[0].Value);
        }
        [Theory]
        [InlineData("SomeID")]
        [InlineData("SomeId1234")]
        [InlineData("id1")]
        public void CheckCorrectId(string op)
        {
            var lexer = new Lexer.Lexer();
            var tokens = lexer.Tokenize(op);
            Assert.Single(tokens);
            Assert.Equal(TokenType.Id, tokens[0].Type);

        }
        [Theory]
        [InlineData("if")]
        [InlineData("else")]
        [InlineData("function")]
        [InlineData("while")]
        public void CheckKeywords(string kw)
        {
            var lexer = new Lexer.Lexer();
            var tokens = lexer.Tokenize(kw);
            Assert.Single(tokens);
            Assert.Equal(TokenType.Keyword, tokens[0].Type);
        }
        [Theory]
        [InlineData("{", TokenType.LBR)]
        [InlineData("}", TokenType.RBR)]
        [InlineData("(", TokenType.LP)]
        [InlineData(")", TokenType.RP)]
        [InlineData(";", TokenType.Semicolon)]
        [InlineData(",", TokenType.Comma)]
        public void CheckSpecialChars(string special, TokenType type)
        {
            var lexer = new Lexer.Lexer();
            var tokens = lexer.Tokenize(special);
            Assert.Single(tokens);
            Assert.Equal(type, tokens[0].Type);
        }
        [Fact]
        public void CheckComment()
        {
            var lexer = new Lexer.Lexer();
            var tokens = lexer.Tokenize("//Cool comment");
            Assert.Single(tokens);
            Assert.Equal(TokenType.Comment, tokens[0].Type);
            Assert.Equal("Cool comment", tokens[0].Value);
        }
    }
}
