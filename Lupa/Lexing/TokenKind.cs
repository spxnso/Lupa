namespace Lupa.Lexing
{
    public enum TokenKind {
        #region Special
        Eof = 0,
        Error,
        #endregion

        #region Primitives
        Number,
        String,
        Boolean,
        
        Name,
        Reserved,
        #endregion

        #region Symbols
        Equals,
        Dot,
        LeftParen,
        RightParen,

        #endregion
        
        #region Operators

        #region  Binary Operators
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

        #endregion

        #region Unary Operators

        Hash,
        Not,

        #endregion
        
        #endregion

        #region Keywords
        Break,
        Do,
        Else,
        ElseIf,
        End,
        For,
        Function,
        If,
        In,
        Local,
        Nil,
        Repeat,
        Until,
        While,
        Continue,
        Type,
        TypeOf,
        Export,
        Extends,
        Require,
        Return,
        Then,
        #endregion
    }
}