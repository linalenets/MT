namespace Lexer
{
    public struct Error
    {
        public string Message { get; set; }
        public Position Position { get; set; }
        public override string ToString()
            => $"{Message} at {Position}";
    }
}
