using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrategyGameDjikstraAlgo
{
    public class Game
    {
        public Unit[,] board; // game board of units and empty tiles (null)
        public Tile[,] tiles; // array of movement costs

        public List<Unit> allUnits;
        public List<Unit> playerUnits;
        public List<Unit> enemyUnits;

        public Game(
            Unit[,] board,
            Tile[,] tiles,
            List<Unit> playerUnits,
            List<Unit> enemyUnits)
        {
            this.board = board;
            this.tiles = tiles;
            this.playerUnits = playerUnits;
            this.enemyUnits = enemyUnits;

            foreach (Unit unit in playerUnits)
            {
                allUnits.Add(unit);
                unit.team = Teams.Player;
            }
            foreach (Unit unit in enemyUnits)
            {
                allUnits.Add(unit);
                unit.team = Teams.Enemy;
            }

            foreach (Unit unit in allUnits)
            {
                unit.board = board;
                unit.tiles = tiles;
                unit.movementPoints = 3;

                board[unit.location.r, unit.location.c] = unit;
            }
        }

        public void PlayGame()
        {
            Console.WriteLine("Game start");

            while (playerUnits.Any(pu => !pu.Dead())
                && enemyUnits.Any(eu => !eu.Dead()))
            {
                foreach (Unit unit in playerUnits.Where(pu => !pu.Dead()))
                {
                    unit.TakeTurn();
                }

                foreach (Unit unit in enemyUnits.Where(eu => !eu.Dead()))
                {
                    unit.TakeTurn();
                }
            }

            Console.WriteLine("Game is over");
        }
    }
}
