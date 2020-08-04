using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StrategyGameDjikstraAlgo;

namespace MainClassUnitTests
{
    [TestClass]
    public class UnitUnitTests
    {
        [TestMethod]
        public void TestCtor()
        {
            string json = File.ReadAllText(
                @"D:\CS\djikstra-problem\json-exports\strategy-game-export-3.json",
                Encoding.UTF8);

            Tile[,] tiles = GameFunctions.GetTiles(json);
            int rows = Util.GetRows(tiles);
            int cols = Util.GetCols(tiles);
            Unit[,] board = new Unit[rows, cols];

            Stats unit1Stats = new Stats(10, 100);
            Coordinate unit1Location = new Coordinate(0, 0);
            Unit unit1 = new Unit(
                UnitTypes.Soldier,
                3,
                unit1Stats,
                unit1Location,
                "unit1",
                Teams.Player,
                board,
                tiles);

            Assert.AreEqual(UnitTypes.Soldier, unit1.unitType);
            Assert.AreEqual(3, unit1.movementPoints);
            Assert.AreEqual(new Stats(10, 100), unit1.stats);
            Assert.AreEqual("unit1", unit1.name);
            Assert.AreEqual(Teams.Player, unit1.team);
            Assert.AreEqual(board, unit1.board);
            Assert.AreEqual(tiles, unit1.tiles);

            Assert.AreEqual(unit1, board[unit1.location.r, unit1.location.c]);
        }

        [TestMethod]
        public void TestTakeTurn()
        {

        }
    }
}
