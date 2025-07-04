using System.Text;
using Lupa.Diagnostics;

namespace Lupa.Lexing
{
    public class Lexer
    {
        private readonly string _input;
        private List<Token> _tokens = new List<Token>();
        private TokenPosition _position = new TokenPosition(1, 1, 0);

        private static Token Error(TokenPosition errorPosition) {
            return new Token(TokenKind.Error, string.Empty, errorPosition);
        }
        public Lexer(string input, DiagnosticBag diagnostics)
        {
            _input = input;
            Diagnostics = diagnostics;
        }
        public Lexer(string input) : this(input, new DiagnosticBag())
        {
        }

        #region Position utils
        private bool AtEof(int offset = 0)
        {
            return _position.Offset + offset >= _input.Length;
        }
        private char Peek(int offset = 0)
        {
            if (AtEof(offset)) return '\0';
            return _input[_position.Offset + offset];
        }

        private char Advance()
        {
            if (AtEof()) return '\0';
            char current = _input[_position.Offset++];

            if (current == '\n')
            {
                _position.Line++;
                _position.Column = 1;
            }
            else
            {
                _position.Column++;
            }

            return current;
        }

        private string Advance(int count)
        {
            StringBuilder sb = new();
            for (int i = 0; i < count; i++)
            {
                sb.Append(Advance());
            }
            return sb.ToString();
        }

        private string AdvanceWhile(Func<char, bool> predicate)
        {
            StringBuilder sb = new();

            while (!AtEof())
            {
                char c = Current;
                if (!predicate(c)) break;
                sb.Append(Advance());
            }
            return sb.ToString();
        }

        private char Current => Peek(0);
        private char LookAhead => Peek(1);

        #endregion



        #region Readers
        private char? ReadBackslashInString()
        {
            Advance();

            switch (Current)
            {
                case '\r':
                    Advance();
                    if (Current == '\n') Advance();
                    return null;

                case 'z':
                    Advance();
                    AdvanceWhile(char.IsWhiteSpace);
                    return null;

                case 'a': Advance(); return '\a';
                case 'b': Advance(); return '\b';
                case 'f': Advance(); return '\f';
                case 'n': Advance(); return '\n';
                case 'r': Advance(); return '\r';
                case 't': Advance(); return '\t';
                case 'v': Advance(); return '\v';
                case '\\': Advance(); return '\\';
                case '\"': Advance(); return '\"';
                case '\'': Advance(); return '\'';

                case '\0': return null;

                default:
                    return Advance();
            }
        }


        private Token ReadQuotedString()
        {
            TokenPosition stringTokenPos = _position;
            StringBuilder sb = new();

            char quoteChar = Advance();

            while (Current != quoteChar)
            {
                if (AtEof())
                {
                    Diagnostics.Add(DiagnosticFactory.UnexpectedCharacter(_position, Current));
                    return Error(_position);
                }

                switch (Current)
                {
                    case '\0':
                    case '\r':
                    case '\n':
                        Diagnostics.Add(DiagnosticFactory.UnterminatedString(stringTokenPos, sb.ToString()));
                        return Error(_position);

                    case '\\':
                        char? escaped = ReadBackslashInString();
                        if (escaped is not null)
                            sb.Append(escaped.Value);
                        break;

                    default:
                        sb.Append(Advance());
                        break;
                }
            }

            Advance();

            return new Token(TokenKind.String, sb.ToString(), stringTokenPos);
        }

        private Token ReadBlockString()
        {
            TokenPosition startPos = _position;
            StringBuilder sb = new();

            Advance();

            int openEqualsCount = 0;
            while (Current == '=')
            {
                openEqualsCount++;
                Advance();
            }

            if (Current != '[')
            {
                Diagnostics.Add(DiagnosticFactory.UnterminatedBlockString(startPos, sb.ToString()));
                return Error(_position);
            }
            Advance();
            while (!AtEof())
            {
                if (Current == ']')
                {
                    Advance();

                    int closeEqualsCount = 0;
                    while (Current == '=')
                    {
                        closeEqualsCount++;
                        Advance();
                    }

                    if (Current == ']' && closeEqualsCount == openEqualsCount)
                    {
                        Advance();
                        return new Token(TokenKind.String, sb.ToString(), startPos);
                    }

                    sb.Append(Advance());
                }
                else
                {
                    sb.Append(Advance());
                }
            }

            Diagnostics.Add(DiagnosticFactory.UnterminatedBlockString(startPos, sb.ToString()));
            return Error(_position);
        }

