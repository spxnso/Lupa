using System;
using System.Linq.Expressions;
using Lupa.Diagnostics;
using Lupa.Lexing;
using Spectre.Console;

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
            var color = Console.ForegroundColor;
            var tree = new Tree(Root.Kind.ToString());

            Build(Root, tree);

            Console.ForegroundColor = ConsoleColor.DarkGray;
            AnsiConsole.Write(tree);

            Console.WriteLine();

            if (Diagnostics.Any()) {
                Console.ForegroundColor = ConsoleColor.Red;
                foreach (var diagnostic in Diagnostics) {
                    diagnostic.Debug();
                }  
            }

            Console.ForegroundColor = color;
        }

        private static void Build(AstNode node, IHasTreeNodes parent)
        {
            switch (node)
            {
                case LiteralExpression literalExpr:
                    parent.AddNode($"Value: {literalExpr.LiteralToken.Lexeme}");
                    parent.AddNode($"Kind: {literalExpr.LiteralToken.Kind}");
                    break;

                case BinaryExpression binExpr:
                    parent.AddNode($"Operator: {binExpr.OperatorToken.Lexeme}");

                    var leftNode = parent.AddNode("Left");
                    Build(binExpr.Left, leftNode);

                    var rightNode = parent.AddNode("Right");
                    Build(binExpr.Right, rightNode);
                    break;
                case UnaryExpression unExpr:
                    parent.AddNode($"Operator: {unExpr.OperatorToken.Lexeme}");

                    var operandNode = parent.AddNode("Operand");
                    Build(unExpr.Operand, operandNode);
                    break;
                default:
                    parent.AddNode("[red](Unknown node type)[/]");
                    break;
            }
        }
    }
}
