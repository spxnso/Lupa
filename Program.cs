using System.Linq.Expressions;
using Lupa;
using Lupa.AST;
using Lupa.Binding;
using Lupa.Diagnostics;
using Lupa.Lexing;
using Lupa.Parsing;

public class Program
{
    private static bool ShowTokens = false;
    private static bool ShowTree = false;
    private static bool Try<T>(Func<T> func, out T result, DiagnosticBag diagnostics)
    {
        try
        {
            result = func();
            return true;
        }
        catch (Exception)
        {
            result = default!;
            return false;
        }
    }


    public static void Main(string[] args)
    {
        while (true)
        {
            var defaultColor = Console.ForegroundColor;

            Console.Write("> ");

            var input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input))
            {
                continue;
            }

            if (input.StartsWith("/"))
            {
                HandleCommand(input);
                continue;
            }




            var diagnostics = new DiagnosticBag();

            var lexer = new Lexer(input, diagnostics);
            var tokens = lexer.Lex();


            if (ShowTokens)
            {
                Console.WriteLine();
                Printer.PrintTokens(tokens);
            }


            var parser = new Parser(tokens, diagnostics);
            SyntaxTree syntaxTree;


            
            if (!Try(() => parser.Parse(), out syntaxTree, diagnostics))
            {
                Console.WriteLine();
                Printer.PrintDiagnostics(diagnostics);
                continue;
            }


            if (ShowTree)
            {
                Console.WriteLine();
                Printer.PrintSyntaxTree(syntaxTree);
            }
            


            var binder = new Binder(diagnostics);
            BoundExpression boundRoot;

            if (!Try(() => binder.BindExpression(syntaxTree.Root), out boundRoot, diagnostics))
            {
                Console.WriteLine();
                Printer.PrintDiagnostics(diagnostics);
                continue;
            }

            Printer.PrintDiagnostics(diagnostics);
            if (!diagnostics.Any())
            {

                var evaluator = new Evaluator(boundRoot);

                Console.WriteLine(evaluator.Evaluate());
            }

            Console.ForegroundColor = defaultColor;
        }
    }

    private static void HandleCommand(string command)
    {
        var rawCommand = command.Replace("/", "").ToLower();

        switch (rawCommand)
        {
            case "clear":
                Console.Clear();
                break;
            case "tokens":
                ShowTokens = !ShowTokens;
                Console.WriteLine($"Show tokens: {ShowTokens}");
                break;
            case "tree":
                ShowTree = !ShowTree;
                Console.WriteLine($"Show tree: {ShowTree}");
                break;
            default:
                Console.WriteLine($"Unknown command: {command}");
                break;
        }
    }
}