        private Token ReadNumber()
        {
            TokenPosition numberTokenPos = _position;
            bool hasDot = false;
            bool hasExponent = false;
            StringBuilder sb = new();

            if (Current == '0' && (LookAhead == 'x' || LookAhead == 'X'))
            {
                sb.Append(Advance(2));
                sb.Append(AdvanceWhile(c => char.IsDigit(c) || (c >= 'a' && c <= 'f') || (c >= 'A' && c <= 'F') || c == '_'));
            }
            else if (Current == '0' && (LookAhead == 'b' || LookAhead == 'B'))
            {
                sb.Append(Advance(2));
                sb.Append(AdvanceWhile(c => c == '0' || c == '1' || c == '_'));
            }
            else
            {
                while (true)
                {
                    if (char.IsDigit(Current) || Current == '_')
                    {
                        sb.Append(Advance());
                    }
                    else if (Current == '.' && !hasDot && !hasExponent && char.IsDigit(Peek(1)))
                    {
                        hasDot = true;
                        sb.Append(Advance());
                    }
                    else if ((Current == 'e' || Current == 'E') && !hasExponent)
                    {
                        hasExponent = true;
                        sb.Append(Advance());

                        if (Current == '+' || Current == '-')
                            sb.Append(Advance());

                        if (!(char.IsDigit(Current) || Current == '_'))
                        {
                            Diagnostics.Add(DiagnosticFactory.MalformedNumber(numberTokenPos, sb.ToString()));
                            return Error(_position);
                        }
                    }
                    else if (char.IsLetter(Current))
                    {
                        Diagnostics.Add(DiagnosticFactory.MalformedNumber(numberTokenPos, sb.ToString()));
                        return Error(_position);
                    }
                    else
                    {
                        break;
                    }
                }
            }

            return new Token(TokenKind.Number, sb.ToString(), numberTokenPos);
        }

        private Token ReadName()
        {
            if (!(Current.IsAsciiLetter() || Current == '_' || Current == '@'))
            {
                Diagnostics.Add(DiagnosticFactory.UnexpectedCharacter(_position, Current));
                return Error(_position);
            }

            StringBuilder sb = new();
            sb.Append(Advance());
            sb.Append(AdvanceWhile(c => char.IsLetterOrDigit(Current) || c == '_' && !AtEof()));

            string name = sb.ToString();

            var kind = name.GetKindFromKeyword();
            return (kind != null) ? new Token(kind.Value, name, _position) : new Token(TokenKind.Name, name, _position);
        }


        private Token ReadNext()
        {
            AdvanceWhile(char.IsWhiteSpace);
            TokenPosition tokenPosition = _position;

            if (AtEof()) return new Token(TokenKind.Eof, String.Empty, tokenPosition);

            if (char.IsDigit(Current))
            {
                return ReadNumber();
            }
            else if (Current.IsAsciiLetter())
            {
                return ReadName();
            }
            else if (Current == '"' || Current == '\'')
            {
                return ReadQuotedString();
            }

            switch (Current)
            {
                case '+':
                    return new Token(TokenKind.Plus, Advance().ToString(), tokenPosition);
                case '-':
                    return new Token(TokenKind.Minus, Advance().ToString(), tokenPosition);
                case '*':
                    return new Token(TokenKind.Star, Advance().ToString(), tokenPosition);
                case '/':
                    if (LookAhead == '/')
                    {
                        return new Token(TokenKind.SlashSlash, Advance(2), tokenPosition);
                    }
                    return new Token(TokenKind.Slash, Advance().ToString(), tokenPosition);
                case '%':
                    return new Token(TokenKind.Percent, Advance().ToString(), tokenPosition);
                case '^':
                    return new Token(TokenKind.Caret, Advance().ToString(), tokenPosition);
                case '>':
                    if (LookAhead == '=')
                    {
                        return new Token(TokenKind.GreaterEquals, Advance(2), tokenPosition);
                    }
                    return new Token(TokenKind.GreaterThan, Advance().ToString(), tokenPosition);
                case '<':
                    if (LookAhead == '=')
                    {
                        return new Token(TokenKind.LessEquals, Advance(2), tokenPosition);
                    }
                    return new Token(TokenKind.LessThan, Advance().ToString(), tokenPosition);
                case '=':
                    if (LookAhead == '=')
                    {
                        return new Token(TokenKind.EqualEquals, Advance(2), tokenPosition);
                    }
                    return new Token(TokenKind.Equals, Advance().ToString(), tokenPosition);

                case '~':
                    if (LookAhead == '=')
                    {
                        return new Token(TokenKind.NotEquals, Advance(2), tokenPosition);
                    }
                    Diagnostics.Add(DiagnosticFactory.UnexpectedCharacter(_position, Current));
                    return Error(_position);
                case '.':
                    if (LookAhead == '.')
                    {
                        return new Token(TokenKind.DotDot, Advance(2), tokenPosition);
                    }
                    return new Token(TokenKind.Dot, Advance().ToString(), tokenPosition);
                case '#':
                    return new Token(TokenKind.Hash, Advance().ToString(), tokenPosition);
                case '(':
                    return new Token(TokenKind.LeftParen, Advance().ToString(), tokenPosition);
                case ')':
                    return new Token(TokenKind.RightParen, Advance().ToString(), tokenPosition);
                case '[':
                    if (LookAhead == '[' || LookAhead == '=')
                    {
                        return ReadBlockString();
                    }
                    return new Token(TokenKind.LeftBracket, Advance().ToString(), tokenPosition);
                default:
                    var errorPosition = _position;
                    Diagnostics.Add(DiagnosticFactory.UnexpectedCharacter(errorPosition, Advance()));
                    return Error(errorPosition);
            }
        }


        #endregion
        public IEnumerable<Token> Lex()
        {
            while (!AtEof())
            {
                Token token = ReadNext();
                _tokens.Add(token);
            }

            _tokens.Add(new Token(TokenKind.Eof, String.Empty, _position));
            return Tokens;
        }

        #region Public 
        public void Debug()
        {   
            foreach (var token in Tokens)
            {
                token.Debug();
            }
        }

        public DiagnosticBag Diagnostics { get; }

        public IEnumerable<Token> Tokens => _tokens;
        #endregion
    }
}