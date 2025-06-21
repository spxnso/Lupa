using System.Collections;
using Lupa.Lexing;

namespace Lupa.Diagnostics
{
    public class DiagnosticBag : IEnumerable<Diagnostic>
        {
            private readonly List<Diagnostic> _diagnostics = new();

            public void Add(Diagnostic diagnostic)
            {
                _diagnostics.Add(diagnostic);
            }

            public void Add(DiagnosticKind kind, string message, TokenPosition position)
            {
                Add(new Diagnostic(kind, message, position));
            }

            public void AddRange(IEnumerable<Diagnostic> diagnostics)
            {
                _diagnostics.AddRange(diagnostics);
            }

            public IEnumerator<Diagnostic> GetEnumerator() => _diagnostics.GetEnumerator();
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            public bool Any() => _diagnostics.Count > 0;

            public void Debug()
            {
                foreach (var diagnostic in _diagnostics)
                {
                    Console.WriteLine(diagnostic);
                }
            }
        }
}