using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StrategyGameDjikstraAlgo;

namespace StrategyGameDjikstraAlgo
{
    [Serializable()]
    public class MinHeapException : System.Exception
    {
        public MinHeapException() : base() { }
        public MinHeapException(string message) : base(message) { }
        public MinHeapException(string message, System.Exception inner)
            : base(message, inner) { }

        // A constructor is needed for serialization when an
        // exception propagates from a remoting server to the client.
        protected MinHeapException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }

    public class MinHeap<T> where T : IComparable<T>
    {
        private T[] arr;
        private int heapSize;
        private Dictionary<T, int> keyToHeapIndex;

        public MinHeap()
        {
            arr = new T[10];
            heapSize = 0;

            keyToHeapIndex = new Dictionary<T, int>();
        }

        public MinHeap(T[] arr)
        {
            const string errorMessage
                = "MinHeap::MinHeap(): 'arr' parameter must be non-null";

            if (arr == null)
                throw new MinHeapException(errorMessage);

            Debug.Assert(arr != null, errorMessage);

            if (arr.Length == 0)
                this.arr = new T[10];
            else
                this.arr = new T[arr.Length * 4];

            heapSize = arr.Length;

            keyToHeapIndex = new Dictionary<T, int>();

            Array.Copy(arr, this.arr, arr.Length);

            for (int i = 0; i <= arr.Length - 1; ++i)
            {
                T element = arr[i];
                keyToHeapIndex.Add(element, i);
            }

            BuildMinHeap();
        }

        public int GetHeapSize()
        {
            return heapSize;
        }

        private int Parent(int heapIndex)
        {
            const string errorMessage
                = "MinHeap::Parent(): Specified minHeapIndex must be >= 1";

            if (heapIndex <= 0)
                throw new MinHeapException(errorMessage);

            Debug.Assert(heapIndex >= 1, errorMessage);

            return heapIndex >> 1; // floor(i/2)
        }

        private int Left(int heapIndex)
        {
            const string errorMessage
                = "MinHeap::Left(): Specified minHeapIndex must be >= 1";

            if (heapIndex <= 0)
                throw new MinHeapException(errorMessage);

            Debug.Assert(heapIndex >= 1, errorMessage);

            return heapIndex << 1; // 2i
        }

        private int Right(int heapIndex)
        {
            const string errorMessage
                = "MinHeap::Right(): Specified minHeapIndex must be >= 1";

            if (heapIndex <= 0)
                throw new MinHeapException(errorMessage);

            Debug.Assert(heapIndex >= 1, errorMessage);

            int iTimes2 = heapIndex << 1;
            int mask = 0x1;

            return iTimes2 ^ mask; // 2i + 1
        }

        /// <summary>
        /// Assumes that the binary trees rooted at
        /// Left(minHeapIndex) and Right(minHeapIndex)
        /// are min heaps, but that minHeap(minHeapIndex)
        /// might be greater than its children.
        /// MinHeapify lets the value at minHeap(minHeapIndex) "float down"
        /// so that the minHeap obeys the minHeap property.
        /// Running time of this algorithm is O(lg n) or O(h).
        /// </summary>
        /// <param name="heapIndex"></param>
        private void MinHeapify(int heapIndex)
        {
            const string errorMessage
                = "MinHeap::MinHeapify(): Specified minHeapIndex must be >= 1";

            if (heapIndex <= 0)
                throw new MinHeapException(errorMessage);

            Debug.Assert(heapIndex >= 1, errorMessage);

            int l = Left(heapIndex);
            int r = Right(heapIndex);

            int smallest;

            if (l <= heapSize && Get(l).CompareTo(Get(heapIndex)) < 0)
                smallest = l;
            else
                smallest = heapIndex;

            if (r <= heapSize && Get(r).CompareTo(Get(smallest)) < 0)
                smallest = r;

            if (smallest != heapIndex)
            {
                Util.Swap(
                    ref arr[arrayIndex(heapIndex)],
                    ref arr[arrayIndex(smallest)]);

                MinHeapify(smallest);
            }
        }

