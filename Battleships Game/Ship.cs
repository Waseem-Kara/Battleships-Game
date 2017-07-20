using System.Collections.Generic;
using System.Linq;

namespace Battleships_Game
{
    public class Ship
    {
        public string Name { get; set; }
        public int Size { get; set; }
        public List<BoardUnit> ShipUnits { get; set; }

        //Check if ship is sunk == 2
        public bool IsSunk => ShipUnits.All(x => x.Value == 2);

        public static readonly List<Ship> Ships = new List<Ship>
        {
            new Ship {Name = "Battleship", Size = 5},
            new Ship {Name = "Destroyer", Size = 4},
            new Ship {Name = "Destroyer", Size = 4}
        };
    }
}
