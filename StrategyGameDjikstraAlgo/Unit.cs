using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StrategyGameDjikstraAlgo;

namespace StrategyGameDjikstraAlgo
{
    public class Unit
    {
        public UnitTypes unitType;
        public int movementPoints;
        public Stats stats;
        public Coordinate location;
        public string name;
        public Teams team;

        public Unit[,] board;
        public Tile[,] tiles;

        public Unit(
            UnitTypes unitType,
            int movementPoints,
            Stats stats,
            Coordinate location,
            string name,
            Teams team,

            ref Unit[,] board,
            Tile[,] tiles)
        {
            
            this.unitType = unitType;
            this.movementPoints = movementPoints;
            this.stats = stats;
            this.location = location;
            this.name = name;
            this.team = team;

            this.board = board;
            board[location.r, location.c] = this;

            this.tiles = tiles;
        }

        public bool Dead()
        {
            return stats.hp <= 0;
        }

        public void TakeTurn()
        {
            CheckUnitIsNotDead(this);
            Move();
            Attack();
        }

        //moves the unit around the gameBoard
        private void Move() {

            Console.WriteLine("In Unit::Move()");

            CheckUnitIsNotDead(this);

            var djikstraOptions = GameFunctions.FindReachableTiles(
                this,
                tiles);

            Coordinate target = GetUserInputForMovement(djikstraOptions);

            // move there
            if (!target.Equals(location))
            {
                board[location.r, location.c] = null;
                location = target;
                board[location.r, location.c] = this;
            }
        }

        private void Attack() {

            Console.WriteLine("In Unit::Attack()");

            CheckUnitIsNotDead(this);

            HashSet<Coordinate> attackOptions = GameFunctions.FindAttackableTiles(
                this,
                board);

            if (attackOptions.Any())
            {
                Coordinate targetCoord = GetUserInputForAttack(attackOptions);

                Unit unitToAttack = board[targetCoord.r, targetCoord.c];

                CheckTargetToAttackIsNotNull(unitToAttack);

                CheckUnitIsNotDead(unitToAttack);

                unitToAttack.stats.hp -= stats.atk;

                Console.WriteLine($"{name} dealt {stats.atk} damage to {unitToAttack.name}");

                if (unitToAttack.Dead())
                {
                    // remove from board
                    board[targetCoord.r, targetCoord.c] = null;
                }
            }
        }

        private void CheckUnitIsNotDead(Unit unit)
        {
            bool unitIsDead = unit.stats.hp <= 0;
            string errorMessage = "Unit.Attack():: selected unit is already dead";
            Debug.Assert(!unitIsDead, errorMessage);
            if (unitIsDead)
                throw new GameException(errorMessage);

        }

        private void CheckTargetToAttackIsNotNull(Unit unit)
        {
            string errorMessage = "Unit.Attack():: target to attack is null";
            Debug.Assert(unit != null, errorMessage);
            if (unit == null)
                throw new GameException(errorMessage);
        }

        private Coordinate GetUserInputForMovement(
            Dictionary<Coordinate, Tuple<int, List<Coordinate>>> options)
        {
            // enumerate the options, print out to player

            foreach (KeyValuePair<Coordinate, Tuple<int, List<Coordinate>>> entry in options)
            {
                // do something with entry.Value or entry.Key

                Coordinate option = entry.Key;

                int costToOption = entry.Value.Item1;

                List<Coordinate> pathToOption = entry.Value.Item2;

                Console.WriteLine($"{option} ||| cost: {costToOption}, path: {String.Join(", ", pathToOption)} ");
            }

            // let the player choose among the options

            // accept two ints from keyboard separated by a space

            Console.Write("Enter row number: ");
            int r = int.Parse(Console.ReadLine()); //Converts str into the type int

            Console.Write("Enter col number: ");
            int c = int.Parse(Console.ReadLine()); //Converts str into the type int

            Coordinate playerChoice = new Coordinate(r, c);

            // check the coord the player chose
            // is in the hashmap returned by Djikstra
            // before returning

            string errorMessage
                = "Unit.GetUserInputForMovement():: inputted coord not in options";
            Debug.Assert(options.ContainsKey(playerChoice));
            if (!options.ContainsKey(playerChoice))
                throw new GameException(errorMessage);

            return playerChoice;
        }

        private Coordinate GetUserInputForAttack(HashSet<Coordinate> options)
        {
            // enumerate the options, print out to player

            foreach (Coordinate option in options)
            {
                Console.WriteLine($"{option} ||| name of unit: {board[option.r, option.c]}");
            }

            // let the player choose among the options

            // accept two ints from keyboard separated by a space

            Console.Write("Enter row number: ");
            int r = int.Parse(Console.ReadLine()); //Converts str into the type int

            Console.Write("Enter col number: ");
            int c = int.Parse(Console.ReadLine()); //Converts str into the type int

            Coordinate playerChoice = new Coordinate(r, c);

            // check the coord the player chose
            // is in the hashmap returned by Djikstra
            // before returning

            string errorMessage
                = "Unit.GetUserInputForAttack():: inputted coord not in options";
            Debug.Assert(options.Contains(playerChoice));
            if (!options.Contains(playerChoice))
                throw new GameException(errorMessage);

            return playerChoice;
        }

        public override string ToString()
        {
            return $"{name}";
        }
    }
}
