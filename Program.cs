using Lupa.Lexing;

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
            Console.ForegroundColor = ConsoleColor.White;


            Console.WriteLine();
            if (lexer.Diagnostics.Any()) {
                Console.ForegroundColor = ConsoleColor.Red;
                foreach (var diagnostic in lexer.Diagnostics) {
                    diagnostic.Debug();
                }
                Console.ForegroundColor = ConsoleColor.White;
            }
        }
    }
}