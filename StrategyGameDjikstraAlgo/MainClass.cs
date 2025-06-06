using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrategyGameDjikstraAlgo
{
    class MainClass
    {
        private static void GameTest()
        {
            string json = File.ReadAllText(
                @"D:\CS\djikstra-problem\json-exports\strategy-game-export.json",
                Encoding.UTF8);

            Tile[,] tiles = GameFunctions.GetTiles(json);
            int rows = Util.GetRows(tiles);
            int cols = Util.GetCols(tiles);
            Unit[,] board = new Unit[rows, cols];

            Unit playerUnit1 = new Unit(
                UnitTypes.Soldier,
                3,
                new Stats(10, 100),
                new Coordinate(0, 1),
                "player-unit-1",
                Teams.Player,
                ref board,
                tiles);

            Unit playerUnit2 = new Unit(
                UnitTypes.Soldier,
                3,
                new Stats(10, 100),
                new Coordinate(1, 1),
                "player-unit-2",
                Teams.Player,
                ref board,
                tiles);

            Unit playerUnit3 = new Unit(
                UnitTypes.Soldier,
                3,
                new Stats(10, 100),
                new Coordinate(2, 1),
                "player-unit-3",
                Teams.Player,
                ref board,
                tiles);

            Unit playerUnit4 = new Unit(
                UnitTypes.Soldier,
                3,
                new Stats(10, 100),
                new Coordinate(1, 2),
                "player-unit-4",
                Teams.Player,
                ref board,
                tiles);

            Unit enemyUnit1 = new Unit(
                UnitTypes.Seaman,
                5,
                new Stats(30, 70),
                new Coordinate(1, 6),
                "enemy-unit-1",
                Teams.Enemy,
                ref board,
                tiles);

            Unit enemyUnit2 = new Unit(
                UnitTypes.Seaman,
                5,
                new Stats(30, 70),
                new Coordinate(2, 6),
                "enemy-unit-2",
                Teams.Enemy,
                ref board,
                tiles);

            Unit enemyUnit3 = new Unit(
                UnitTypes.Seaman,
                5,
                new Stats(30, 70),
                new Coordinate(3, 6),
                "enemy-unit-3",
                Teams.Enemy,
                ref board,
                tiles);

            Unit enemyUnit4 = new Unit(
                UnitTypes.Seaman,
                5,
                new Stats(30, 70),
                new Coordinate(2, 5),
                "enemy-unit-4",
                Teams.Enemy,
                ref board,
                tiles);

            Unit enemyUnit5 = new Unit(
                UnitTypes.Seaman,
                5,
                new Stats(30, 70),
                new Coordinate(3, 5),
                "enemy-unit-5",
                Teams.Enemy,
                ref board,
                tiles);

            List<Unit> playerUnits = new List<Unit>();
            List<Unit> enemyUnits = new List<Unit>();

            playerUnits.Add(playerUnit1);
            playerUnits.Add(playerUnit2);
            playerUnits.Add(playerUnit3);
            playerUnits.Add(playerUnit4);

            enemyUnits.Add(enemyUnit1);
            enemyUnits.Add(enemyUnit2);
            enemyUnits.Add(enemyUnit3);
            enemyUnits.Add(enemyUnit4);
            enemyUnits.Add(enemyUnit5);

            Game game = new Game(board, tiles, playerUnits, enemyUnits);

            game.PlayGame();
        }

        public static void Main() { 
            GameTest();
        }
    }
}
