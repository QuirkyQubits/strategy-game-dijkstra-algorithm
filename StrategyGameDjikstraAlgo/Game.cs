using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrategyGameDjikstraAlgo
{
    public class Game
    {
        public List<Unit> playerUnits;
        public List<Unit> enemyUnits;

        public Unit[,] board;

        public void PlayGame()
        {
            Console.WriteLine("Game start");

            while (playerUnits.Any(pu => !pu.Dead())
                && enemyUnits.Any(eu => !eu.Dead()))
            {
                foreach (Unit unit in playerUnits)
                {
                    unit.TakeTurn();
                }

                foreach (Unit unit in enemyUnits)
                {
                    unit.TakeTurn();
                }
            }

            Console.WriteLine("Game is over");
        }
    }
}
