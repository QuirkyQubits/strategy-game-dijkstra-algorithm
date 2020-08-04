using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrategyGameDjikstraAlgo
{
    public static class Util
    {
        public static void Swap<T> (ref T lhs, ref T rhs)
        {
            T temp = lhs;
            lhs = rhs;
            rhs = temp;
        }

        public static void Swap<T>(IList<T> list, int indexA, int indexB)
        {
            T tmp = list[indexA];
            list[indexA] = list[indexB];
            list[indexB] = tmp;
        }

        public static int GetRows<T>(T[,] array)
        {
            return array.GetUpperBound(0) - array.GetLowerBound(0) + 1;
        }

        public static int GetCols<T>(T[,] array)
        {
            return array.GetUpperBound(1) - array.GetLowerBound(1) + 1;
        }

        /// <summary>
        /// Returns the file path to the export folder.
        /// </summary>
        /// <returns></returns>
        public static string GetPathToExportFolder()
        {
            return @"D:\CS\djikstra-problem\json-exports";
        }
    }
}
