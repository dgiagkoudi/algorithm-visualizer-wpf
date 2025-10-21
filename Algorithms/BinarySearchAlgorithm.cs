using System;

namespace AlgorithmVisualizer.Algorithms
{
    /// Binary Search – O(log n)
    public class BinarySearchAlgorithm : IAlgorithm
    {
        private int[] array;
        private int target;
        private int low, high, mid;
        private bool found = false;

        public int Low => low;
        public int High => high;
        public int Mid => mid;

        public bool IsSorted => found || low > high;
        public int[] CurrentArray => array;

        public BinarySearchAlgorithm(int[] inputArray, int targetValue)
        {
            array = (int[])inputArray.Clone();
            Array.Sort(array);
            target = targetValue;
            low = 0;
            high = array.Length - 1;
            mid = -1;
        }

        public void NextStep()
        {
            if (IsSorted) return;

            mid = low + (high - low) / 2;

            if (array[mid] == target)
            {
                found = true;
            }
            else if (array[mid] < target)
            {
                low = mid + 1;
            }
            else
            {
                high = mid - 1;
            }
        }
    }
}