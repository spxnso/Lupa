using Lupa.AST;
using Lupa.Lexing;

namespace Lupa
{
    internal class Evaluator
    {
        private readonly Expression _root;
        public Evaluator(Expression root)
        {
            _root = root;
        }

        public object Evaluate()
        {
            return EvaluateExpression(_root);
        }

        public object EvaluateExpression(Expression expression)
        {
            if (expression is LiteralExpression n)
            {
                return n.Value;
            }

            if (expression is ParenthizedExpression p)
            {
                return EvaluateExpression(p.Expression);
            }

            if (expression is UnaryExpression u)
            {
                var operand = EvaluateExpression(u.Operand);
                switch (u.OperatorToken.Kind)
                {
                    case TokenKind.Minus:
                        return -(double)operand;
                    case TokenKind.Not:
                        return !(bool)operand;
                }
            }

            if (expression is BinaryExpression b)
            {
                var left = EvaluateExpression(b.Left);
                var right = EvaluateExpression(b.Right);

                switch (b.OperatorToken.Kind)
                {
                    case TokenKind.Plus:
                        return (double)left + (double)right;
                    case TokenKind.Minus:
                        return (double)left - (double)right;
                    case TokenKind.Star:
                        return (double)left * (double)right;
                    case TokenKind.Slash:
                        return (double)left / (double)right;
                    case TokenKind.SlashSlash:
                        return Math.Floor((double)left / (double)right);
                    case TokenKind.Percent:
                        return (double)left % (double)right;
                    case TokenKind.Caret:
                        return Math.Pow((double)left, (double)right);
                    case TokenKind.And:
                        return (bool)left && (bool)right;
                    case TokenKind.Or:
                        return (bool)left || (bool)right;
                    case TokenKind.Equals:
                        return left.Equals(right);
                    case TokenKind.NotEquals:
                        return !left.Equals(right);
                    default:
                        throw new Exception($"Unknown binary operator: {b.OperatorToken.Kind}");
                }
            }

            throw new Exception($"Unknown expression kind: {expression.Kind}");
        }
    }
}