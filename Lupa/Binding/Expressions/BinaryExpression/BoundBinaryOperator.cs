using Lupa.Lexing;

namespace Lupa.Binding
{
    public class BoundBinaryOperator
    {
        private BoundBinaryOperator(TokenKind tokenKind, BoundBinaryOperatorKind kind, Type type) : this(tokenKind, kind, type, type, type)
        {
        }

        private BoundBinaryOperator(TokenKind tokenKind, BoundBinaryOperatorKind kind, Type operandType, Type resultType) : this(tokenKind, kind, operandType, operandType, resultType)
        {
        }
        public BoundBinaryOperator(TokenKind tokenKind, BoundBinaryOperatorKind kind, Type leftType, Type rightType, Type resultType) 
        {
            TokenKind = tokenKind;
            Kind = kind;
            LeftType = leftType;
            RightType = rightType;
            Type = resultType;
        }

        public TokenKind TokenKind { get; }
        public BoundBinaryOperatorKind Kind { get; }
        public Type LeftType { get; }
        public Type RightType { get; }
        public Type Type { get; }

        private static BoundBinaryOperator[] _operators = {
            // Doubles
            new BoundBinaryOperator(TokenKind.Plus, BoundBinaryOperatorKind.Addition, typeof(double)),
            new BoundBinaryOperator(TokenKind.Minus, BoundBinaryOperatorKind.Subtraction, typeof(double)),
            new BoundBinaryOperator(TokenKind.Star, BoundBinaryOperatorKind.Multiplication, typeof(double)),
            new BoundBinaryOperator(TokenKind.Slash, BoundBinaryOperatorKind.Division, typeof(double)),
            new BoundBinaryOperator(TokenKind.SlashSlash, BoundBinaryOperatorKind.FloorDivision, typeof(double)),
            new BoundBinaryOperator(TokenKind.Percent, BoundBinaryOperatorKind.Modulus, typeof(double)),
            new BoundBinaryOperator(TokenKind.Caret, BoundBinaryOperatorKind.Pow, typeof(double)),
            new BoundBinaryOperator(TokenKind.GreaterThan, BoundBinaryOperatorKind.GreaterThan, typeof(double), typeof(double), typeof(bool)),
            new BoundBinaryOperator(TokenKind.GreaterEquals, BoundBinaryOperatorKind.GreaterEquals, typeof(double), typeof(double), typeof(bool)),
            new BoundBinaryOperator(TokenKind.LessThan, BoundBinaryOperatorKind.LessThan, typeof(double), typeof(double), typeof(bool)),
            new BoundBinaryOperator(TokenKind.LessEquals, BoundBinaryOperatorKind.LessEquals, typeof(double), typeof(double), typeof(bool)),
            new BoundBinaryOperator(TokenKind.EqualEquals, BoundBinaryOperatorKind.Equals, typeof(double), typeof(double), typeof(bool)),
            new BoundBinaryOperator(TokenKind.NotEquals, BoundBinaryOperatorKind.NotEquals, typeof(double), typeof(double), typeof(bool)),

            // Strings
            new BoundBinaryOperator(TokenKind.DotDot, BoundBinaryOperatorKind.Concatenation, typeof(string), typeof(string), typeof(string)),

            // Bools
            new BoundBinaryOperator(TokenKind.And, BoundBinaryOperatorKind.LogicalAnd, typeof(bool)),
            new BoundBinaryOperator(TokenKind.Or, BoundBinaryOperatorKind.LogicalOr, typeof(bool)),
            new BoundBinaryOperator(TokenKind.EqualEquals, BoundBinaryOperatorKind.Equals, typeof(bool)),
            new BoundBinaryOperator(TokenKind.NotEquals, BoundBinaryOperatorKind.NotEquals, typeof(bool)),
        };

        public static BoundBinaryOperator? Bind(TokenKind tokenKind, Type leftType, Type rightType)
        {
            foreach (var op in _operators)
            {
                if (op.TokenKind == tokenKind && op.LeftType == leftType && op.RightType == rightType)
                {
                    return op;
                }
            }

            return null;
        }
    }
}