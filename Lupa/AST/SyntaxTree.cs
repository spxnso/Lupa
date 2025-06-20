using System.Linq.Expressions;
using Lupa.Diagnostics;
using Lupa.Lexing;

namespace Lupa.AST
{
    internal sealed class SyntaxTree
    {
        public IEnumerable<Diagnostic> Diagnostics { get; set; }
        public Expression Root { get; set; }
        public Token Eof { get; set; }

        public SyntaxTree(Expression root, Token eof, IEnumerable<Diagnostic> diagnostics)
        {
            Diagnostics = diagnostics;
            Root = root;
            Eof = eof;
        }

        public void Debug()
        {
            Print(Root);
        }
        private static void Print(Expression node, string indent = "", bool isLast = true)
        {
            if (node == null)
                return;

            var marker = isLast ? "└──" : "├──";

            Console.Write(indent);
            Console.Write(marker);

            switch (node)
            {
                case LiteralExpression literal:
                    Console.WriteLine($"LiteralExpression: {literal.Value}");
                    break;
                default:
                    Console.WriteLine($"UnknownExpression ({node.GetType().Name})");
                    break;
            }

        }
    }
}