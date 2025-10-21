using System.Collections.Generic;

namespace AlgorithmVisualizer.Algorithms
{
    /// Depth-First Search – Οπτικοποίηση με Stack
    public class DFSAlgorithm : IAlgorithm
    {
        private int[,] matrix;
        private int[] visited;
        private Stack<int> stack;
        private int visitCounter = 1;

        public int[,] AdjacencyMatrix => matrix;
        public int[] CurrentArray => visited;
        public bool IsSorted => stack.Count == 0;

        public DFSAlgorithm(int[,] adjacencyMatrix)
        {
            matrix = adjacencyMatrix;
            int n = matrix.GetLength(0);
            visited = new int[n];
            stack = new Stack<int>();
            stack.Push(0);
        }

        public void NextStep()
        {
            if (stack.Count == 0) return;

            int node = stack.Pop();
            if (visited[node] == 0)
            {
                visited[node] = visitCounter++;
                for (int i = matrix.GetLength(0) - 1; i >= 0; i--)
                {
                    if (matrix[node, i] == 1 && visited[i] == 0)
                        stack.Push(i);
                }
            }
        }
    }
}