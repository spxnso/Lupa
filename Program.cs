using System.Linq.Expressions;
using Lupa;
using Lupa.AST;
using Lupa.Binding;
using Lupa.Lexing;
using Lupa.Parsing;

public class Program
{
    public static void Main(string[] args)
    {
        while (true)
        {
            Console.Write("> ");

            var input = Console.ReadLine();
            if (string.IsNullOrEmpty(input))
            {
                return;
            }

            var lexer = new Lexer(input);
            var tokens = lexer.Lex();

            lexer.Debug();

            Console.WriteLine();

            var parser = new Parser(tokens, lexer.Diagnostics);
            var binder = new Binder();
            var syntaxTree = parser.Parse();


            var boundRoot = binder.BindExpression(syntaxTree.Root);
            syntaxTree.Debug();


            if (binder.Diagnostics.Any())
            {
                Console.ForegroundColor = ConsoleColor.Red;
                foreach (var diagnostic in binder.Diagnostics)
                {
                    diagnostic.Debug();
                }
                Console.ForegroundColor = ConsoleColor.White;
            }
            else
            {
                var evaluator = new Evaluator(boundRoot);

                Console.WriteLine(evaluator.Evaluate());
            }

        

        }
    }
}