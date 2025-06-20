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

            Console.ForegroundColor = ConsoleColor.DarkGray;
            foreach (var token in tokens) {
                token.Debug();
            }





            Console.WriteLine();

            var parser = new Parser(tokens, lexer.Diagnostics);
            var syntaxTree = parser.Parse();

            syntaxTree.Debug();
            
            if (parser.Diagnostics.Any()) {
                Console.ForegroundColor = ConsoleColor.Red;
                foreach (var diagnostic in parser.Diagnostics) {
                    diagnostic.Debug();
                }
                
               
            }
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}