using System;

namespace Battleships_Game
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //initialize the board
            var tile = new Board();

            //Check for game over
            while (tile.AnyShipsLeft())
            {
                //Improve console readability
                Console.Clear();
                Console.WriteLine("Type in the coordinates to hit:");
                var input = Console.ReadLine();

                tile.UserHit(input);
            }
        }
    }
}
