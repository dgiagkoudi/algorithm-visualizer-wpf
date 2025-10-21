using System.Collections.Generic;

namespace AlgorithmVisualizer.Algorithms
{
    /// Breadth-First Search – Οπτικοποίηση με Queue
    public class BFSAlgorithm : IAlgorithm
    {
        private readonly int[,] matrix;
        private readonly int[] visited;
        private readonly Queue<int> queue;
        private int visitCounter = 1;

        public int[,] AdjacencyMatrix => matrix;
        public int[] CurrentArray => visited;
        public bool IsSorted => queue.Count == 0;

        public BFSAlgorithm(int[,] adjacencyMatrix)
        {
            matrix = adjacencyMatrix;
            int n = matrix.GetLength(0);
            visited = new int[n];
            queue = new Queue<int>();
            queue.Enqueue(0);
        }

        public void NextStep()
        {
            if (queue.Count == 0) return;

            int node = queue.Dequeue();
            if (visited[node] == 0)
            {
                visited[node] = visitCounter++;
                for (int i = 0; i < matrix.GetLength(0); i++)
                {
                    if (matrix[node, i] == 1 && visited[i] == 0)
                        queue.Enqueue(i);
                }
            }
        }
    }
}