namespace Lupa.Lexing {
    public class Token {
        public Token(TokenKind kind, string lexeme, TokenPosition position) 
        {
            Kind = kind;
            Lexeme = lexeme;
            Position = position;
        }

        public TokenKind Kind { get; }
        public string Lexeme { get; }
        public TokenPosition Position { get; }
    }
}