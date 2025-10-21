namespace AlgorithmVisualizer
{
    public interface IAlgorithm
    {
        void NextStep();
        bool IsSorted { get; }
        int[] CurrentArray { get; }
    }
}