        /// <summary>
        /// Assumes that arr and length are set, but arr might not be a min heap.
        /// Makes arr a min heap.
        /// The running time of this heapify algorithm is O(n).
        /// </summary>
        private void BuildMinHeap()
        {
            int firstNonLeafIndex = (int)Math.Floor((double)heapSize / 2);

            for (int heapIndex = firstNonLeafIndex; heapIndex >= 1; --heapIndex)
                MinHeapify(heapIndex);
        }

        private void DoubleArraySize()
        {
            T[] oldArr = arr;
            arr = new T[oldArr.Length * 2];
            Array.Copy(oldArr, arr, oldArr.Length);
        }

        public bool Contains(T key)
        {
            if (heapSize == 0)
                return false;

            return keyToHeapIndex.ContainsKey(key);
        }

        public void Insert(T key)
        {
            // keyToHeapIndex.Add(key);

            heapSize++;

            if (heapSize > arr.Length)
                DoubleArraySize();

            int tempHeapIndex = heapSize;

            Set(tempHeapIndex, key);

            while (tempHeapIndex > 1
                && Get(Parent(tempHeapIndex)).CompareTo(Get(tempHeapIndex)) > 0)
            {
                Util.Swap(
                    ref arr[arrayIndex(tempHeapIndex)],
                    ref arr[arrayIndex(Parent(tempHeapIndex))]);

                tempHeapIndex = Parent(tempHeapIndex);
            }
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
            CheckHeapIndex(heapSize);

            set.Remove(Get(1));

            T min = Get(1);

            Set(1, Get(heapSize));

            heapSize--;

            MinHeapify(1);

            return min;
        }

        public void DecreaseKey(int heapIndex, T key)
        {
            CheckHeapIndex(heapIndex);

            const string errorMessage
                = "MinHeap::HeapDecreaseKey(): New key is larger than old key";

            if (key.CompareTo(Get(heapIndex)) > 0)
                throw new MinHeapException(errorMessage);

            Debug.Assert(key.CompareTo(Get(heapIndex)) <= 0, errorMessage);

            Set(heapIndex, key);

            while (heapIndex > 1
                && Get(Parent(heapIndex)).CompareTo(Get(heapIndex)) > 0) {
                Util.Swap(
                    ref arr[arrayIndex(heapIndex)],
                    ref arr[arrayIndex(Parent(heapIndex))]);
                heapIndex = Parent(heapIndex);
            }
        }

        // Do we need this method?
        /*
        private void IncreaseKey(int heapIndex, T key)
        {
            CheckHeapIndex(heapIndex);

            const string errorMessage
                = "MinHeap::HeapIncreaseKey(): New key is smaller than old key";

            if (key.CompareTo(Get(heapIndex)) < 0)
                throw new MinHeapException(errorMessage);

            Debug.Assert(key.CompareTo(Get(heapIndex)) >= 0, errorMessage);

            Set(heapIndex, key);

            MinHeapify(heapIndex);
        }
        */

        /// <summary>
        /// Gets the item at min heap index i.
        /// Inside the class,
        /// use this method to get items instead of accessing the array directly.
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        private T Get(int heapIndex)
        {
            CheckHeapIndex(heapIndex);

            return arr[arrayIndex(heapIndex)];
        }

        /// <summary>
        /// Sets the heap node at index "minHeapIndex" to be "value".
        /// </summary>
        /// <param name="heapIndex"></param>
        /// <param name="value"></param>
        private void Set(int heapIndex, T value)
        {
            CheckHeapIndex(heapIndex);

            arr[arrayIndex(heapIndex)] = value;
        }

        /// <summary>
        /// Given a min heap index, return a C# array index.
        /// Indices in the min heap are numbered from 1 to n,
        /// but indices in a C# array are numbered from 0 to n-1.
        /// </summary>
        private int arrayIndex(int heapIndex)
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
        private void CheckHeapIndex(int heapIndex)
        {
            const string errorMessage =
                "MinHeap::CheckHeapIndex(): it is required that " +
                "1 <= Heap index <= heapSize";

            if (heapIndex <= 0 || heapIndex >= heapSize + 1)
                throw new MinHeapException(errorMessage);

            Debug.Assert(heapIndex >= 1 && heapIndex <= heapSize, errorMessage);
        }
    }
}
