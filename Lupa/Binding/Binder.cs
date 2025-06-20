using Lupa.AST;
using Lupa.Diagnostics;

namespace Lupa.Binding
{
    internal sealed class Binder {
        private List<Diagnostic> _diagnostics = new List<Diagnostic>();

        public IEnumerable<Diagnostic> Diagnostics => _diagnostics;

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
            throw new NotImplementedException();
        }

        private BoundExpression BindBinaryExpression(BinaryExpression expr)
        {
            var boundLeft = BindExpression(expr.Left);
            var boundRight = BindExpression(expr.Right);

            var boundOperator = BoundBinaryOperator.Bind(expr.OperatorToken.Kind, boundLeft.Type, boundRight.Type);

            if (boundOperator == null) {
                _diagnostics.Add(new Diagnostic(
                    DiagnosticKind.TypeError,
                    $"Operator '{expr.OperatorToken.Lexeme}' cannot be applied to operands of type '{boundLeft.Type}' and '{boundRight.Type}'",
                    expr.OperatorToken.Position
                ));

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