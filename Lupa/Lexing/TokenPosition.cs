
namespace Lupa.Lexing
{
    public struct TokenPosition
    {
        public int Line { get; set; }
        public int Column { get; set; }
        public int Offset { get; set; }

        public TokenPosition(int line, int column, int index)
        {
            Line = line;
            Column = column;
            Offset = index;
        }
    }
}
