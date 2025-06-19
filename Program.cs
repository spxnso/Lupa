using Lupa.Lexing;

public class Program {
    public static void Main(string[] args) {
        var lexer = new Lexer("5");
        var tokens = lexer.Lex();

        foreach (var token in tokens) {
            Console.WriteLine($"Token: {token.Kind}, Lexeme: '{token.Lexeme}', Position: {token.Position.Line}:{token.Position.Column}");
        }
    }
}