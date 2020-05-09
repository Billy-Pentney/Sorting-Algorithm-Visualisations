using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;
using System.Windows.Threading;
using System.Resources;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Windows.Media.Animation;

namespace Sorting_Algorithms
{
    enum SortAlgs { Bubble, Insert, Select, BinaryInsert, Quick};

    public partial class MainWindow : Window
    {
        SolidColorBrush red = new SolidColorBrush(Color.FromRgb((byte)255, (byte)100, (byte)100));
        SolidColorBrush blue = new SolidColorBrush(Color.FromRgb((byte)100, (byte)100, (byte)255));

        double[] array;     //actual data being sorted
        Line[] Lines;   //array of visual lines to represent data values
        Random rand = new Random();
        int thickness = 20;
        DispatcherTimer SortTimer = new DispatcherTimer();

        int SortI = 0;
        int SortJ = 0;
        int minIndex = -1;

        const int minHeight = 20;
        int maxHeight;

        int numOfElements = 200;

        double LeftIndent = 100;
        double TopIndent = 150;

        int compareCount = 0;
        int swapCount = 0;

        int IndexToInsert = 0;

        Queue<int[]> QuickSortsToPerform = new Queue<int[]>();

        int currentEnd = 0;
        int currentStart = 0;
        int pivotIndex = 0;
        double pivotValue = 0;

        double time = 0;
        //time since sort started in milliseconds
        double interval;
        //time interval for timer.tick in milliseconds

        SortAlgs runningAlgorithm;
        //represents which algorithm is currently being performed

        public MainWindow()
        {
            InitializeComponent();

            maxHeight = (int)(this.Height - TopIndent) - 50;
            //maximum height for the bars

            BarCount.Value = 50;
            SortTimer.Interval = new TimeSpan(0, 0, 0, 0, thickness);

            Shuffle();
            SortTimer.Tick += SortTimer_Tick;
        }

        private void SortTimer_Tick(object sender, EventArgs e)
        {
            IterateSort();
            //time += interval;
            //UpdateInfoLabels();
        }

        public void IterateSort()
        {
            //applies the correct sort based on the button clicked

            switch (runningAlgorithm)
            {
                case SortAlgs.Bubble:
                    BubbleSort();
                    break;
                case SortAlgs.Insert:
                    InsertionSort();
                    break;
                case SortAlgs.Select:
                    SelectionSort();
                    break;
                case SortAlgs.BinaryInsert:
                    BinaryInsertSort();
                    break;
                case SortAlgs.Quick:
                    QuickSort();
                    break;
                default:
                    break;
            }
        }

        public void Draw(int iPos, int jPos)
        {
            Brush colour;

            for (int i = 0; i < array.Length; i++)
            {
                if (i == iPos)
                {
                    colour = red;
                }
                else if (i == jPos)
                {
                    colour = blue;
                }
                else
                {
                    colour = new SolidColorBrush(Color.FromArgb(255, (byte)(array[i] * 255 / this.Height), (byte)(array[i] * 255 / this.Height), (byte)(array[i] * 255 / this.Height)));
                }

                Lines[i].X1 = i * thickness + 1.5 * LeftIndent;
                Lines[i].X2 = Lines[i].X1;
                Lines[i].Y1 = this.Height - 40;
                Lines[i].Y2 = Lines[i].Y1 - array[i];
                Lines[i].Stroke = colour;
            }
        }

        public void Swap(int a, int b)
        {
            //swaps array values and lines for indexes a and b in the respected arrays

            double temp = array[a];
            array[a] = array[b];
            array[b] = temp;

            Line tempLine = Lines[a];
            Lines[a] = Lines[b];
            Lines[b] = tempLine;

            swapCount++;
        }

        #region Bubble / Insertion / Selection

        public void BubbleSort()
        {
            //only checks those up to sortI to reduce wasted checks

            if (SortJ < array.Length - (SortI + 1))
            {
                if (array[SortJ] > array[SortJ + 1])
                {
                    Swap(SortJ, SortJ + 1);
                }

                compareCount++;
                SortJ++;
            }
            else
            {
                SortJ = 0;
                SortI++;
            }

            if (SortI > array.Length - 1)
            {
                Stop();
                Draw(-1, -1);
            }
            else
            {
                Draw(SortI, SortJ);
                UpdateInfoLabels();
            }
        }

