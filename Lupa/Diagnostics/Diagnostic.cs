using Lupa.Lexing;

namespace Lupa.Diagnostics
{
    public class Diagnostic {
        public Diagnostic(DiagnosticKind kind, string message, TokenPosition position)
        {
            Message = message;
            Position = position;
            Kind = kind;
        }

        public DiagnosticKind Kind { get; }
        public string Message { get; }
        public TokenPosition Position { get; }

        public void Debug() {
            Console.WriteLine(ToString());
        }
        public override string ToString()
        {
            return $"[{Position.Line}:{Position.Column}] {Message}";
        }
    }
}