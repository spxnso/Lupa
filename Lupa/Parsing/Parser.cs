using Lupa.Lexing;
using Lupa.Diagnostics;
using Lupa.AST;

namespace Lupa.Parsing
{

    public class Parser {
        private readonly List<Token> _tokens = new List<Token>();
        private int _position;
        private DiagnosticBag _diagnostics { get; set; }
        public Parser(IEnumerable<Token> tokens, DiagnosticBag diagnostics)
        {
            _tokens = tokens.ToList();
            _position = 0;
            _diagnostics = diagnostics;
        }
        public Parser(IEnumerable<Token> tokens) : this(tokens, new DiagnosticBag())
        {
        }


        private Token Peek(int offset = 0) {
            var index = offset + _position;
            if (index >= _tokens.Count()) return _tokens[^1];
            
            return _tokens[index];
        }

        private Token Current => Peek();
        private Token LookAhead => Peek(1);

        private Token Advance() {
            if (_position >= _tokens.Count()) return _tokens[^1];
            return _tokens[_position++];
        }

        private Token Match(TokenKind kind) {
            if (Current.Kind == kind) {
                return Advance();
            }
            else {
                _diagnostics.Add(DiagnosticFactory.MismatchedToken(Current.Position, kind, Current.Kind));
                return Advance();
            }
        }


        public SyntaxTree Parse() {
            var root = ParseExpression();
            var eof = Match(TokenKind.Eof);
            
            return new SyntaxTree(root, eof, _diagnostics);
        }

        private Expression ParseExpression(int parentPrecedence = 0) {
            Expression left;
            
            var unaryOperatorPrecedence = Current.Kind.GetUnaryOperatorPrecedence();
            if (unaryOperatorPrecedence != 0 && unaryOperatorPrecedence > parentPrecedence) {
                var operatorToken = Advance();
                var operand = ParsePrimaryExpression();
                left = new UnaryExpression(operatorToken, operand);
            } else {
                left = ParsePrimaryExpression();
            }

            while (true) {
                var precedence = Current.Kind.GetBinaryOperatorPrecedence();
                if (precedence == 0 || precedence <= parentPrecedence) {
                    break;
                }

                var operatorToken = Advance();
                var right = ParseExpression(precedence);

                left = new BinaryExpression(left, operatorToken, right);
            }

            return left;
        }

        private Expression ParsePrimaryExpression() {
            switch (Current.Kind) {
                case TokenKind.LeftParen:
                    var leftParenToken = Advance();
                    var expression = ParseExpression();
                    var rightParenToken = Match(TokenKind.RightParen);

                    return new ParenthesizedExpression(leftParenToken, expression, rightParenToken);
                case TokenKind.String:
                    var stringToken = Advance();
                    return new LiteralExpression(stringToken, stringToken.Lexeme);
                case TokenKind.Boolean:
                    var booleanToken = Advance();
                    return new LiteralExpression(booleanToken, booleanToken.Lexeme == "true");
                case TokenKind.Number:
                    var numberToken = Advance();
                    return new LiteralExpression(numberToken, numberToken.TryParseLuauNumber(out var numberValue) ? numberValue : 0);
                case TokenKind.Name:
                    var nameToken = Advance();
                    return new NameExpression(nameToken);
                default:
                    _diagnostics.Add(DiagnosticFactory.UnexpectedToken(Current.Position, Current.Kind));
                    throw new Exception($"Unexpected token {Current.Kind} at position {Current.Position}.");
            }
        }
    }


}