using Lupa.Lexing;
using Lupa.Parsing;

public class Program {
    public static void Main(string[] args) {
        while (true) {
            Console.Write("> ");
        
            var input = Console.ReadLine();
            if (string.IsNullOrEmpty(input)) {
                return;
            }

            var lexer = new Lexer(input);
            var tokens = lexer.Lex();

            lexer.Debug();

            Console.WriteLine();

            var parser = new Parser(tokens, lexer.Diagnostics);
            var syntaxTree = parser.Parse();

            syntaxTree.Debug();
        }
    }
}