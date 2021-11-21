using System.Collections.Generic;

namespace Lexer
{
    public class Lexer
    {
        private List<Error> _errors = new();
        private Position _currentPosition;
        private int _pos = 0;
        private HashSet<char> _operators = new() { '+', '-', '*', '/', };
        private HashSet<char> _canBeDouble = new() { '>', '<', '=', '!' };
        private HashSet<string> _keywords = new()
        {
            "function",
            "int",
            "double",
            "if",
            "else",
            "while",
            "null",
            "return",
        };
        private Dictionary<char, TokenType> _specialChars = new()
        {
            ['('] = TokenType.LP,
            [')'] = TokenType.RP,
            ['{'] = TokenType.LBR,
            ['}'] = TokenType.RBR,
            [';'] = TokenType.Semicolon,
            [','] = TokenType.Comma,
        };
        public IReadOnlyList<Error> Errors => _errors;

        public List<Token> Tokenize(string input)
        {
            var result = new List<Token>();
            _currentPosition = new Position { Line = 1, Column = 1 };
            _errors.Clear();

            while (_pos < input.Length)
            {
                switch (input[_pos])
                {
                    case '/' when _pos + 1 < input.Length && input[_pos + 1] == '/':
                        result.Add(TokenizeComment(input));
                        break;
                    case '"':
                        result.Add(TokenizeString(input));
                        break;
                    case '\n':
                        _currentPosition = new Position { Line = _currentPosition.Line + 1, Column = 1 };
                        _pos++;
                        break;
                    case char c when char.IsLetter(c):
                        result.Add(TokenizeId(input));
                        break;
                    case char c when _operators.Contains(c):
                        result.Add(new Token { Type = TokenType.Operator, Value = c.ToString(), Position = _currentPosition });
                        _currentPosition.Column++;
                        _pos++;
                        break;
                    case char c when _canBeDouble.Contains(c):
                        if (++_pos < input.Length && input[_pos] == '=')
                        {
                            result.Add(new Token { Type = TokenType.Operator, Value = new string(new[] { c, '=' }), Position = _currentPosition });
                            _pos++;
                            _currentPosition.Column += 2;
                        }
                        else
                        {
                            result.Add(new Token { Type = TokenType.Operator, Value = c.ToString(), Position = _currentPosition });
                            _currentPosition.Column++;
                            _pos++;
                        }
                        break;
                    case char c when _specialChars.ContainsKey(c):
                        result.Add(new Token { Type = _specialChars[c], Value = c.ToString(), Position = _currentPosition });
                        _currentPosition.Column++;
                        _pos++;
                        break;
                    case char c when char.IsDigit(c):
                        result.Add(TokenizeNumber(input));
                        break;
                    default:
                        _pos++;
                        _currentPosition.Column++;
                        break;
                }
            }
            return result;
        }

        private Token TokenizeComment(string input)
        {
            var pos = _currentPosition;
            int start = ++_pos + 1;
            if (_pos >= input.Length || input[_pos++] != '/')
            {
                _errors.Add(new Error { Message = "Expected second slash for comment", Position = _currentPosition });
            }
            while (_pos < input.Length && input[_pos] != '\n')
            {
                _pos++;
            }
            _currentPosition.Line += 1;
            _currentPosition.Column = 1;
            var val = input[start.._pos];
            return new Token { Type = TokenType.Comment, Value = val, Position = pos };
        }

        private Token TokenizeNumber(string input)
        {
            var start = _pos++;
            var pos = _currentPosition;
            _currentPosition.Column++;
            int dotCount = 0;
            while (_pos < input.Length && (char.IsDigit(input[_pos]) || input[_pos] == '.'))
            {
                if (input[_pos] == '.')
                {
                    dotCount++;
                }
                _pos++;
                _currentPosition.Column++;
            }
            if (dotCount > 1)
            {
                _errors.Add(new Error { Message = "More than one dot in the number", Position = _currentPosition });
            }
            var type = TokenType.Number;
            var val = input[start.._pos];
            return new Token { Type = type, Value = val, Position = pos };
        }

        private Token TokenizeId(string input)
        {
            int start = _pos++;
            var pos = _currentPosition;
            _currentPosition.Column++;
            while (_pos < input.Length && char.IsLetterOrDigit(input[_pos]))
            {
                _pos++;
                _currentPosition.Column++;
            }
            string res = input[start.._pos];
            var type = _keywords.Contains(res) ? TokenType.Keyword : res == "AND" || res == "OR" ? TokenType.Operator : TokenType.Id;
            return new Token { Type = type, Value = res, Position = pos };
        }

        private Token TokenizeString(string input)
        {
            int start = ++_pos;
            var pos = _currentPosition;
            _currentPosition.Column++;
            while (input[_pos] is not '"')
            {
                _pos++;
                _currentPosition.Column++;
                if (_pos >= input.Length || input[_pos] == '\n')
                {
                    _errors.Add(new Error { Message = "String does not have an end", Position = pos });
                    break;
                }
            }
            string res = input[start.._pos];
            _pos++;
            _currentPosition.Column++;
            var result = new Token { Type = TokenType.String, Value = res, Position = pos };
            return result;
        }
    }
}
