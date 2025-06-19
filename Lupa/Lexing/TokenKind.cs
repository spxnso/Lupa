namespace Lupa.Lexing
{
    public enum TokenKind {
        Eof = 0,

        // Primitives
        Number,


        // Binary Operators
        Plus,
        Minus,
        Star,
        Slash,
        SlashSlash,
        Percent,
        Caret,
        Error
    }
}