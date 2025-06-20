using Lupa.Lexing;

namespace Lupa.Binding
{
    internal sealed class BoundUnaryOperator
    {
        private BoundUnaryOperator(TokenKind tokenKind, BoundUnaryOperatorKind kind, Type operandType) : this(tokenKind, kind, operandType, operandType) { }

        private BoundUnaryOperator(TokenKind tokenKind, BoundUnaryOperatorKind kind, Type operandType, Type resultType)
        {
            TokenKind = tokenKind;
            Kind = kind;
            OperandType = operandType;
            Type = resultType;
        }


        public TokenKind TokenKind { get; }
        public BoundUnaryOperatorKind Kind { get; }
        public Type OperandType { get; }
        public Type Type { get; }

        private static BoundUnaryOperator[] _operators =
        {
            new BoundUnaryOperator(TokenKind.Not, BoundUnaryOperatorKind.LogicalNegation, typeof(bool)),
            new BoundUnaryOperator(TokenKind.Minus, BoundUnaryOperatorKind.Negative, typeof(double)),
        };
        public static BoundUnaryOperator? Bind(TokenKind tokenKind, Type operandType)
        {
            foreach (var op in _operators)
            {
                if (op.TokenKind == tokenKind && op.OperandType == operandType)
                {
                    return op;
                }
            }

            return null;
        }
    }
}