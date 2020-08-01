using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrategyGameDjikstraAlgo
{
    public abstract class Heap<T> where T : IComparable<T>
    {
        protected List<T> heap;
        protected Dictionary<T, int> keyToHeapIndex;

        protected Heap()
        {
            heap = new List<T>();

            keyToHeapIndex = new Dictionary<T, int>();
        }

        protected Heap(IEnumerable<T> collection)
        {
            const string errorMessage
                = "Heap::Heap(): 'collection' parameter must be non-null";

            if (collection == null)
                throw new HeapException(errorMessage);

            Debug.Assert(collection != null, errorMessage);

            heap = new List<T>(collection);

            keyToHeapIndex = new Dictionary<T, int>();

            for (int i = 0; i <= collection.Count() - 1; ++i)
                keyToHeapIndex.Add(collection.ElementAt(i), i); // initial index is just the i

            BuildHeap();
        }

        public bool Contains(T key)
        {
            return keyToHeapIndex.ContainsKey(key);
        }

        public T Peek()
        {
            // check for heap underflow (heap has no elements and trying to peek)
            CheckHeapIndex(1);

            return Get(1);
        }

        public T Pop()
        {
            // check for heap underflow (heap has no elements and trying to pop)
            CheckHeapIndex(HeapSize());

            if (HeapSize() == 1)
            {
                T item = Get(1);
                keyToHeapIndex.Clear();
                heap.Clear();
                return item;
            }
            else
            {
                T lastItem = Get(HeapSize());
                T firstItem = Get(1);

                keyToHeapIndex.Remove(Get(1));

                heap.RemoveAt(listIndex(HeapSize()));

                Set(1, lastItem);

                Heapify(1);

                return firstItem;
            }
        }

        public int HeapSize()
        {
            return heap.Count;
        }

        protected int Parent(int heapIndex)
        {
            const string errorMessage
                = "Heap::Parent(): Specified heapIndex must be >= 1";

            if (heapIndex <= 0)
                throw new HeapException(errorMessage);

            Debug.Assert(heapIndex >= 1, errorMessage);

            return heapIndex >> 1; // floor(i/2)
        }

        protected int Left(int heapIndex)
        {
            const string errorMessage
                = "Heap::Left(): Specified heapIndex must be >= 1";

            if (heapIndex <= 0)
                throw new HeapException(errorMessage);

            Debug.Assert(heapIndex >= 1, errorMessage);

            return heapIndex << 1; // 2i
        }

        protected int Right(int heapIndex)
        {
            const string errorMessage
                = "Heap::Right(): Specified heapIndex must be >= 1";

            if (heapIndex <= 0)
                throw new HeapException(errorMessage);

            Debug.Assert(heapIndex >= 1, errorMessage);

            int iTimes2 = heapIndex << 1;
            int mask = 0x1;

            return iTimes2 ^ mask; // 2i + 1
        }



        /// <summary>
        /// Gets the item at min heap index i.
        /// Inside the class,
        /// use this method to get items instead of accessing the array directly.
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        protected T Get(int heapIndex)
        {
            CheckHeapIndex(heapIndex);

            return heap[listIndex(heapIndex)];
        }

        /// <summary>
        /// Sets the heap node at index "heapIndex" to be "value".
        /// </summary>
        /// <param name="heapIndex"></param>
        /// <param name="value"></param>
        protected void Set(int heapIndex, T value)
        {
            CheckHeapIndex(heapIndex);

            heap[listIndex(heapIndex)] = value;
        }

        /// <summary>
        /// Given a min heap index, return a C# list index.
        /// Indices in the min heap are numbered from 1 to n,
        /// but indices in a C# list are numbered from 0 to n-1.
        /// </summary>
        protected int listIndex(int heapIndex)
        {
            CheckHeapIndex(heapIndex);

            return heapIndex - 1;
        }

        /// <summary>
        /// On any method requiring a heapIndex,
        /// call this helper method for bounds checking the heapIndex
        /// before attempting to execute the rest of the (outer) method.
        /// </summary>
        /// <param name="heapIndex"></param>
        protected void CheckHeapIndex(int heapIndex)
        {
            const string errorMessage =
                "Heap::CheckHeapIndex(): it is required that " +
                "1 <= Heap index <= heapSize";

            if (heapIndex <= 0 || heapIndex > HeapSize())
                throw new HeapException(errorMessage);

            Debug.Assert(heapIndex >= 1 && heapIndex <= HeapSize(), errorMessage);
        }

        protected abstract void Heapify(int heapIndex);
        protected abstract void BuildHeap();
    }
}
