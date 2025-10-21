using System.Collections.Generic;

namespace AlgorithmVisualizer.Algorithms
{
    /// Quick Sort – O(n log n) μέση, O(n²) χειρότερη
    public class QuickSortAlgorithm : IAlgorithm
    {
        private int[] array;
        private Stack<(int low, int high)> stack;
        private bool partitioning;
        private int low, high, pivotIndex, i, j;

        public bool IsSorted { get; private set; }
        public int[] CurrentArray => array;

        public QuickSortAlgorithm(int[] inputArray)
        {
            array = (int[])inputArray.Clone();
            stack = new Stack<(int, int)>();
            stack.Push((0, array.Length - 1));
        }

        public void NextStep()
        {
            if (IsSorted) return;

            if (!partitioning)
            {
                if (stack.Count == 0)
                {
                    IsSorted = true;
                    return;
                }

                (low, high) = stack.Pop();
                if (low >= high) return;

                pivotIndex = high;
                i = low - 1;
                j = low;
                partitioning = true;
            }
            else
            {
                if (j < pivotIndex)
                {
                    if (array[j] <= array[pivotIndex])
                    {
                        i++;
                        (array[i], array[j]) = (array[j], array[i]);
                    }
                    j++;
                }
                else
                {
                    i++;
                    (array[i], array[pivotIndex]) = (array[pivotIndex], array[i]);

                    if (low < i - 1) stack.Push((low, i - 1));
                    if (i + 1 < high) stack.Push((i + 1, high));
                    partitioning = false;
                }
            }
        }
    }
}