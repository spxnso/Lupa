namespace Lupa.Lexing
{
    public enum TokenKind {
        Eof = 0,
        Error,

        // Primitives
        Number,
        String,
        Boolean,
        
        Name,
        Reserved,
        
        // Symbols
        Equals,
        Dot,

        LeftParen,
        RightParen,
        
        // Binary Operators
        Plus,
        Minus,
        Star,
        Slash,
        SlashSlash,
        Percent,
        Caret,

        NotEquals,
        EqualEquals,
        GreaterEquals,
        GreaterThan,
        LessThan,
        LessEquals,

        DotDot,
        Or,
        And,

        // Unary Operators

        Hash,
        Not,

    }
}