        public void InsertionSort()
        {
            if (SortJ >= 0 && array[SortJ] > array[SortJ + 1])
            {
                Swap(SortJ, SortJ + 1);
                SortJ = SortJ - 1;
            }
            else
            {
                SortI++;
                SortJ = SortI - 1;
            }

            compareCount++;
            UpdateInfoLabels();

            if (SortI > array.Length - 1)
            {
                Stop();
            }
            else
            {
                Draw(SortI, SortJ + 1);
            }
        }

        public void SelectionSort()
        {
            if (SortJ < array.Length && SortJ > minIndex)
            {
                //finds smallest instance in array

                if (array[SortJ] < array[minIndex])
                {
                    minIndex = SortJ;
                }

                compareCount++;
                SortJ++;
            }
            else if (SortI < array.Length)
            {
                Swap(SortI, minIndex);

                SortI++;
                minIndex = SortI;
                SortJ = minIndex + 1;
            }

            UpdateInfoLabels();

            if (SortI > array.Length - 1)
            {
                Stop();
                Draw(-1, -1);
            }
            else
            {
                Draw(SortI, SortJ);
            }

        }

        #endregion

        #region Binary Insertion

        public void BinaryInsertSort()
        {
            if (IndexToInsert != SortI)
            {
                if (SortJ > IndexToInsert - 1)
                {
                    Swap(SortJ+1, SortJ);
                    SortJ--;
                }
                else
                {
                    IndexToInsert = SortI;
                }
            }
            else if (++SortI < array.Length)
            {
                IndexToInsert = BinarySearchForSpace(SortI, 0, SortI);
                SortJ = SortI - 1;
            }

            /////Old Version - much faster since whole for loop completed per tick

            //IndexToInsert = BinarySearchForSpace(array, SortI, 0, SortI);

            //if (IndexToInsert != SortI)
            //{
            //    temp = array[SortI];

            //    for (int J = SortI - 1; J > IndexToInsert - 1; J--)
            //    {
            //        array[J + 1] = array[J];
            //        swapCount++;
            //    }

            //    array[IndexToInsert] = temp;
            //}

            //SortI++;

            UpdateInfoLabels();

            if (SortI > array.Length - 1)
            {
                Stop();
                Draw(-1, -1);
            }
            else
            {
                Draw(SortI, SortJ + 1);
            }
        }

        public int BinarySearchForSpace(int toInsert, int min, int max)
        {
            int mid = (int)(array.Length / 2);

            ///applies binary search to find index where element should be located
            while (min < max)
            {
                mid = (min + max) / 2;

                if (array[toInsert] >= array[mid])
                {
                    min = mid + 1;
                }
                else
                {
                    max = mid;
                }

                compareCount++;
                UpdateInfoLabels();
            }

            return min;
        }

        #endregion

        #region Quick Sort

        #region QuickSortAlgorithm

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

        #endregion

        public void QuickSort()
        {
            if (currentStart < currentEnd)
            {
                if (SortJ < currentEnd)
                {
                    if (array[SortJ] < pivotValue)
                    {
                        Swap(SortJ, pivotIndex);
                        pivotIndex++;
                    }

                    compareCount++;
                    SortJ++;
                    Draw(SortJ, pivotIndex);
                }
                else
                {
                    Swap(pivotIndex, currentEnd);

                    QuickSortsToPerform.Enqueue(new int[] { currentStart, pivotIndex - 1 });
                    QuickSortsToPerform.Enqueue(new int[] { pivotIndex + 1, currentEnd });

                    GetNextQuickSortToPerform();
                }
            }
            else if (QuickSortsToPerform.Count > 0)
            {
                GetNextQuickSortToPerform();
            }
            else
            {
                Stop();
                Draw(-1, -1);
            }

            UpdateInfoLabels();
        }

