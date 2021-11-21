namespace Lexer
{
    public struct Token
    {
        public Position Position { get; set; }
        public TokenType Type { get; set; }
        public string Value { get; set; }
        public override string ToString()
            => $"{Type}\t{Value}\t{Position}";
    }
}
