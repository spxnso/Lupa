using Lupa.Binding;
using Lupa.Lexing;

namespace Lupa
{

    public class Evaluator
    {
        private readonly BoundExpression _root;
        public Evaluator(BoundExpression root)
        {
            _root = root;
        }

        public object Evaluate()
        {
            return EvaluateExpression(_root);
        }

        public object EvaluateExpression(BoundExpression expression)
        {
            if (expression is BoundLiteralExpression n)
            {
                return n.Value;
            }
            if (expression is BoundUnaryExpression u)
            {
                var operand = EvaluateExpression(u.Operand);
                switch (u.Operator.Kind)
                {
                    case BoundUnaryOperatorKind.Negative:
                        return -(double)operand;
                    case BoundUnaryOperatorKind.LogicalNegation:
                        return !(bool)operand;
                }
            }

            if (expression is BoundBinaryExpression b)
            {
                var left = EvaluateExpression(b.Left);
                var right = EvaluateExpression(b.Right);

                switch (b.Operator.Kind)
                {
                    case BoundBinaryOperatorKind.Addition:
                        return (double) left + (double) right;
                    case BoundBinaryOperatorKind.Subtraction:
                        return (double) left - (double) right;
                    case BoundBinaryOperatorKind.Multiplication:
                        return (double) left * (double) right;
                    case BoundBinaryOperatorKind.Division:
                        return (double) left / (double) right;
                    case BoundBinaryOperatorKind.FloorDivision:
                        return Math.Floor((double) left / (double) right);
                    case BoundBinaryOperatorKind.Modulus:
                        return (double) left % (double) right;
                    case BoundBinaryOperatorKind.Pow:
                        return Math.Pow((double) left, (double) right);
                    case BoundBinaryOperatorKind.LogicalAnd:
                        return (bool)left && (bool)right;
                    case BoundBinaryOperatorKind.LogicalOr:
                        return (bool)left || (bool)right;
                    case BoundBinaryOperatorKind.Equals:
                        return Equals(left, right);
                    case BoundBinaryOperatorKind.NotEquals:
                        return !Equals(left, right);
                    case BoundBinaryOperatorKind.GreaterThan:
                        return (double)left > (double)right;
                    case BoundBinaryOperatorKind.GreaterEquals:
                        return (double)left >= (double)right;
                    case BoundBinaryOperatorKind.LessThan:
                        return (double)left < (double)right;
                    case BoundBinaryOperatorKind.LessEquals:
                        return (double)left <= (double)right;
                    default:
                        throw new Exception($"Unexpected binary operator {b.Operator.Kind}");
                }
            }

            throw new Exception($"Unknown expression kind: {expression.Kind}");
        }
    }
}