        public void GetNextQuickSortToPerform()
        {
            //retrieves the partition information so the next iteration can perform

            int[] newIndices = QuickSortsToPerform.Dequeue();
            currentStart = newIndices[0];
            currentEnd = newIndices[1];

            SortJ = currentStart;
            pivotIndex = currentStart;

            if (currentStart < currentEnd)
            {
                pivotValue = array[currentEnd];
            }
        }

        #endregion

        #region Start Sort Buttons

        public void StartSort()
        {
            Shuffle();
            ResetLabels();
            EnableSortButtons(false);
            interval = SortTimer.Interval.TotalMilliseconds;
        }

        public void EnableSortButtons(bool state)
        {
            BubbleBtn.IsEnabled = state;
            InsertBtn.IsEnabled = state;
            SelectBtn.IsEnabled = state;
            BinaryInsertBtn.IsEnabled = state;
            QuickBtn.IsEnabled = state;

            ShuffleBtn.IsEnabled = state;
            StopBtn.IsEnabled = !state;
        }

        private void BubbleBtn_Click(object sender, RoutedEventArgs e)
        {
            StartSort();

            runningAlgorithm = SortAlgs.Bubble;

            SortI = 0;
            SortJ = 0;

            SortTimer.Start();
        }

        private void InsertBtn_Click(object sender, RoutedEventArgs e)
        {
            StartSort();

            runningAlgorithm = SortAlgs.Insert;

            SortTimer.Start();
        }

        private void SelectBtn_Click(object sender, RoutedEventArgs e)
        {
            StartSort();

            runningAlgorithm = SortAlgs.Select;

            minIndex = SortI;
            SortJ = minIndex + 1;
        }

        private void BinaryInsertBtn_Click(object sender, RoutedEventArgs e)
        {
            StartSort();

            runningAlgorithm = SortAlgs.BinaryInsert;

            //if this breaks, use SortI = 0, and IndexToInsert = SortI;
            SortI = 1;
            SortJ = 0;  //SortI - 1;
            IndexToInsert = BinarySearchForSpace(SortI, 0, SortI);

            SortTimer.Start();
        }

        private void QuickBtn_Click(object sender, RoutedEventArgs e)
        {
            StartSort();

            runningAlgorithm = SortAlgs.Quick;

            currentStart = 0;
            currentEnd = array.Length - 1;
            pivotIndex = currentStart;
            pivotValue = array[currentEnd];
            SortJ = 0;

            SortTimer.Start();
        }

        #endregion

        #region Labels

        private void UpdateInfoLabels()
        {
            ComparesLbl.Content = "Comparisons: " + compareCount;
            SwapsLbl.Content = "Swaps: " + swapCount;
            TimeLbl.Content = "Time Elapsed: " + (time / 1000).ToString() + "s";
        }

        public void ResetLabels()
        {
            compareCount = 0;
            swapCount = 0;
            time = 0;

            UpdateInfoLabels();
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

            SortI = 0;
            SortJ = 0;
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

            thickness = (int)((this.Width - 2 * LeftIndent) / numOfElements);
            //thickness inverself proportional to the number of bars

            SortTimer.Interval = new TimeSpan(0, 0, 0, 0, 200);

            RandomlyAssignArrayValues();

            for (int i = 0; i < Lines.Length; i++)
            {
                Lines[i] = new Line();

                Lines[i].ToolTip = Math.Round(array[i], 4);
                Lines[i].StrokeThickness = thickness;

                myCanvas.Children.Add(Lines[i]);
            }

            Draw(-1, -1);
        }

        private void RandomlyAssignArrayValues()
        {
            //gives each instance a random double between 50 and maxHeight + 50

            for (int i = 0; i < array.Length; i++)
            {
                array[i] = rand.NextDouble() * maxHeight + 50;
            }
        }

        #endregion

        #region Shuffle

        private void ShuffleBtn_Click(object sender, RoutedEventArgs e)
        {
            Shuffle();

            ResetLabels();

            EnableSortButtons(true);
        }

        public void Shuffle()
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

    }
}
