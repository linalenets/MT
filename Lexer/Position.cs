namespace Lexer
{
    public struct Position
    {
        public int Line, Column;
        public override string ToString()
            => $"Line: {Line}, Column: {Column}";
    }
}
