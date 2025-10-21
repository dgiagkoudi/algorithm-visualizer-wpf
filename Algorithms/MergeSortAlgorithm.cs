using System.Collections.Generic;

namespace AlgorithmVisualizer.Algorithms
{
    /// Merge Sort – O(n log n)
    public class MergeSortAlgorithm : IAlgorithm
    {
        private int[] array;
        private int[] temp;
        private int i, j, k, left, mid, right;
        private int stage;
        private Stack<(int left, int mid, int right, int stage)> stack;
        private bool merging;

        public bool IsSorted { get; private set; }
        public int[] CurrentArray => array;

        public MergeSortAlgorithm(int[] inputArray)
        {
            array = (int[])inputArray.Clone();
            temp = new int[array.Length];
            stack = new Stack<(int, int, int, int)>();
            stack.Push((0, (0 + array.Length - 1) / 2, array.Length - 1, 0));
        }

        public void NextStep()
        {
            if (IsSorted) return;

            if (!merging)
            {
                if (stack.Count == 0)
                {
                    IsSorted = true;
                    return;
                }

                (left, mid, right, stage) = stack.Pop();

                if (left < right)
                {
                    if (stage == 0)
                    {
                        int m = (left + right) / 2;
                        stack.Push((left, m, right, 1));
                        stack.Push((m + 1, (m + 1 + right) / 2, right, 0));
                        stack.Push((left, (left + m) / 2, m, 0));
                    }
                    else
                    {
                        merging = true;
                        i = left;
                        j = mid + 1;
                        k = left;
                    }
                }
            }
            else
            {
                if (i <= mid && j <= right)
                    temp[k++] = (array[i] <= array[j]) ? array[i++] : array[j++];
                else if (i <= mid)
                    temp[k++] = array[i++];
                else if (j <= right)
                    temp[k++] = array[j++];
                else
                {
                    for (int x = left; x <= right; x++)
                        array[x] = temp[x];
                    merging = false;
                }
            }
        }
    }
}