using Lupa.Lexing;

namespace Lupa.AST
{
    public sealed class NameExpression : Expression {
        public NameExpression(Token nameToken)
        {
            NameToken = nameToken;
        }

        public Token NameToken { get; private set; }

        public override NodeKind Kind => NodeKind.NameExpression;
    }
}