namespace Lupa.Binding
{
    internal sealed class BoundUnaryExpression : BoundExpression {
        public BoundUnaryExpression(BoundUnaryOperator op, BoundExpression operand)
        {
            Operator = op;
            Operand = operand;
        }

        public BoundUnaryOperator Operator { get; }
        public BoundExpression Operand { get; }
        public override Type Type => Operator.Type;
        public override BoundNodeKind Kind => BoundNodeKind.UnaryExpression;
    }
}