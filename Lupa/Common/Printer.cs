using Lupa.AST;
using Lupa.Diagnostics;
using Lupa.Lexing;


namespace Lupa
{
    internal static class Printer
    {
        private static ConsoleColor defaultColor = Console.ForegroundColor;
        private static ConsoleColor primaryColor = ConsoleColor.DarkGray;
        private static ConsoleColor errorColor = ConsoleColor.Red;


        private static void ResetColor()
        {
            Console.ForegroundColor = defaultColor;
        }

        public static void PrintTokens(IEnumerable<Token> tokens)
        {
            Console.ForegroundColor = primaryColor;
            foreach (var token in tokens)
            {
                token.Debug();
            }
            ResetColor();
        }

        public static void PrintSyntaxTree(SyntaxTree syntaxTree)
        {
            Console.ForegroundColor = primaryColor;
            syntaxTree.Debug();
            ResetColor();
        }
        
        public static void PrintDiagnostics(DiagnosticBag diagnostics)
        {
            if (!diagnostics.Any()) return;

            Console.ForegroundColor = errorColor;
            foreach (var diagnostic in diagnostics)
            {
                diagnostic.Debug();
            }
            ResetColor();
        }
    }
}