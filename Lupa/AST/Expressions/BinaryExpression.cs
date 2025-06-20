using Lupa.Lexing;

namespace Lupa.AST
{
    internal sealed class BinaryExpression : Expression
    {
        public override NodeKind Kind => NodeKind.BinaryExpression;

        public Expression Left { get; }
        public Token OperatorToken { get; }
        public Expression Right { get; }


        public BinaryExpression(Expression left, Token operatorToken, Expression right)
        {
            Left = left;
            OperatorToken = operatorToken;
            Right = right;
        }
    }
}