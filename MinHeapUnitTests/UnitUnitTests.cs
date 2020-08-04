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

        }

        [TestMethod]
        public void TestMove()
        {
            string json = File.ReadAllText(
                @"D:\CS\djikstra-problem\json-exports\strategy-game-export-3.json",
                Encoding.UTF8);

            Tile[,] tiles = MainClass.GetTiles(json);


            
        }

        [TestMethod]
        public void TestAttack()
        {

        }
    }
}
