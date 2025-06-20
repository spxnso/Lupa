namespace Lupa
{
    internal static class CharExtensions
    {
        public static bool IsAsciiLetter(this char c)
        {
            return (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z');
        }

        public static bool IsHexDigit(this char c)
        {
            return char.IsDigit(c) || (c >= 'a' && c <= 'f') || (c >= 'A' && c <= 'F');
        }
    }
}
