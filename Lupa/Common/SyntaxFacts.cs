using System.Globalization;
using Lupa.Lexing;

namespace Lupa
{
    internal static class SyntaxFacts
    {
        private static readonly Dictionary<string, TokenKind> _keywords = new()
        {
            ["nil"] = TokenKind.Nil,
            
            ["true"] = TokenKind.Boolean,
            ["false"] = TokenKind.Boolean,

            ["if"] = TokenKind.If,
            ["then"] = TokenKind.Then,
            ["else"] = TokenKind.Else,
            ["elseif"] = TokenKind.ElseIf,

            ["and"] = TokenKind.And,
            ["or"] = TokenKind.Or,

            ["not"] = TokenKind.Not,
            
            ["break"] = TokenKind.Break,
            ["return"] = TokenKind.Return,
            ["end"] = TokenKind.End,

            ["for"] = TokenKind.For,
            ["function"] = TokenKind.Function,
            ["if"] = TokenKind.If,
            ["in"] = TokenKind.In,
            ["local"] = TokenKind.Local,


            ["do"] = TokenKind.Do,
            ["until"] = TokenKind.Until,
            ["while"] = TokenKind.While,
            ["repeat"] = TokenKind.Repeat,
            ["continue"] = TokenKind.Continue,

            ["type"] = TokenKind.Type,
            ["typeof"] = TokenKind.TypeOf,
            ["export"] = TokenKind.Export,
            ["extends"] = TokenKind.Extends,
            ["require"] = TokenKind.Require,
        };

        public static TokenKind? GetKindFromKeyword(this string keyword)
        {
            return _keywords.TryGetValue(keyword, out var kind) ? kind : (TokenKind?)null;
        }

        public static int GetBinaryOperatorPrecedence(this TokenKind kind)
        {
            switch (kind)
            {
                case TokenKind.Or:
                    return 1;

                case TokenKind.And:
                    return 2;

                case TokenKind.LessThan:
                case TokenKind.GreaterThan:
                case TokenKind.LessEquals:
                case TokenKind.GreaterEquals:
                case TokenKind.NotEquals:
                case TokenKind.EqualEquals:
                    return 3;

                case TokenKind.DotDot:
                    return 4;

                case TokenKind.Plus:
                case TokenKind.Minus:
                    return 5;

                case TokenKind.Star:
                case TokenKind.Slash:
                case TokenKind.SlashSlash:
                case TokenKind.Percent:
                    return 6;

                case TokenKind.Caret:
                    return 8;

                default:
                    return 0;
            }
        }

        public static int GetUnaryOperatorPrecedence(this TokenKind kind)
        {
            switch (kind)
            {
                case TokenKind.Not:
                case TokenKind.Hash:
                case TokenKind.Minus:
                    return 7;

                default:
                    return 0;
            }
        }
        
        public static bool TryParseLuauNumber(this Token numberToken, out double value)
        {
            value = 0;

            if (numberToken.Kind != TokenKind.Number)
                return false;

            var fixedNum = numberToken.Lexeme.Replace("_", "");
            var lower = fixedNum.ToLowerInvariant();

            try
            {
                if (lower.StartsWith("0x"))
                {

                    value = (double)Convert.ToInt64(fixedNum.Substring(2), 16);
                    return true;
                }
                else if (lower.StartsWith("0b"))
                {
                    value = (double)Convert.ToInt64(fixedNum.Substring(2), 2);
                    return true;
                }
                else
                {
                    return double.TryParse(fixedNum, NumberStyles.Float, CultureInfo.InvariantCulture, out value);
                }
            }
            catch
            {
                return false;
            }
        }


    }
}
