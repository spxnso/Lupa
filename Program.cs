using System.Linq.Expressions;
using Lupa;
using Lupa.AST;
using Lupa.Binding;
using Lupa.Diagnostics;
using Lupa.Lexing;
using Lupa.Parsing;

public class Program
{
    public static void Main(string[] args)
    {
        while (true)
        {
            var color = Console.ForegroundColor;
            Console.Write("> ");

            var input = Console.ReadLine();
            if (string.IsNullOrEmpty(input))
            {
                return;
            }
            

            var diagnostics = new DiagnosticBag();
            var lexer = new Lexer(input, diagnostics);
            var tokens = lexer.Lex();

            lexer.Debug();

            Console.WriteLine();
            
            var parser = new Parser(tokens, diagnostics);
            var binder = new Binder(diagnostics);
            var syntaxTree = parser.Parse();
            var boundRoot = binder.BindExpression(syntaxTree.Root);

 
            syntaxTree.Debug();

            Console.WriteLine();
            if (diagnostics.Any())
            {
                Console.ForegroundColor = ConsoleColor.Red;
                foreach (var diagnostic in diagnostics)
                {
                    diagnostic.Debug();
                }

                Console.ForegroundColor = color;
            }
            else
            {

                var evaluator = new Evaluator(boundRoot);

                Console.WriteLine(evaluator.Evaluate());
            }

            Console.ForegroundColor = color;

        

        }
    }
}