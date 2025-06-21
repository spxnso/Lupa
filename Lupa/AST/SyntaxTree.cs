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
            var tree = new Tree("Root");

            Build(Root, tree);

            AnsiConsole.Write(tree);
        }

        private static void Build(AstNode node, IHasTreeNodes parent)
        {
            switch (node)
            {
                case LiteralExpression literalExpr:
                    var literalNode = parent.AddNode("LiteralExpression");
                    literalNode.AddNode($"Value: {literalExpr.Value}");
                    literalNode.AddNode($"Kind: {literalExpr.LiteralToken.Kind}");
                    break;

                case BinaryExpression binExpr:
                    var binNode = parent.AddNode("BinaryExpression");
                    binNode.AddNode($"Operator: {binExpr.OperatorToken.Lexeme}");
                    var leftNode = binNode.AddNode("Left");
                    Build(binExpr.Left, leftNode);
                    var rightNode = binNode.AddNode("Right");
                    Build(binExpr.Right, rightNode);
                    break;

                case UnaryExpression unExpr:
                    var unNode = parent.AddNode("UnaryExpression");
                    unNode.AddNode($"Operator: {unExpr.OperatorToken.Lexeme}");

                    var operandNode = unNode.AddNode("Operand");
                    Build(unExpr.Operand, operandNode);
                    break;

                case ParenthesizedExpression parenExpr:
                    var parenNode = parent.AddNode("ParenthesizedExpression");
                    parenNode.AddNode("(");
                    Build(parenExpr.Expression, parenNode);
                    parenNode.AddNode(")");


                    break;
                default:
                    parent.AddNode("[red](Unknown node type)[/]");
                    break;
            }
        }
    }
}
