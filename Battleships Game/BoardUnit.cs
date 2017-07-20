using System.Collections.Generic;

namespace Battleships_Game
{
    public class BoardUnit
    {
        public int Value { get; set; }
        public string Letter { get; set; }
        public int Number { get; set; }
    }

    public static class Keys
    {
        public static readonly List<string> LetterKeys = new List<string> {"","A", "B", "C", "D", "E", "F", "G", "H", "I", "J"};
    }
}
