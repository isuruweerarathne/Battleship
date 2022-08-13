using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship
{
    class ShipOperation
    {
        Random random = new Random();
        //size of ships
        private const int shipAlpha = 2;
        private const int shipBeta = 4;
        private const int submarineAlpha = 3;
        private const int submarineBeta = 3;
        private const int aircraftCarrier = 5;

        //Generate the postions in the constructor
        public ShipOperation()
        {
            shipAlphaPositions = AddPosistion(shipAlpha, allShipsPosition);
            shipBetaPositions = AddPosistion(shipBeta, allShipsPosition);
            submarineAlphaPositions = AddPosistion(submarineAlpha, allShipsPosition);
            submarineBetaPositions = AddPosistion(submarineBeta, allShipsPosition);
            aircraftCarrierPositions = AddPosistion(aircraftCarrier, allShipsPosition);
        }        

        public List<Position> shipAlphaPositions { get; set; }
        public List<Position> shipBetaPositions { get; set; }
        public List<Position> submarineAlphaPositions { get; set; }
        public List<Position> submarineBetaPositions { get; set; }
        public List<Position> aircraftCarrierPositions { get; set; }
        public List<Position> allShipsPosition { get; set; } = new List<Position>();
        public List<Position> firePositions { get; set; } = new List<Position>();

        public bool IsShipAlphaSunk { get; set; } = false;
        public bool IsShipAlphaHit { get; set; } = false;
        public bool IsShipBetaSunk { get; set; } = false;
        public bool IsShipBetaHit { get; set; } = false;
        public bool IsSubmarineAlphaSunk { get; set; } = false;
        public bool IsSubmarineAlphaHit { get; set; } = false;
        public bool IsSubmarineBetaSunk { get; set; } = false;
        public bool IsSubmarineBetaHit { get; set; } = false;
        public bool IsAircraftCarrierSunk { get; set; } = false;
        public bool IsAircraftCarrierHit { get; set; } = false;
        public bool IsAllSunken { get; set; } = false;


        public bool CheckShipAlpha { get; set; } = true;
        public bool CheckShipBeta { get; set; } = true;
        public bool CheckSubmarineAlpha { get; set; } = true;
        public bool CheckSubmarineBeta { get; set; } = true;
        public bool CheckAircraftCarrier { get; set; } = true;

        public int shipAlphaCurrentHitCount { get; set; }
        public int shipBetaCurrentHitCount { get; set; }
        public int submarineAlphaCurrentHitCount { get; set; }
        public int submarineBetaCurrentHitCount { get; set; }
        public int aircratfCarrierCurrentHitCount { get; set; }

        public int shipAlphaPreviousHitCount { get; set; }
        public int shipBetaPreviousHitCount { get; set; }
        public int submarineAlphaPreviousHitCount { get; set; }
        public int submarineBetaPreviousHitCount { get; set; }
        public int aircratfCarrierPreviousHitCount { get; set; }        

        public int remainingShipAlpha { get; set; }
        public int remainingShipBeta { get; set; }
        public int remainingSubAlpha { get; set; }
        public int remainingSubBeta { get; set; }
        public int remainingAirCarrier { get; set; }

        public List<Position> AddPosistion(int size, List<Position> AllPosition)
        {
            List<Position> positions = new List<Position>();

            bool IsExist = false;

            do
            {
                positions = GeneratePositionRandomly(size);
                IsExist = positions.Where(AP => AllPosition.Exists(ShipPos => ShipPos.x == AP.x && ShipPos.y == AP.y)).Any();
            }
            while (IsExist);

            AllPosition.AddRange(positions);


            return positions;
        }

        public List<Position> GeneratePositionRandomly(int size)
        {
            List<Position> positions = new List<Position>();
            
            //odd for horizontal and even for vertical
            int direction = random.Next(1, size);

            //pick row and column
            int row = random.Next(1, 11);
            int col = random.Next(1, 11);

            if (direction % 2 != 0)
            {
                //left first, then right
                if (row - size > 0)
                {
                    for (int i = 0; i < size; i++)
                    {
                        Position pos = new Position();
                        pos.x = row - i;
                        pos.y = col;
                        positions.Add(pos);
                    }
                }
                else // row
                {
                    for (int i = 0; i < size; i++)
                    {
                        Position pos = new Position();
                        pos.x = row + i;
                        pos.y = col;
                        positions.Add(pos);
                    }
                }
            }
            else
            {
                //top first, then bottom
                if (col - size > 0)
                {
                    for (int i = 0; i < size; i++)
                    {
                        Position pos = new Position();
                        pos.x = row;
                        pos.y = col - i;
                        positions.Add(pos);
                    }
                }
                else // row
                {
                    for (int i = 0; i < size; i++)
                    {
                        Position pos = new Position();
                        pos.x = row;
                        pos.y = col + i;
                        positions.Add(pos);
                    }
                }
            }
            return positions;
        }

        public ShipOperation Fire()
        {
            Position EnemyShotPos = new Position();
            bool alreadyShot = false;
            do
            {
                EnemyShotPos.x = random.Next(1, 11);
                EnemyShotPos.y = random.Next(1, 11);
                alreadyShot = firePositions.Any(EFP => EFP.x == EnemyShotPos.x && EFP.y == EnemyShotPos.y);
            }
            while (alreadyShot);

            firePositions.Add(EnemyShotPos);
            return this;
        }

        public ShipOperation CheckShipStatus(List<Position> HitPositions)
        {
            shipAlphaCurrentHitCount = shipAlphaPositions.Where(B => HitPositions.Any(H => B.x == H.x && B.y == H.y)).ToList().Count;
            remainingShipAlpha = shipAlphaPositions.Count - shipAlphaCurrentHitCount;

            shipBetaCurrentHitCount = shipBetaPositions.Where(B => HitPositions.Any(H => B.x == H.x && B.y == H.y)).ToList().Count;
            remainingShipBeta = shipBetaPositions.Count - shipBetaCurrentHitCount;

            submarineAlphaCurrentHitCount = submarineAlphaPositions.Where(B => HitPositions.Any(H => B.x == H.x && B.y == H.y)).ToList().Count;
            remainingSubAlpha = submarineAlphaPositions.Count - submarineAlphaCurrentHitCount;

            submarineBetaCurrentHitCount = submarineBetaPositions.Where(B => HitPositions.Any(H => B.x == H.x && B.y == H.y)).ToList().Count;
            remainingSubBeta = submarineBetaPositions.Count - submarineBetaCurrentHitCount;

            aircratfCarrierCurrentHitCount = aircraftCarrierPositions.Where(B => HitPositions.Any(H => B.x == H.x && B.y == H.y)).ToList().Count;
            remainingAirCarrier = aircraftCarrierPositions.Count - aircratfCarrierCurrentHitCount;
            
            //check whether ship cordinates match with hit positions
            IsShipAlphaSunk = shipAlphaPositions.Where(C => !HitPositions.Any(H => C.x == H.x && C.y == H.y)).ToList().Count == 0;
            IsShipBetaSunk = shipBetaPositions.Where(B => !HitPositions.Any(H => B.x == H.x && B.y == H.y)).ToList().Count == 0;
            IsSubmarineAlphaSunk = submarineAlphaPositions.Where(D => !HitPositions.Any(H => D.x == H.x && D.y == H.y)).ToList().Count == 0;
            IsSubmarineBetaSunk = submarineBetaPositions.Where(S => !HitPositions.Any(H => S.x == H.x && S.y == H.y)).ToList().Count == 0;
            IsAircraftCarrierSunk = aircraftCarrierPositions.Where(P => !HitPositions.Any(H => P.x == H.x && P.y == H.y)).ToList().Count == 0;

            if (!IsShipAlphaSunk)
            {
                IsShipAlphaHit = shipAlphaCurrentHitCount > 0 && (shipAlphaPositions.Count == shipAlphaPreviousHitCount);
                shipAlphaPreviousHitCount = remainingShipAlpha;
            }

            if (!IsShipBetaSunk)
            {
                IsShipBetaHit = shipBetaCurrentHitCount > 0 && (shipBetaPositions.Count == shipBetaPreviousHitCount);
                shipBetaPreviousHitCount = remainingShipBeta;
            }

            if (!IsSubmarineAlphaSunk)
            {
                IsSubmarineAlphaHit = submarineAlphaCurrentHitCount > 0 && (submarineAlphaPositions.Count == submarineAlphaPreviousHitCount);
                submarineAlphaPreviousHitCount = remainingSubAlpha;
            }

            if (!IsSubmarineBetaSunk)
            {
                IsSubmarineBetaHit = submarineBetaCurrentHitCount > 0 && (submarineBetaPositions.Count == submarineBetaPreviousHitCount);
                submarineBetaPreviousHitCount = remainingSubBeta;
            }

            if (!IsAircraftCarrierSunk)
            {
                IsAircraftCarrierHit = aircratfCarrierCurrentHitCount > 0 && (aircraftCarrierPositions.Count == aircratfCarrierPreviousHitCount);
                aircratfCarrierPreviousHitCount = remainingAirCarrier;
            }


            IsAllSunken = IsShipAlphaSunk && IsShipBetaSunk && IsSubmarineAlphaSunk && IsSubmarineBetaSunk && IsAircraftCarrierSunk;
            return this;
        }
    }
}
