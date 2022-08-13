using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship
{
    class Program
    {
        static void Main(string[] args)
        {
            ShipOperation MyShips = new ShipOperation();
            ShipOperation EnemyShips = new ShipOperation();

            Console.WriteLine("Battleship\r");
            Console.WriteLine("------------------------\n");
           
            Dictionary<char, int> Coordinates = PopulateDictionary();                      

            int Game;
            for (Game = 1; Game < 101; Game++)
            {                
                Position position = new Position();
                
                Console.WriteLine("Enter firing position (e.g. A3).");
                string input = Console.ReadLine();
                position = AnalyzeInput(input, Coordinates);

                if (position.x == -1 || position.y == -1)
                {
                    Console.WriteLine("Invalid coordinates!");
                    Game--;
                    continue;
                }

                if (MyShips.firePositions.Any(EFP => EFP.x == position.x && EFP.y == position.y))
                {
                    Console.WriteLine("This coordinate already being shot.");
                    Game--;
                    continue;
                }

                //Randomly fire on behalf enemy
                EnemyShips.Fire();

                var index = MyShips.firePositions.FindIndex(p => p.x == position.x && p.y == position.y);

                if (index == -1)
                    MyShips.firePositions.Add(position);

                Console.Clear();

                MyShips.allShipsPosition.OrderBy(o => o.x).ThenBy(n => n.y).ToList();
                MyShips.CheckShipStatus(EnemyShips.firePositions);
                PrintResult(MyShips, true);

                EnemyShips.allShipsPosition.OrderBy(o => o.x).ThenBy(n => n.y).ToList();
                EnemyShips.CheckShipStatus(MyShips.firePositions);
                PrintResult(EnemyShips, false);

                if (EnemyShips.IsAllSunken || MyShips.IsAllSunken) { break; }

            }
           

            if (EnemyShips.IsAllSunken && !MyShips.IsAllSunken)
            {
                Console.WriteLine("Game Ended, you win.");
            }
            else if (!EnemyShips.IsAllSunken && MyShips.IsAllSunken)
            {
                Console.WriteLine("Game Ended, you lose.");
            }
            else
            {
                Console.WriteLine("Game Ended in a draw.");
            }

            Console.WriteLine("Total steps taken:{0} ", Game);
            Console.ReadLine();

        }        

        static Position AnalyzeInput(string input, Dictionary<char, int> Coordinates)
        {
            Position pos = new Position();

            char[] inputSplit = input.ToUpper().ToCharArray();


            if (inputSplit.Length < 2 || inputSplit.Length > 4)
            {
                return pos;
            }

            if (Coordinates.TryGetValue(inputSplit[0], out int value))
            {
                pos.x = value;
            }
            else
            {
                return pos;
            }


            if (inputSplit.Length == 3)
            {

                if (inputSplit[1] == '1' && inputSplit[2] == '0')
                {
                    pos.y = 10;
                    return pos;
                }
                else
                {
                    return pos;
                }

            }


            if (inputSplit[1] - '0' > 9)
            {
                return pos;
            }
            else
            {
                pos.y = inputSplit[1] - '0';
            }

            return pos;
        }

        static Dictionary<char, int> PopulateDictionary()
        {
            Dictionary<char, int> Coordinate =
                     new Dictionary<char, int>
                     {
                         { 'A', 1 },
                         { 'B', 2 },
                         { 'C', 3 },
                         { 'D', 4 },
                         { 'E', 5 },
                         { 'F', 6 },
                         { 'G', 7 },
                         { 'H', 8 },
                         { 'I', 9 },
                         { 'J', 10 }
                     };

            return Coordinate;
        }

        static void PrintResult(ShipOperation Ships, bool isMyShip)
        {

            string title = isMyShip ? "Your" : "Enemy";

            if (Ships.CheckShipAlpha && Ships.IsShipAlphaSunk)
            {
                Console.WriteLine("{0} {1} has sunk", title, nameof(Ships.shipAlphaPositions));
                Ships.CheckShipAlpha = false;
            }

            if (Ships.CheckShipBeta && Ships.IsShipBetaSunk)
            {
                Console.WriteLine("{0} {1} has sunk", title, nameof(Ships.shipBetaPositions));
                Ships.CheckShipBeta = false;
            }

            if (Ships.CheckSubmarineAlpha && Ships.IsSubmarineAlphaSunk)
            {
                Console.WriteLine("{0} {1} has sunk", title, nameof(Ships.submarineAlphaPositions));
                Ships.CheckSubmarineAlpha = false;
            }

            if (Ships.CheckSubmarineBeta && Ships.IsSubmarineBetaSunk)
            {
                Console.WriteLine("{0} {1} has sunk", title, nameof(Ships.submarineBetaPositions));
                Ships.CheckSubmarineBeta = false;
            }

            if (Ships.CheckAircraftCarrier && Ships.IsAircraftCarrierSunk)
            {
                Console.WriteLine("{0} {1} has sunk", title, nameof(Ships.aircraftCarrierPositions));
                Ships.CheckAircraftCarrier = false;
            }

            if (!Ships.IsShipAlphaSunk && !Ships.IsShipBetaSunk && !Ships.IsSubmarineBetaSunk && !Ships.IsSubmarineAlphaSunk & !Ships.IsAircraftCarrierSunk)
            {
                if (!Ships.IsShipAlphaHit && !Ships.IsShipBetaHit && !Ships.IsSubmarineAlphaHit && !Ships.IsSubmarineBetaHit & !Ships.IsAircraftCarrierHit)
                {
                    Console.WriteLine("{0} attempt unsuccessful", title);
                }
            }

            if (Ships.CheckShipAlpha && Ships.IsShipAlphaHit)
            {
                Console.WriteLine("{0} {1} has been hit", title, nameof(Ships.shipAlphaPositions));
                Ships.IsShipAlphaHit = false;
            }

            if (Ships.CheckShipBeta && Ships.IsShipBetaHit)
            {
                Console.WriteLine("{0} {1} has been hit", title, nameof(Ships.shipBetaPositions));
                Ships.IsShipBetaHit = false;
            }

            if (Ships.CheckSubmarineAlpha && Ships.IsSubmarineAlphaHit)
            {
                Console.WriteLine("{0} {1} has been hit", title, nameof(Ships.submarineAlphaPositions));
                Ships.IsSubmarineAlphaHit = false;
            }

            if (Ships.CheckSubmarineBeta && Ships.IsSubmarineBetaHit)
            {
                Console.WriteLine("{0} {1} has been hit", title, nameof(Ships.submarineBetaPositions));
                Ships.IsSubmarineBetaHit = false;
            }

            if (Ships.CheckAircraftCarrier && Ships.IsAircraftCarrierHit)
            {
                Console.WriteLine("{0} {1} has been hit", title, nameof(Ships.aircraftCarrierPositions));
                Ships.IsAircraftCarrierHit = false;
            }            
        }
    }
}