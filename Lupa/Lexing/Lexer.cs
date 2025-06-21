using System.Text;
using Lupa.Diagnostics;

namespace Lupa.Lexing
{
    public class Lexer
    {
        private readonly string _input;
        private List<Token> _tokens = new List<Token>();
        private TokenPosition _position = new TokenPosition(1, 1, 0);
        
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
            while (!AtEof() && predicate(Current))
            {
                sb.Append(Advance());
            }
            return sb.ToString();
        }

        private char Current => Peek(0);
        private char LookAhead => Peek(1);

        #endregion


        private Token Error => new Token(TokenKind.Error, String.Empty, _position);


        private Token ReadNumber()
        {
            TokenPosition numberTokenPos = _position;

            StringBuilder sb = new();

            sb.Append(AdvanceWhile(c => char.IsDigit(c) ||  c == '_'));
            return new Token(TokenKind.Number, sb.ToString(), numberTokenPos);
        }

        private Token ReadName()
        {
            if (!(Current.IsAsciiLetter() || Current == '_' || Current == '@')) {
                Diagnostics.Add(DiagnosticFactory.UnexpectedCharacter(_position, Current));
                return Error;
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
            } else if (Current.IsAsciiLetter()) {
                return ReadName();
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
                    if (LookAhead == '=') {
                        return new Token(TokenKind.NotEquals, Advance(2), tokenPosition);
                    }
                    Diagnostics.Add(DiagnosticFactory.UnexpectedCharacter(_position, Current));
                    return Error;
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
                default:
                    Diagnostics.Add(DiagnosticFactory.UnexpectedCharacter(_position, Current));
                    return Error;
            }
        }


        public IEnumerable<Token> Lex() {
            while (!AtEof()) {
                Token token = ReadNext(); 
                _tokens.Add(token);
            }
            
            _tokens.Add(new Token(TokenKind.Eof, String.Empty, _position));
            return Tokens;
        }

        public void Debug() {
            foreach (var token in Tokens) {
                token.Debug();
            }
        }
         public DiagnosticBag Diagnostics { get; }

        public IEnumerable<Token> Tokens => _tokens;
    }
}