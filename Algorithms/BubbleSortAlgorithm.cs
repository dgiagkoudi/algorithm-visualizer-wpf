namespace AlgorithmVisualizer.Algorithms
{
    /// Bubble Sort – απλή ταξινόμηση O(n²)
    public class BubbleSortAlgorithm : IAlgorithm
    {
        private int[] array;
        private int i = 0;
        private int j = 0;
        private bool swapped = false;
        public bool IsSorted { get; private set; } = false;
        public int[] CurrentArray => array;

        public BubbleSortAlgorithm(int[] inputArray)
        {
            array = (int[])inputArray.Clone();
        }

        public void NextStep()
        {
            if (IsSorted) return;

            if (j < array.Length - i - 1)
            {
                if (array[j] > array[j + 1])
                {
                    (array[j], array[j + 1]) = (array[j + 1], array[j]);
                    swapped = true;
                }
                j++;
            }
            else
            {
                if (!swapped) IsSorted = true;
                else
                {
                    i++;
                    j = 0;
                    swapped = false;
                }
            }
        }
    }
}