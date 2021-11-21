namespace Lexer
{
    public enum TokenType
    {
        Number,
        Keyword,
        Operator,
        LP, RP, // (, )
        Id,
        String,
        Semicolon,
        LBR, RBR, // {, } 
        Comma,
        Comment
    }
}
