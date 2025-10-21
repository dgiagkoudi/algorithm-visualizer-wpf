using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Windows.Media.Animation;
using AlgorithmVisualizer; // Για το IAlgorithm
using AlgorithmVisualizer.Algorithms; // Για όλους τους αλγορίθμους

namespace AlgorithmVisualizer
{
    public partial class MainWindow : Window
    {
        private DispatcherTimer timer;
        private IAlgorithm currentAlgorithm;

        public MainWindow()
        {
            InitializeComponent();

            StartButton.Click += StartButton_Click;
            PauseButton.Click += PauseButton_Click;
            StopButton.Click += StopButton_Click;
            SizeSlider.ValueChanged += SizeSlider_ValueChanged;
            SpeedSlider.ValueChanged += SpeedSlider_ValueChanged;
            AlgorithmComboBox.SelectionChanged += AlgorithmComboBox_SelectionChanged;

            timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(SpeedSlider.Value)
            };
            timer.Tick += Timer_Tick;

            AlgorithmComboBox_SelectionChanged(null, null);
        }

        private void SizeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            SizeLabel.Text = ((int)SizeSlider.Value).ToString();
        }

        private void SpeedSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            SpeedLabel.Text = $"{(int)SpeedSlider.Value} ms";
            timer.Interval = TimeSpan.FromMilliseconds(SpeedSlider.Value);
        }

        private void AlgorithmComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AlgorithmComboBox.SelectedItem == null) return;

            string selectedAlgorithm = ((ComboBoxItem)AlgorithmComboBox.SelectedItem).Content.ToString();
            string newDescription;

            switch (selectedAlgorithm)
            {
                case "Bubble Sort":
                    newDescription = "Bubble Sort: Απλή ταξινόμηση με σύγκριση \nδιαδοχικών στοιχείων.";
                    break;

                case "Merge Sort":
                    newDescription = "Merge Sort: Διαχωρισμός και συγχώνευση \nυπο-πινάκων (Divide & Conquer).";
                    break;

                case "Quick Sort":
                    newDescription = "Quick Sort: Επιλογή pivot και διαχωρισμός,\nO(n log n) μέση.";
                    break;

                case "Binary Search":
                    newDescription = "Binary Search: Αναζήτηση σε ταξινομημένο \nπίνακα, O(log n). Highlight low, mid, high.";
                    break;

                case "DFS":
                    newDescription = "DFS: Επίσκεψη κόμβων γράφου κατά βάθος \nμε Stack. Αριθμός επίσκεψης εμφανίζεται.";
                    break;

                case "BFS":
                    newDescription = "BFS: Επίσκεψη κόμβων γράφου κατά πλάτος \nμε Queue. Αριθμός επίσκεψης εμφανίζεται.";
                    break;

                default:
                    newDescription = "Επέλεξε έναν αλγόριθμο για να δεις περιγραφή.";
                    break;
            }

            var fadeOut = new DoubleAnimation(0, TimeSpan.FromMilliseconds(150));
            fadeOut.Completed += (s, _) =>
            {
                DescriptionTextBlock.Text = newDescription;
                var fadeIn = new DoubleAnimation(1, TimeSpan.FromMilliseconds(300));
                DescriptionTextBlock.BeginAnimation(OpacityProperty, fadeIn);
            };
            DescriptionTextBlock.BeginAnimation(OpacityProperty, fadeOut);
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            int size = (int)SizeSlider.Value;
            Random rand = new Random();

            if (AlgorithmComboBox.SelectedItem == null)
            {
                MessageBox.Show("Επέλεξε έναν αλγόριθμο.");
                return;
            }

            string selectedAlgorithm = ((ComboBoxItem)AlgorithmComboBox.SelectedItem).Content.ToString();

            switch (selectedAlgorithm)
            {
                case "Bubble Sort":
                    currentAlgorithm = new BubbleSortAlgorithm(Enumerable.Range(1, size).OrderBy(x => rand.Next()).ToArray());
                    DrawArray(currentAlgorithm.CurrentArray);
                    break;

                case "Merge Sort":
                    currentAlgorithm = new MergeSortAlgorithm(Enumerable.Range(1, size).OrderBy(x => rand.Next()).ToArray());
                    DrawArray(currentAlgorithm.CurrentArray);
                    break;

                case "Quick Sort":
                    currentAlgorithm = new QuickSortAlgorithm(Enumerable.Range(1, size).OrderBy(x => rand.Next()).ToArray());
                    DrawArray(currentAlgorithm.CurrentArray);
                    break;

                case "Binary Search":
                    int[] array = Enumerable.Range(1, size).OrderBy(x => rand.Next()).ToArray();
                    int target = rand.Next(1, size + 1);
                    currentAlgorithm = new BinarySearchAlgorithm(array, target);
                    DrawArray(currentAlgorithm.CurrentArray);
                    MessageBox.Show($"Αναζήτηση για το στοιχείο: {target}");
                    break;

                case "DFS":
                    currentAlgorithm = new DFSAlgorithm(GenerateSampleGraphMatrix(size));
                    DrawGraph(currentAlgorithm.CurrentArray);
                    break;

                case "BFS":
                    currentAlgorithm = new BFSAlgorithm(GenerateSampleGraphMatrix(size));
                    DrawGraph(currentAlgorithm.CurrentArray);
                    break;
            }

            timer.Start();
        }

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            timer.Stop();
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            timer.Stop();
            currentAlgorithm = null;
            VisualizationCanvas.Children.Clear();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (currentAlgorithm == null) return;

            if (currentAlgorithm.IsSorted)
            {
                timer.Stop();
                MessageBox.Show("Η διαδικασία ολοκληρώθηκε!");
                return;
            }

            currentAlgorithm.NextStep();

            if (currentAlgorithm is DFSAlgorithm || currentAlgorithm is BFSAlgorithm)
                DrawGraph(currentAlgorithm.CurrentArray);
            else
                DrawArray(currentAlgorithm.CurrentArray);
        }

        private void DrawArray(int[] array)
        {
            VisualizationCanvas.Children.Clear();
            double w = VisualizationCanvas.ActualWidth;
            double h = VisualizationCanvas.ActualHeight;

            if (w == 0 || h == 0) return;

            int n = array.Length;
            double barWidth = w / n;
            int maxVal = array.Max();

            int low = -1, mid = -1, high = -1;
            if (currentAlgorithm is BinarySearchAlgorithm bin)
            {
                low = bin.Low;
                mid = bin.Mid;
                high = bin.High;
            }

            for (int i = 0; i < n; i++)
            {
                double barHeight = (array[i] / (double)maxVal) * (h - 20);

                var rect = new Rectangle
                {
                    Width = barWidth - 2,
                    Height = barHeight,
                    Fill = Brushes.SteelBlue
                };

                if (i == mid) rect.Fill = Brushes.OrangeRed;
                else if (i == low) rect.Fill = Brushes.Green;
                else if (i == high) rect.Fill = Brushes.Yellow;

                Canvas.SetLeft(rect, i * barWidth);
                Canvas.SetTop(rect, h - barHeight);
                VisualizationCanvas.Children.Add(rect);
            }
        }

        private void DrawGraph(int[] visited)
        {
            VisualizationCanvas.Children.Clear();
            double w = VisualizationCanvas.ActualWidth;
            double h = VisualizationCanvas.ActualHeight;
            int n = visited.Length;
            if (n == 0 || w == 0 || h == 0) return;

            double radius = Math.Min(w, h) / (2 * n);
            double cx = w / 2;
            double cy = h / 2;

            Point[] positions = new Point[n];
            for (int i = 0; i < n; i++)
            {
                double angle = 2 * Math.PI * i / n;
                double x = cx + Math.Cos(angle) * (w / 3);
                double y = cy + Math.Sin(angle) * (h / 3);
                positions[i] = new Point(x, y);
            }

            int[,] matrix = null;
            if (currentAlgorithm is DFSAlgorithm dfsAlg) matrix = dfsAlg.AdjacencyMatrix;
            else if (currentAlgorithm is BFSAlgorithm bfsAlg) matrix = bfsAlg.AdjacencyMatrix;

            if (matrix != null)
            {
                for (int i = 0; i < n; i++)
                {
                    for (int j = i + 1; j < n; j++)
                    {
                        if (matrix[i, j] == 1)
                        {
                            var line = new Line
                            {
                                X1 = positions[i].X,
                                Y1 = positions[i].Y,
                                X2 = positions[j].X,
                                Y2 = positions[j].Y,
                                Stroke = Brushes.Gray,
                                StrokeThickness = 1
                            };
                            VisualizationCanvas.Children.Add(line);
                        }
                    }
                }
            }

            for (int i = 0; i < n; i++)
            {
                var ellipse = new Ellipse
                {
                    Width = radius * 2,
                    Height = radius * 2,
                    Fill = visited[i] > 0 ? (currentAlgorithm is DFSAlgorithm ? Brushes.OrangeRed : Brushes.LightBlue) : Brushes.LightGray,
                    Stroke = Brushes.Black,
                    StrokeThickness = 1
                };
                Canvas.SetLeft(ellipse, positions[i].X - radius);
                Canvas.SetTop(ellipse, positions[i].Y - radius);
                VisualizationCanvas.Children.Add(ellipse);

                if (visited[i] > 0)
                {
                    var label = new TextBlock
                    {
                        Text = visited[i].ToString(),
                        FontWeight = FontWeights.Bold,
                        Foreground = Brushes.Black,
                        FontSize = radius * 0.8
                    };
                    Canvas.SetLeft(label, positions[i].X - radius / 2);
                    Canvas.SetTop(label, positions[i].Y - radius / 2);
                    VisualizationCanvas.Children.Add(label);
                }
            }
        }

        private int[,] GenerateSampleGraphMatrix(int size)
        {
            int[,] matrix = new int[size, size];
            Random rand = new Random();
            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                    matrix[i, j] = (i != j && rand.NextDouble() > 0.7) ? 1 : 0;
            return matrix;
        }
    }
}
