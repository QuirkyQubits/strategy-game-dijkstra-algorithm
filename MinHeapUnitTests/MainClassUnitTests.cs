using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StrategyGameDjikstraAlgo;

namespace MainClassUnitTests
{
    [TestClass]
    public class MainClassUnitTests
    {
        [TestMethod]
        public void TestInitializeTiles()
        {
            Tile[,] tiles = null;

            string json = File.ReadAllText(@"D:\CS\djikstra-problem\json-exports\strategy-game-export-3.json", Encoding.UTF8);

            MainClass.InitializeTiles(out tiles, json);

            Assert.AreEqual(1, tiles[0, 0].movementCost);
            Assert.AreEqual(3, tiles[0, 1].movementCost);
            Assert.AreEqual(1, tiles[1, 0].movementCost);
            Assert.AreEqual(2, tiles[1, 1].movementCost);
        }
    }
}
