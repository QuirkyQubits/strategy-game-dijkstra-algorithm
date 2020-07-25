using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StrategyGameDjikstraAlgo;

namespace MinHeapUnitTests
{
    [TestClass]
    public class MinHeapUnitTests
    {
        [TestMethod]
        public void TestCtor()
        {
            int[] heapArr = { 1, 2, 3, 4, 5, 6 };
            MinHeap<int> pq1 = new MinHeap<int>(heapArr);
            Assert.AreEqual(1, pq1.Peek());
            Assert.AreEqual(6, pq1.GetHeapSize());

            double[] heapArr2 = { -1.0, 2.1, -3.5, 43, -1.5, 60, 0.001 };
            MinHeap<double> pq2 = new MinHeap<double>(heapArr2);
            Assert.IsTrue(Math.Abs(pq2.Peek() - (-3.5)) < 0.0000001);
            Assert.AreEqual(7, pq2.GetHeapSize());

            string[] heapArr3 = { "this", "is", "a", "heap", "of", "strings" };
            MinHeap<string> pq3 = new MinHeap<string>(heapArr3);
            Assert.AreEqual("a", pq3.Peek());
            Assert.AreEqual(6, pq3.GetHeapSize());

            float[] heapArr4 = null;
            try
            {
                MinHeap<float> pq4 = new MinHeap<float>(heapArr4);

                Assert.Fail(
                    "Initializing a minHeap with a null array " +
                    "should have thrown an exception.");
            }
            catch (MinHeapException e) { }

            MinHeap<string> pq5 = new MinHeap<string>();

            try
            {
                pq5.Peek();
                Assert.Fail("Heap underflow should have thrown exception");
            }
            catch (MinHeapException e) { }

            try
            {
                pq5.Pop();
                Assert.Fail("Heap underflow should have thrown exception");
            }
            catch (MinHeapException e) { }

            pq5.Insert("hello");

            Assert.AreEqual(1, pq5.GetHeapSize());
            Assert.AreEqual("hello", pq5.Peek());
            Assert.AreEqual("hello", pq5.Pop());
            Assert.AreEqual(0, pq5.GetHeapSize());
        }

        [TestMethod]
        public void TestPeek()
        {
            int[] heapArr = { 1, 2, 3, 4, 5, 6 };
            MinHeap<int> pq = new MinHeap<int>(heapArr);

            Assert.AreEqual(1, pq.Peek());
        }

        [TestMethod]
        public void TestPop()
        {
            int[] heapArr = { 1, 2, 3, 4, 5, 6 };
            MinHeap<int> pq = new MinHeap<int>(heapArr);

            Assert.AreEqual(1, pq.Pop());
            Assert.AreEqual(2, pq.Pop());
            Assert.AreEqual(3, pq.Peek());
        }

        [TestMethod]
        public void TestInsert()
        {
            int[] heapArr = { 1, 2, 3, 4, 5, 6 };
            MinHeap<int> pq = new MinHeap<int>(heapArr);

            pq.Insert(20);

            for (int i = 20; i >= 7; --i)
                pq.Insert(i);

            Assert.AreEqual(21, pq.GetHeapSize());
            Assert.AreEqual(1, pq.Peek());
        }

        [TestMethod]
        public void TestReferenceTypes()
        {
            Tile t1 = new Tile(1);
            Tile t2 = new Tile(6);
            Tile t3 = new Tile(10);

            Tile[] tileArr = {
                t1,
                t2,
                t3,
            };

            MinHeap<Tile> pq = new MinHeap<Tile>(tileArr);

            Assert.AreEqual(1, pq.Peek().movementCost);

            //t1.movementCost = 100; // destroys min heap property
            //Assert.AreEqual(100, pq.Peek().movementCost);
        }

        [TestMethod]
        public void TestDoubleArraySize()
        {
            int[] heapArr = { };
            MinHeap<int> pq = new MinHeap<int>(heapArr);

            for (int i = 1; i <= 1000; ++i)
                pq.Insert(i);

            Assert.AreEqual(1, pq.Peek());
            Assert.AreEqual(1000, pq.GetHeapSize());

            pq.Insert(0);

            Assert.AreEqual(0, pq.Peek());
        }

        [TestMethod]
        public void TestContains()
        {
            int[] heapArr = { };
            MinHeap<int> pq = new MinHeap<int>(heapArr);

            for (int i = 1; i <= 100; ++i)
                Assert.IsFalse(pq.Contains(i));

            for (int i = 1; i <= 100; ++i)
                pq.Insert(i);

            for (int i = 1; i <= 100; ++i)
                Assert.IsTrue(pq.Contains(i));

            for (int i = 101; i <= 200; ++i)
                Assert.IsFalse(pq.Contains(i));

            Assert.IsTrue(pq.Contains(1));
            Assert.AreEqual(1, pq.Peek());

            pq.Pop();

            Assert.IsFalse(pq.Contains(1));
            Assert.AreEqual(2, pq.Peek());
        }

        [TestMethod]
        public void TestDecreaseKey()
        {
            string[] heapArr = { "this", "is", "a", "heap", "of", "strings" };
            MinHeap<string> pq = new MinHeap<string>(heapArr);

            Assert.AreEqual("a", pq.Peek());
            Assert.AreEqual(6, pq.GetHeapSize());

            // not impemented yet


        }
    }
}
