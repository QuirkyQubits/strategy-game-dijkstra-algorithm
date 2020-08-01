using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StrategyGameDjikstraAlgo;

namespace MaxHeapUnitTests
{
    [TestClass]
    public class MaxHeapUnitTests
    {
        [TestMethod]
        public void TestCtor()
        {
            int[] heapArr = { 1, 2, 3, 4, 5, 6 };
            MaxHeap<int> pq1 = new MaxHeap<int>(heapArr);
            Assert.AreEqual(6, pq1.Peek());
            Assert.AreEqual(6, pq1.HeapSize());

            double[] heapArr2 = { -1.0, 2.1, -3.5, 43, -1.5, 60, 0.001 };
            MaxHeap<double> pq2 = new MaxHeap<double>(heapArr2);
            Assert.IsTrue(Math.Abs(pq2.Peek() - 60) < 0.0000001);
            Assert.AreEqual(7, pq2.HeapSize());

            string[] heapArr3 = { "this", "is", "a", "heap", "of", "strings" };
            MaxHeap<string> pq3 = new MaxHeap<string>(heapArr3);
            Assert.AreEqual("this", pq3.Peek());
            Assert.AreEqual(6, pq3.HeapSize());

            float[] heapArr4 = null;
            try
            {
                MaxHeap<float> pq4 = new MaxHeap<float>(heapArr4);

                Assert.Fail(
                    "Initializing a MaxHeap with a null array " +
                    "should have thrown an exception.");
            }
            catch (HeapException e) { }

            MaxHeap<string> pq5 = new MaxHeap<string>();

            try
            {
                pq5.Peek();
                Assert.Fail("Heap underflow should have thrown exception");
            }
            catch (HeapException e) { }

            try
            {
                pq5.Pop();
                Assert.Fail("Heap underflow should have thrown exception");
            }
            catch (HeapException e) { }

            pq5.Insert("hello");

            Assert.AreEqual(1, pq5.HeapSize());
            Assert.AreEqual("hello", pq5.Peek());
            Assert.AreEqual("hello", pq5.Pop());
            Assert.AreEqual(0, pq5.HeapSize());
        }

        [TestMethod]
        public void TestPeek()
        {
            int[] heapArr = { 1, 2, 3, 4, 5, 6 };
            MaxHeap<int> pq = new MaxHeap<int>(heapArr);

            Assert.AreEqual(6, pq.Peek());
        }

        [TestMethod]
        public void TestPop()
        {
            int[] heapArr = { 1, 2, 3, 4, 5, 6 };
            MaxHeap<int> pq = new MaxHeap<int>(heapArr);

            Assert.AreEqual(6, pq.Pop());
            Assert.AreEqual(5, pq.Pop());
            Assert.AreEqual(4, pq.Peek());
        }

        [TestMethod]
        public void TestInsert()
        {
            int[] heapArr = { 1, 2, 3, 4, 5, 6 };
            MaxHeap<int> pq = new MaxHeap<int>(heapArr);

            // pq.Insert(20); // no duplicates in this implementation of Prioirity Queue

            for (int i = 20; i >= 7; --i)
                pq.Insert(i);

            Assert.AreEqual(20, pq.HeapSize());
            Assert.AreEqual(20, pq.Peek());
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

            MaxHeap<Tile> pq = new MaxHeap<Tile>(tileArr);

            Assert.AreEqual(10, pq.Peek().movementCost);

            //t1.movementCost = 100; // destroys min heap property
            //Assert.AreEqual(100, pq.Peek().movementCost);
        }

        /// <summary>
        /// Double array size method no longer in use,
        /// as we switched from using an array to a list,
        /// which automatically doubles its capacity.
        /// </summary>
        [TestMethod]
        public void TestDoubleArraySize()
        {
            int[] heapArr = { };
            MaxHeap<int> pq = new MaxHeap<int>(heapArr);

            for (int i = 1; i <= 1000; ++i)
                pq.Insert(i);

            Assert.AreEqual(1000, pq.Peek());
            Assert.AreEqual(1000, pq.HeapSize());

            pq.Insert(0);

            Assert.AreEqual(1000, pq.Peek());

            pq.Insert(1001);

            Assert.AreEqual(1001, pq.Peek());
        }

        [TestMethod]
        public void TestContains()
        {
            int[] heapArr = { };
            MaxHeap<int> pq = new MaxHeap<int>(heapArr);

            for (int i = 1; i <= 100; ++i)
                Assert.IsFalse(pq.Contains(i));

            for (int i = 1; i <= 100; ++i)
                pq.Insert(i);

            for (int i = 1; i <= 100; ++i)
                Assert.IsTrue(pq.Contains(i));

            for (int i = 101; i <= 200; ++i)
                Assert.IsFalse(pq.Contains(i));

            Assert.IsTrue(pq.Contains(1));
            Assert.AreEqual(100, pq.Peek());

            pq.Pop();

            Assert.IsFalse(pq.Contains(100));
            Assert.AreEqual(99, pq.Peek());
        }

        [TestMethod]
        public void TestIncreaseKey()
        {
            string[] heapArr = { "this", "is", "a", "heap", "of", "strings" };
            MaxHeap<string> pq = new MaxHeap<string>(heapArr);

            Assert.AreEqual("this", pq.Peek());
            Assert.AreEqual(6, pq.HeapSize());

            try
            {
                pq.IncreaseKey("this", "that");
                Assert.Fail("IncreaseKey: new key must be greater than old");
            }
            catch (HeapException e) { }

            pq.IncreaseKey("this", "xylophone");

            Assert.AreEqual("xylophone", pq.Peek());

            Assert.AreEqual("xylophone", pq.Pop());
            Assert.AreEqual("strings", pq.Pop());
            Assert.AreEqual("of", pq.Pop());
            Assert.AreEqual("is", pq.Pop());
            Assert.AreEqual("heap", pq.Pop());

            Assert.AreEqual("a", pq.Peek());
            Assert.AreEqual("a", pq.Pop());

            Assert.AreEqual(0, pq.HeapSize());
        }
    }
}
