using Lupa.AST;
using Lupa.Diagnostics;

namespace Lupa.Binding
{
    internal sealed class Binder {
        public DiagnosticBag Diagnostics { get; }
        public Binder(DiagnosticBag diagnostics)
        {
            Diagnostics = diagnostics;
        }


        public BoundExpression BindExpression(Expression expr) {
            switch(expr.Kind) {
                case NodeKind.UnaryExpression:
                    return BindUnaryExpression((UnaryExpression)expr);
                case NodeKind.BinaryExpression:
                    return BindBinaryExpression((BinaryExpression)expr);
                case NodeKind.ParenthesizedExpression:
                    return BindExpression(((ParenthesizedExpression)expr).Expression);
                case NodeKind.LiteralExpression:
                    return BindLiteralExpression((LiteralExpression)expr);
                default:
                    throw new Exception($"Unexpected expression kind: {expr.Kind}");
            }
        }

        private BoundExpression BindUnaryExpression(UnaryExpression expr)
        {
            var boundOperand = BindExpression(expr.Operand);
            var boundOperator = BoundUnaryOperator.Bind(expr.OperatorToken.Kind, boundOperand.Type);

            if (boundOperator == null) {
                Diagnostics.Add(DiagnosticFactory.UnaryOperatorTypeError(expr.OperatorToken.Position, expr.OperatorToken.Kind, boundOperand.Type));

                return boundOperand;
            }
            return new BoundUnaryExpression(boundOperator, boundOperand);
        }

        private BoundExpression BindBinaryExpression(BinaryExpression expr)
        {
            var boundLeft = BindExpression(expr.Left);
            var boundRight = BindExpression(expr.Right);

            var boundOperator = BoundBinaryOperator.Bind(expr.OperatorToken.Kind, boundLeft.Type, boundRight.Type);

            if (boundOperator == null) {
                Diagnostics.Add(DiagnosticFactory.BinaryOperatorTypeError(expr.OperatorToken.Position, expr.OperatorToken.Kind, boundLeft.Type, boundRight.Type));

                return boundLeft;
            }

            return new BoundBinaryExpression(boundLeft, boundOperator, boundRight);
        }

        private BoundExpression BindLiteralExpression(LiteralExpression expr) {
            var value = expr.Value ?? 0;

            return new BoundLiteralExpression(value);
        }
    }
}