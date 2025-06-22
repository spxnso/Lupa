using Lupa.Lexing;

namespace Lupa.AST {
    public sealed class LiteralExpression : Expression
    {
        public override NodeKind Kind => NodeKind.LiteralExpression;

        public Token LiteralToken { get; }
        public object Value { get; }

        public LiteralExpression(Token literalToken) : this(literalToken, literalToken.Lexeme)
        {}
        public LiteralExpression(Token literalToken, object value)
        {
            LiteralToken = literalToken;
            Value = value;
        }
    }
}