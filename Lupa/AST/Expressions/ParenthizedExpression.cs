using Lupa.Lexing;

namespace Lupa.AST { 
    internal sealed class ParenthizedExpression : Expression
    {
        public override NodeKind Kind => NodeKind.ParenthizedExpression;

        public Token OpenParenthesisToken { get; }
        public Expression Expression { get; }
        public Token CloseParenthesisToken { get; }

        public ParenthizedExpression(Token openParenthesisToken, Expression expression, Token closeParenthesisToken)
        {
            OpenParenthesisToken = openParenthesisToken;
            Expression = expression;
            CloseParenthesisToken = closeParenthesisToken;
        }
    }
}
