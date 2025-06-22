using Lupa.Lexing;

namespace Lupa.AST
{
    public sealed class UnaryExpression : Expression
    {
        public override NodeKind Kind => NodeKind.UnaryExpression;
        public Token OperatorToken { get; }
        public Expression Operand { get; }

        public UnaryExpression(Token operatorToken, Expression operand)
        {
            OperatorToken = operatorToken;
            Operand = operand;
        }
    }
}