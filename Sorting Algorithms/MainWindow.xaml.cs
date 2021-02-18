using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Sorting_Algorithms
{
    enum SortAlgs { Bubble, CocktailShaker, Comb, Insertion, Selection, DoubleSelect, BinaryInsertion, QuickLomuto, QuickHoare, QuickLomutoMedian, Shell};

    public partial class MainWindow : Window
    {
        SolidColorBrush red = new SolidColorBrush(Color.FromRgb(255, 100, 100));
        SolidColorBrush blue = new SolidColorBrush(Color.FromRgb(100, 100, 255));
        SolidColorBrush dblue = new SolidColorBrush(Color.FromRgb(150, 175, 230));

        Brush appBkg = Brushes.White;
        Brush appFrg = Brushes.Black;
        Brush borderColour = Brushes.Gray;

        #region Variables

        bool fillGap = false;

        double[] array;                                             //actual data being sorted
        Line[] Lines;                                               //array of visual bars to represent data values

        Random rand = new Random();
        int thickness = 20;                                         //width of each bar
        double barGap = 0;
        DispatcherTimer SortTimer = new DispatcherTimer();          //timer to control when the sort algorithm increments and the display is updated

        const int minHeight = 15;                                   //minimum height of the bars
        int maxHeight;                                              //maximum height of the bars - dependent on window size

        int numOfElements = 200;                                    //number of values/lines to sort

        double LeftIndent = 200;                                    //how far from the left side of the window to the left-most bar
        double TopIndent = 160;                                     //how far from the top side of the window to the top of the highest possible bar

        #endregion

        Sort currentSort;

        public MainWindow()
        {
            InitializeComponent();

            MinHeight = 400;
            MinWidth = 860;

            SortButtonsScroller.Height = myCanvas.Height;

            BitmapImage iconSrc = new BitmapImage();
            iconSrc.BeginInit();
            iconSrc.UriSource = new Uri(Environment.CurrentDirectory + "/icon.png");
            iconSrc.EndInit();

            this.Icon = iconSrc;

            BarCount.Value = 50;
            SortTimer.Interval = new TimeSpan(0, 0, 0, 0, 20);

            Shuffle();
            SortTimer.Tick += SortTimer_Tick;

            //SwapColours();
        }

        private void SortTimer_Tick(object sender, EventArgs e)
        {
            IterateSort();
            UpdateInfoLabels(currentSort.getComparisons(), currentSort.getSwaps());
        }

        private void IterateSort()
        {
            // performs next iteration of the sort algorithm and displays the array afterwards
            array = currentSort.Run();

            if (currentSort.isFinished())
            {
                Stop();
            }
            else
            {
                int sortI = currentSort.getIPointer();
                int sortJ = currentSort.getJPointer();

                Draw(sortI, sortJ);
            }
        }

        private void Draw(int iPos, int jPos)
        {
            Brush colour;

            for (int i = 0; i < array.Length; i++)
            {
                if (i == iPos)
                {
                    // main pointer
                    colour = red;
                }
                else if (i == jPos)
                {
                    // secondary pointer
                    colour = blue;
                }
                else if (fillGap && (i > iPos && i < jPos || i > jPos && i < iPos))
                {
                    // interval between pointers
                    colour = dblue;
                }
                else
                {
                    // set others based on position in array
                    Color color = Color.FromArgb(255, (byte)(array[i] * 255 / this.Height), (byte)(array[i] * 255 / this.Height), (byte)(array[i] * 255 / this.Height));
                    colour = new SolidColorBrush(color);
                }

                Lines[i].Stroke = colour;
                Lines[i].ToolTip = Math.Round(array[i], 2);
                Lines[i].Y1 = this.Height - 39;
                Lines[i].Y2 = Lines[i].Y1 - array[i];
            }
        }

        private void Swap(int a, int b)
        {
            //swaps array values and lines for indexes a and b in the respected arrays

            double temp = array[a];
            array[a] = array[b];
            array[b] = temp;
        }

        private void ReverseOrder()
        {
            //reverses the array, such that a sorted ascending array would become a sorted descending array

            int num = array.Length - 1;

            for (int i = 0; i < array.Length / 2; i++)
                Swap(i, num - i);

            Draw(-1, -1);
        }

        //public void QuickSort()
        //{
        //    int[] indices = QuickSortsToPerform.Dequeue();
        //    int start = indices[0];
        //    int end = indices[1];

        //    if (start < end)
        //    {
        //        int index = Partition(start, end);
        //        QuickSortsToPerform.Enqueue(new int[] { start, index - 1 });
        //        QuickSortsToPerform.Enqueue(new int[] { index + 1, end });
        //        Draw(start, end);
        //    }
        //}

        //public int Partition(int start, int end)
        //{
        //    double pivotValue = array[end];
        //    int pivotIndex = start;

        //    for (int i = start; i < end; i++)
        //    {
        //        if (array[i] < pivotValue)
        //        {
        //            Swap(i, pivotIndex);
        //            pivotIndex++;
        //        }

        //        Draw(pivotIndex, i);
        //        compareCount++;
        //    }

        //    Swap(pivotIndex, end);
        //    UpdateInfoLabels();

        //    return pivotIndex;
        //}

        #region Start Sort Buttons

        private void StartSort()
        {
            //Shuffle();
            ResetLabels();
            EnableSortButtons(false);
            fillGap = true;
            SortTimer.Start();
        }

        private void EnableSortButtons(bool state)
        {
            BarCount.IsEnabled = state;

            BubbleGroup.IsEnabled = state;
            InsertionGroup.IsEnabled = state;
            SelectionGroup.IsEnabled = state;
            QuickSortGroup.IsEnabled = state;

            DataStyleCheckBoxes.IsEnabled = state;

            ShuffleBtn.IsEnabled = state;
            ReverseBtn.IsEnabled = state;
            StopBtn.IsEnabled = !state;
        }

        private void BubbleBtn_Click(object sender, RoutedEventArgs e)
        {
            currentSort = new BubbleSort(array);
            StartSort();
        }

        private void CocktailBtn_Click(object sender, RoutedEventArgs e)
        {
            currentSort = new CocktailShakerSort(array);
            StartSort();
        }

        private void CombBtn_Click(object sender, RoutedEventArgs e)
        {
            currentSort = new CombSort(array);
            StartSort();
        }

        private void InsertBtn_Click(object sender, RoutedEventArgs e)
        {
            currentSort = new InsertionSort(array);
            StartSort();
        }

        private void ShellBtn_Click(object sender, RoutedEventArgs e)
        {
            currentSort = new ShellSort(array);
            StartSort();
        }

        private void BinaryInsertBtn_Click(object sender, RoutedEventArgs e)
        {
            currentSort = new BinaryInsertion(array);
            StartSort();
        }

        private void SelectBtn_Click(object sender, RoutedEventArgs e)
        {
            currentSort = new SelectionSort(array);
            StartSort();
        }

        private void DoubleSelectBtn_Click(object sender, RoutedEventArgs e)
        {
            currentSort = new DoubleSelectionSort(array);
            StartSort();
        }

        private void QuickLomutoBtn_Click(object sender, RoutedEventArgs e)
        {
            currentSort = new LomutoQuickSort(array);
            StartSort();
        }

        private void QuickLomutoMedianBtn_Click(object sender, RoutedEventArgs e)
        {
            currentSort = new LomutoMedianQuickSort(array);
            StartSort();
        }

        private void QuickHoareBtn_Click(object sender, RoutedEventArgs e)
        {
            currentSort = new HoareQuickSort(array);
            StartSort();
        }

        #endregion

        #region Labels

        private void UpdateInfoLabels(int comps, int swaps)
        {
            ComparesLbl.Content = "Comparisons: " + comps;
            SwapsLbl.Content = "Swaps: " + swaps;

            //TimeLbl.Content = "Time Elapsed: " + time.ToString() + "s";
        }

        private void ResetLabels()
        {
            UpdateInfoLabels(0,0);
        }

        #endregion

        #region Stop
        private void StopBtn_Click(object sender, RoutedEventArgs e)
        {
            Stop();
        }

        private void Stop()
        {
            SortTimer.Stop();
            EnableSortButtons(true);
            Draw(-1, -1);
        }

        #endregion

        #region Bars/Lines Size and Shape

        private void BarCount_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            numOfElements = (int)BarCount.Value;
            CountLbl.Content = "Elements: " + numOfElements;

            InitialiseBarsAndLines();
        }

        private void InitialiseBarsAndLines()
        {
            maxHeight = (int)(this.Height - TopIndent);
            //here in case window has been resizd

            array = new double[numOfElements];
            //creates array to store values to be sorted

            if (Lines != null)
            {
                //if lines exists, remove them from the canvas

                for (int i = 0; i < Lines.Length; i++)
                {
                    myCanvas.Children.Remove(Lines[i]);
                }
            }

            Lines = new Line[numOfElements];
            //creates array of lines to represent the array values

            double actualArea = this.Width - 1.2 * LeftIndent;

            thickness = (int)(actualArea / numOfElements);
            //thickness inversely proportional to the number of bars

            barGap = (actualArea - thickness * array.Length) / (array.Length - 1);

            AssignArrayValues();  

            for (int i = 0; i < Lines.Length; i++)
            {
                Lines[i] = new Line();

                Lines[i].ToolTip = Math.Round(array[i], 4);
                Lines[i].Y1 = this.Height - 39;
                Lines[i].Y2 = Lines[i].Y1 - array[i];

                Lines[i].StrokeThickness = thickness;

                Lines[i].X1 = i * (thickness + barGap) + LeftIndent;
                Lines[i].X2 = Lines[i].X1;

                myCanvas.Children.Add(Lines[i]);
            }

            Draw(-1, -1);
        }

        private void AssignRandomArrayValues()
        {
            //gives each instance a random double between 50 and maxHeight + 50

            for (int i = 0; i < array.Length; i++)
            {
                array[i] = minHeight;
                array[i] += rand.NextDouble() * (maxHeight - minHeight);
            }
        }

        private void AssignLinearArrayValues()
        {
            //gives each instance a random double between 50 and maxHeight + 50

            double deltaH = (maxHeight - minHeight) / array.Length;
            array[0] = deltaH;

            for (int i = 1; i < array.Length; i++)
            {
                array[i] = array[i - 1] + deltaH;
            }
        }

        private void AssignExpArrayValues()
        {
            //assigns height based on exponential function

            double a = 1.8;     //increase to make exponential increase "tighter" to right end
            double b = -0.1;        //decrease magnitude to reduce change as array length increases

            double baseExp = a * Math.Pow(array.Length, b);
            //double baseExp = 1.5277 * Math.Exp(-0.0022 * array.Length);
            //double baseExp = 1.5 + -0.0024 * array.Length;            //only works at extremes

            for (int i = 0; i < array.Length; i++)
            {
                double pow = i + 1 - array.Length;
                double val = Math.Pow(baseExp, pow);

                array[i] = val * (maxHeight - minHeight) + minHeight;
            }
        }

        private void AssignLogArrayValues()
        {
            //assigns height based on natural logarithm function

            const double c = 0.5;
            double max = Math.Log(array.Length + c);

            for (int i = 0; i < array.Length; i++)
            {
                double val = Math.Log(i + c + 1) / max;

                array[i] = val * (maxHeight - minHeight) + minHeight;
            }
        }

        private void AssignTrigArrayValues()
        {
            //assigns height based on cos function

            double a = 4 * Math.PI;
            double b = Math.PI / 2;

            for (int i = 0; i < array.Length; i++)
            {
                double val = 1 + Math.Sin(a * i / array.Length + b);

                array[i] = val * (maxHeight - minHeight) / 2 + minHeight;
            }
        }

        private void AssignArrayValues()
        {
            if (RandomValueCheck == null)
            {
                AssignLinearArrayValues();
            }
            else
            {
                if ((bool)RandomValueCheck.IsChecked)
                {
                    AssignRandomArrayValues();
                }
                else if ((bool)ExpValueCheck.IsChecked)
                {
                    AssignExpArrayValues();
                }
                else if ((bool)LogValueCheck.IsChecked)
                {
                    AssignLogArrayValues();
                }
                else
                {
                    AssignLinearArrayValues();
                }
            }

            for (int i = 0; i < array.Length; i++)
            {
                if (Lines[i] == null)
                {
                    Lines[i] = new Line();
                }

                Lines[i].Y1 = this.Height - 39;
                Lines[i].Y2 = Lines[i].Y1 - array[i];
            }
        }

        private void RandomValueCheck_Checked(object sender, RoutedEventArgs e)
        {
            AssignArrayValues();
            Draw(-1, -1);
        }

        private void LinearValueCheck_Checked(object sender, RoutedEventArgs e)
        {
            AssignArrayValues();
            Draw(-1, -1);
        }

        private void ExpValueCheck_Checked(object sender, RoutedEventArgs e)
        {
            AssignArrayValues();
            Draw(-1, -1);
        }

        private void LogValueCheck_Checked(object sender, RoutedEventArgs e)
        {
            AssignArrayValues();
            Draw(-1, -1);
        }

        #endregion

        #region Shuffle

        private void ShuffleBtn_Click(object sender, RoutedEventArgs e)
        {
            Shuffle();

            ResetLabels();

            EnableSortButtons(true);
        }

        private void ReverseBtn_Click(object sender, RoutedEventArgs e)
        {
            ReverseOrder();
        }

        private void Shuffle()
        {
            //randomly orders all instances in the array

            int j;

            for (int i = 0; i < array.Length; i++)
            {
                j = rand.Next(0, array.Length);

                Swap(i, j);
            }

            Draw(-1, -1);
        }

        #endregion

        private void SwapColours()
        {
            Brush temp = appBkg;
            appBkg = appFrg;
            appFrg = temp;

            UpdateColourScheme();
        }

        private void UpdateColourScheme()
        {
            for (int i = 0; i < myCanvas.Children.Count; i++)
            {
                if (myCanvas.Children[i] is Button)
                {
                    Button b = (Button)myCanvas.Children[i];
                    b.Background = appBkg;
                    b.Foreground = appFrg;
                    myCanvas.Children[i] = b;
                }
            }

            Background = appBkg;
            Foreground = appFrg;
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            InitialiseBarsAndLines();
            SortButtonsScroller.Height = this.Height - 42;
        }
    }
}
