using Lupa.Lexing;

namespace Lupa.Diagnostics
{
    public class DiagnosticFactory
    {
        public static Diagnostic MismatchedToken(TokenPosition position, TokenKind expectedTokenKind, TokenKind unexpectedTokenKind)
        {
            return new Diagnostic(DiagnosticKind.Error, $"Expected token <{expectedTokenKind}>, but found <{unexpectedTokenKind}> instead.", position);
        }

        public static Diagnostic UnexpectedToken(TokenPosition position, TokenKind unexpectedTokenKind)
        {
            return new Diagnostic(DiagnosticKind.Error, $"Unexpected token <{unexpectedTokenKind}>.", position);
        }

        public static Diagnostic UnexpectedCharacter(TokenPosition position, char character)
        {
            return new Diagnostic(DiagnosticKind.Error, $"Unexpected character '{character}'.", position);
        }

        public static Diagnostic MalformedNumber(TokenPosition position, string number)
        {
            return new Diagnostic(DiagnosticKind.Error, $"Malformed number '{number}'.", position);
        }
        public static Diagnostic BinaryOperatorTypeError(TokenPosition position, TokenKind operatorKind, Type leftType, Type rightType)
        {
            return new Diagnostic(
                DiagnosticKind.Error,
                $"Binary operator <{operatorKind}> cannot be applied to types '{leftType}' and '{rightType}'.",
                position
            );
        }

        public static Diagnostic UnaryOperatorTypeError(TokenPosition position, TokenKind operatorKind, Type operandType)
        {
            return new Diagnostic(
                DiagnosticKind.Error,
                $"Unary operator <{operatorKind}> cannot be applied to type '{operandType}'.",
                position
            );
        }

        internal static Diagnostic UnterminatedString(TokenPosition stringTokenPos, string v)
        {
            return new Diagnostic(
                DiagnosticKind.Error,
                $"Unterminated string literal at position {stringTokenPos}.", 
                stringTokenPos
            );
        }

        internal static Diagnostic UnterminatedBlockString(TokenPosition stringTokenPos, string v)
        {
            return new Diagnostic(
                DiagnosticKind.Error,
                $"Unterminated block string literal at position {stringTokenPos}.",
                stringTokenPos
            );
        }
    };
}