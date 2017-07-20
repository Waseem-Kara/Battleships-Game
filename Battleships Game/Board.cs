using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Threading;

namespace Battleships_Game
{
    public class Board
    {
        //stores the values for the game board
        public List<BoardUnit> BoardUnits = new List<BoardUnit>();

        /// <summary>
        ///     Initializes the game tile
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public Board()
        {
            //initialize the game board
            PopulateBoard();

            //position ships
            PositionShips();
        }

        /// <summary>
        /// Populates the game board with specific key value pairs
        /// </summary>
        public void PopulateBoard()
        {
            for (var i = 1; i <= 10; i++)
            for (var j = 1; j <= 10; j++)
                BoardUnits.Add(new BoardUnit { Letter = Keys.LetterKeys.ElementAt(i), Number = j, Value = 0 });
        }

        /// <summary>
        ///     places the initial ships to the game tile
        /// </summary>
        private void PositionShips()
        {
            foreach (var ship in Ship.Ships)
            {
                //Find available positon for a ship to be placed
                var units = GetAvailablePosition(ship);
                ship.ShipUnits = units;

                //Place ship at every unit co-ordinate
                foreach (var unit in units)
                    BoardUnits.Single(x => x.Letter == unit.Letter && x.Number == unit.Number).Value = 1;
            }
        }

        /// <summary>
        ///     Returns available location for the ship to be placed
        /// </summary>
        private List<BoardUnit> GetAvailablePosition(Ship ship)
        {
            var values = new List<BoardUnit>();
            var isValidLocation = false;
            var random = new Random();

            while (!isValidLocation)
            {
                //Create random co-ordinate
                var unit = GetRandomUnit(ship);


                if (random.NextDouble() >= 0.5)
                {
                    if (BoardUnits.Any(x => x.Letter == unit.Letter && x.Number == unit.Number && x.Value != 0) ||
                        unit.Number + ship.Size > 10) continue;
                    
                    for (var i = unit.Number; i < unit.Number + ship.Size; i++)
                    {
                        if (BoardUnits.Single(x => x.Letter == unit.Letter && x.Number == i).Value != 0)
                        {
                            values.Clear();
                            isValidLocation = false;
                        }
                        else
                        {
                            values.Add(new BoardUnit { Letter = unit.Letter, Number = i, Value = 1 });
                            isValidLocation = true;
                        }
                    }
                    
                }
                else
                {
                    if (BoardUnits.Any(x => x.Letter == unit.Letter && x.Number == unit.Number && x.Value != 0) ||
                        Keys.LetterKeys.IndexOf(unit.Letter) + ship.Size > 10) continue;
                    
                        for (var i = Keys.LetterKeys.IndexOf(unit.Letter);
                            i < Keys.LetterKeys.IndexOf(unit.Letter) + ship.Size;
                            i++)
                        {
                            if (BoardUnits.Single(x => x.Letter == Keys.LetterKeys.ElementAt(i) &&
                                                       x.Number == unit.Number)
                                    .Value != 0)
                            {
                                values.Clear();
                                isValidLocation = false;
                            }
                            else
                            {
                                values.Add(new BoardUnit { Letter = Keys.LetterKeys.ElementAt(i), Number = unit.Number, Value = 1 });
                                isValidLocation = true;
                            }
                        }
                    
                }
              
            }

            return values;
        }

        /// <summary>
        ///     Returns true if there are any not destroyed ships left, otherwise false
        /// </summary>
        /// <returns></returns>
        public bool AnyShipsLeft()
        {
            return Ship.Ships.Any(x => !x.IsSunk);
        }

        /// <summary>
        ///     Hits the coordinate entered by user
        /// </summary>
        /// <param name="input"></param>
        public void UserHit(string input)
        {
            try
            {
                #region :INPUT VALIDATION:
                if (string.IsNullOrWhiteSpace(input)) throw new InvalidInputException();
                var letter = input[0].ToString().ToUpper();
                int number;

                int.TryParse(input[1].ToString(), out number);
                if (Keys.LetterKeys.All(x => x != letter)) throw new InvalidInputException();
                if (number < 1 || number > 10) throw new InvalidInputException();

                #endregion

                //Check if input matches the co-ordinate of a ship
                if (BoardUnits.Any(x => x.Letter == letter && x.Number == number && x.Value == 1))
                {
                    //Change co-ordinate to 2 representing a hit
                    BoardUnits.Single(x => x.Letter == letter && x.Number == number && x.Value == 1).Value = 2;

                    var ship = Ship.Ships.Single(x => x.ShipUnits.Any(y => y.Letter == letter && y.Number == number && y.Value == 1));
                    ship.ShipUnits.Single(x => x.Letter == letter && x.Number == number && x.Value == 1).Value = 2;

                    Console.Clear();
                    Console.WriteLine("Successfull hit for user!");
                    if (ship.IsSunk)
                    {
                        Console.WriteLine("Ship sunk! : {0}", ship.Name);
                    }
                    Console.WriteLine("Press enter to play again!");
                    Console.ReadLine();
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Missed! Computer is playing!");
                    Thread.Sleep(2000);
                    ComputerHit();
                }
               
            }
            catch (InvalidInputException)
            {
                Console.Clear();
                Console.WriteLine("Invalid coordinate, please try again");
                Thread.Sleep(2500);
            }
        }

        /// <summary>
        /// Hits random unit
        /// </summary>
        private void ComputerHit()
        {
            //Generate random number and letter between 1-10
            var random = new Random();
            var letter =  Keys.LetterKeys.ElementAt(random.Next(1, 10));
            var number = random.Next(1, 10);
            //Check if AI hit
            var isHit = true;

            while (isHit)
            {
                if (BoardUnits.Any(x => x.Letter == letter && x.Number == number && x.Value == 1))
                {
                    BoardUnits.Single(x => x.Letter == letter && x.Number == number && x.Value == 1).Value = 2;
                    var ship = Ship.Ships.Single(x => x.ShipUnits.Any(y => y.Letter == letter && y.Number == number && y.Value == 1));
                    ship.ShipUnits.Single(x => x.Letter == letter && x.Number == number && x.Value == 1).Value = 2;

                    Console.Clear();
                    Console.WriteLine("Successfull hit for computer! {0}{1}", letter, number);
                    if (ship.IsSunk)
                    {
                        Console.WriteLine("Ship sunk! : {0}", ship.Name);
                    }
                    Thread.Sleep(1000);
                }
                else
                {
                    isHit = false;
                    Console.Clear();
                    Console.WriteLine("Computer missed! User is playing!");
                    Thread.Sleep(2000);
                }
            }

        }

        #region :PROCESSING FUNCTIONS:

        /// <summary>
        ///     Generates random game board coordinates
        /// </summary>
        /// <returns></returns>
        private BoardUnit GetRandomUnit(Ship ship)
        {
            var random = new Random();
            var randomRow = random.Next(1, 10);
            var randomCol = random.Next(1, 10);
            return new BoardUnit {Letter = Keys.LetterKeys.ElementAt(randomRow), Number = randomCol, Value = 0};
        }

        #endregion
    }
}
