using Lupa.Lexing;
using Lupa.Diagnostics;
using Lupa.AST;

namespace Lupa.Parsing
{

    internal class Parser {
        private readonly List<Token> _tokens = new List<Token>();
        private int _position;
        private List<Diagnostic> _diagnostics = new List<Diagnostic>();
        public IEnumerable<Diagnostic> Diagnostics => _diagnostics;
        public Parser(IEnumerable<Token> tokens, IEnumerable<Diagnostic> diagnostics)
        {
            _tokens = tokens.ToList();
            _position = 0;
            _diagnostics.AddRange(diagnostics);
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
                _diagnostics.Add(new Diagnostic(DiagnosticKind.UnexpectedToken, $"Expected token of kind <{kind}>, found <{Current.Kind}> instead.", Current.Position));
                return Advance();
            }
        }


        public SyntaxTree Parse() {
            var root = ParseExpression();
            var eof = Match(TokenKind.Eof);
            
            return new SyntaxTree(root, eof, _diagnostics);
        }

        private Expression ParseExpression() {
            return ParsePrimaryExpression();
        }

        private Expression ParsePrimaryExpression() {
            switch (Current.Kind) {
                case TokenKind.Number:
                    var literalToken = Advance();

                    return new LiteralExpression(literalToken, int.Parse(literalToken.Lexeme));
                default:
                    _diagnostics.Add(new Diagnostic(DiagnosticKind.UnexpectedToken, $"Unexpected token {Current.Kind} at position {Current.Position}.", Current.Position));
                    throw new Exception($"Unexpected token {Current.Kind} at position {Current.Position}.");
            }
        }
    }


}