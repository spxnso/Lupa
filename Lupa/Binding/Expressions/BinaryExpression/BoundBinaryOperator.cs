using Lupa.Lexing;

namespace Lupa.Binding
{
    public sealed class BoundBinaryOperator
    {
        public TokenKind TokenKind { get; }
        public BoundBinaryOperatorKind Kind { get; }
        public Type LeftType { get; }
        public Type RightType { get; }
        public Type Type { get; }

        private BoundBinaryOperator(TokenKind tokenKind, BoundBinaryOperatorKind kind, Type type)
            : this(tokenKind, kind, type, type, type)
        {
        }

        private BoundBinaryOperator(TokenKind tokenKind, BoundBinaryOperatorKind kind, Type operandType, Type resultType)
            : this(tokenKind, kind, operandType, operandType, resultType)
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

        private static readonly BoundBinaryOperator[] _operators = {
            #region Arithmetic (Double)
            new BoundBinaryOperator(TokenKind.Plus,         BoundBinaryOperatorKind.Addition,       typeof(double)),
            new BoundBinaryOperator(TokenKind.Minus,        BoundBinaryOperatorKind.Subtraction,    typeof(double)),
            new BoundBinaryOperator(TokenKind.Star,         BoundBinaryOperatorKind.Multiplication, typeof(double)),
            new BoundBinaryOperator(TokenKind.Slash,        BoundBinaryOperatorKind.Division,       typeof(double)),
            new BoundBinaryOperator(TokenKind.SlashSlash,   BoundBinaryOperatorKind.FloorDivision,  typeof(double)),
            new BoundBinaryOperator(TokenKind.Percent,      BoundBinaryOperatorKind.Modulus,        typeof(double)),
            new BoundBinaryOperator(TokenKind.Caret,        BoundBinaryOperatorKind.Pow,            typeof(double)),

            new BoundBinaryOperator(TokenKind.GreaterThan,   BoundBinaryOperatorKind.GreaterThan,   typeof(double), typeof(double), typeof(bool)),
            new BoundBinaryOperator(TokenKind.GreaterEquals, BoundBinaryOperatorKind.GreaterEquals, typeof(double), typeof(double), typeof(bool)),
            new BoundBinaryOperator(TokenKind.LessThan,      BoundBinaryOperatorKind.LessThan,      typeof(double), typeof(double), typeof(bool)),
            new BoundBinaryOperator(TokenKind.LessEquals,    BoundBinaryOperatorKind.LessEquals,    typeof(double), typeof(double), typeof(bool)),
            new BoundBinaryOperator(TokenKind.EqualEquals,   BoundBinaryOperatorKind.Equals,        typeof(double), typeof(double), typeof(bool)),
            new BoundBinaryOperator(TokenKind.NotEquals,     BoundBinaryOperatorKind.NotEquals,     typeof(double), typeof(double), typeof(bool)),
            #endregion

            #region Logical (Boolean)
            new BoundBinaryOperator(TokenKind.And,           BoundBinaryOperatorKind.LogicalAnd,    typeof(bool)),
            new BoundBinaryOperator(TokenKind.Or,            BoundBinaryOperatorKind.LogicalOr,     typeof(bool)),
            new BoundBinaryOperator(TokenKind.EqualEquals,   BoundBinaryOperatorKind.Equals,        typeof(bool)),
            new BoundBinaryOperator(TokenKind.NotEquals,     BoundBinaryOperatorKind.NotEquals,     typeof(bool)),
            #endregion

            #region Comparison (String)
            new BoundBinaryOperator(TokenKind.EqualEquals,   BoundBinaryOperatorKind.Equals,        typeof(string), typeof(string), typeof(bool)),
            new BoundBinaryOperator(TokenKind.NotEquals,     BoundBinaryOperatorKind.NotEquals,     typeof(string), typeof(string), typeof(bool)),
            #endregion

            #region Concatenation (..)
            new BoundBinaryOperator(TokenKind.DotDot,        BoundBinaryOperatorKind.Concatenation, typeof(string), typeof(string), typeof(string)),
            new BoundBinaryOperator(TokenKind.DotDot,        BoundBinaryOperatorKind.Concatenation, typeof(double), typeof(double), typeof(string)),
            new BoundBinaryOperator(TokenKind.DotDot,        BoundBinaryOperatorKind.Concatenation, typeof(string), typeof(double), typeof(string)),
            new BoundBinaryOperator(TokenKind.DotDot,        BoundBinaryOperatorKind.Concatenation, typeof(double), typeof(string), typeof(string)),
            #endregion
        };

        public static BoundBinaryOperator? Bind(TokenKind tokenKind, Type leftType, Type rightType)
        {
            foreach (var op in _operators)
            {
                if (op.TokenKind == tokenKind &&
                    op.LeftType == leftType &&
                    op.RightType == rightType)
                {
                    return op;
                }
            }

            foreach (var op in _operators)
            {
                if (op.TokenKind == tokenKind &&
                    (op.LeftType == typeof(object) || op.LeftType == leftType) &&
                    (op.RightType == typeof(object) || op.RightType == rightType))
                {
                    return op;
                }
            }
            return null;
        }
    }
}
