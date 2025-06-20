using Lupa.Lexing;

namespace Lupa.AST { 
    internal sealed class ParenthesizedExpression : Expression
    {
        public override NodeKind Kind => NodeKind.ParenthesizedExpression;

        public Token OpenParenthesisToken { get; }
        public Expression Expression { get; }
        public Token CloseParenthesisToken { get; }

        public ParenthesizedExpression(Token openParenthesisToken, Expression expression, Token closeParenthesisToken)
        {
            OpenParenthesisToken = openParenthesisToken;
            Expression = expression;
            CloseParenthesisToken = closeParenthesisToken;
        }
    }
}
