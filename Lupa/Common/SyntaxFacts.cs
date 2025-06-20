using System.Globalization;
using Lupa.Lexing;

namespace Lupa
{
    internal static class SyntaxFacts
    {
        private static readonly Dictionary<string, TokenKind> _keywords = new()
        {
            // Lua / Luau shared
            ["and"] = TokenKind.And,
            ["break"] = TokenKind.Reserved,
            ["do"] = TokenKind.Reserved,
            ["else"] = TokenKind.Reserved,
            ["elseif"] = TokenKind.Reserved,
            ["end"] = TokenKind.Reserved,

            ["for"] = TokenKind.Reserved,
            ["function"] = TokenKind.Reserved,
            ["if"] = TokenKind.Reserved,
            ["in"] = TokenKind.Reserved,
            ["local"] = TokenKind.Reserved,
            ["nil"] = TokenKind.Reserved,
            ["not"] = TokenKind.Not,
            ["or"] = TokenKind.Or,
            ["repeat"] = TokenKind.Reserved,
            ["return"] = TokenKind.Reserved,
            ["then"] = TokenKind.Reserved,
            ["true"] = TokenKind.Boolean,
            ["false"] = TokenKind.Boolean,
            ["until"] = TokenKind.Reserved,
            ["while"] = TokenKind.Reserved,
            ["continue"] = TokenKind.Reserved,
            ["type"] = TokenKind.Reserved,
            ["typeof"] = TokenKind.Reserved,
            ["export"] = TokenKind.Reserved,
            ["extends"] = TokenKind.Reserved,
            ["require"] = TokenKind.Reserved,
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
