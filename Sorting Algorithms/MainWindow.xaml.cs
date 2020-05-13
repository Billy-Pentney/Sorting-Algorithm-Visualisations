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
using System.Globalization;

namespace Sorting_Algorithms
{
    enum SortAlgs { Bubble, CocktailShaker, Comb, Insertion, Selection, BinaryInsertion, QuickLomuto, QuickHoare, QuickLomutoMedian, Shell};

    public partial class MainWindow : Window
    {
        SolidColorBrush red = new SolidColorBrush(Color.FromRgb(255, 100, 100));
        SolidColorBrush blue = new SolidColorBrush(Color.FromRgb(100, 100, 255));
        SolidColorBrush dblue = new SolidColorBrush(Color.FromRgb(175, 150, 255));

        bool fillGap = false;

        double[] array;                                             //actual data being sorted
        Line[] Lines;                                               //array of visual bars to represent data values

        Random rand = new Random();
        int thickness = 20;                                         //width of each bar
        DispatcherTimer SortTimer = new DispatcherTimer();          //timer to control when the sort algorithm increments and the display is updated

        int SortI = 0;                                              //main loop - usually when this reaches array.length, the sort is finished      (e.g. BUBBLE)
        int SortJ = 0;                                              //inner loop - usually when this reaches array.length, SortI++                  (e.g. BUBBLE)

        int minIndex = -1;                                          //stores the index of the smallest found element on this pass (SELECTION)

        const int minHeight = 2;                                    //minimum height of the bars
        int maxHeight;                                              //maximum height of the bars - dependent on window size

        int numOfElements = 200;                                    //number of values/lines to sort

        double LeftIndent = 100;                                    //how far from the left side of the window to the left-most bar
        double TopIndent = 100;                                     //how far from the top side of the window to the top of the highest possible bar

        int compareCount = 0;                                       //number of comparisons made between elements in the array per sort
        int swapCount = 0;                                          //number of times Swap() is called per sort

        int IndexToInsert = 0;                                      //the index to be inserted at its correct location in the array (INSERTION)

                                                                    //QUICK SORT//
        Queue<int[]> QuickSortsToPerform = new Queue<int[]>();      //stores all pairs of values for which QuickSort still needs to be performed - if empty, the data is sorted

        int currentEnd = 0;                                         //end of the current selection in QuickSort
        int currentStart = 0;                                       //start of the current selection in QuickSort
        int pivotIndex = 0;                                         //current index being compared to pivotValue in QuickSort
        double pivotValue = 0;                                      //pivot point for the current QuickSort
                                                                    // END //

        SortAlgs runningAlgorithm;                                  //Enum represents which algorithm is currently being performed

        int gap = 0;                                                //the gap between comparisons for Cocktail Shaker / Comb / Shell
        const double comb_K = 1.3;                                  //the ratio used in the Comb Sort to reduce gap
        int maxIndex;                                               //the highest value which SortJ must not exceed in Comb Sort, before the next iteration
        bool swappedThisCycle = false;                              //allows algorithm to early exit if no swaps performed on this pass (for Bubble / Cocktail / Comb / Selection) 

        double time = 0;

        public MainWindow()
        {
            InitializeComponent();

            BitmapImage iconSrc = new BitmapImage();
            iconSrc.BeginInit();
            iconSrc.UriSource = new Uri(Environment.CurrentDirectory + "/icon.png");
            iconSrc.EndInit();

            this.Icon = iconSrc;

            maxHeight = (int)(this.Height - TopIndent);
            //maximum height for the bars

            BarCount.Value = 50;
            SortTimer.Interval = new TimeSpan(0, 0, 0, 0, 15);

            Shuffle();
            SortTimer.Tick += SortTimer_Tick;
        }

        private void SortTimer_Tick(object sender, EventArgs e)
        {
            IterateSort();
            UpdateInfoLabels();
        }

        private void IterateSort()
        {
            //applies the correct sort algorithm based on the button initially clicked

            switch (runningAlgorithm)
            {
                case SortAlgs.Bubble:
                    BubbleSort();
                    break;
                case SortAlgs.CocktailShaker:
                    CocktailShakerSort();
                    break;
                case SortAlgs.Comb:
                    CombSort();
                    break;
                case SortAlgs.Insertion:
                    InsertionSort();
                    break;
                case SortAlgs.Selection:
                    SelectionSort();
                    break;
                case SortAlgs.BinaryInsertion:
                    BinaryInsertSort();
                    break;
                case SortAlgs.QuickLomuto:
                    QuickSort_Lomuto();
                    break;
                case SortAlgs.QuickLomutoMedian:
                    QuickSort_Lomuto();
                    break;
                case SortAlgs.QuickHoare:
                    QuickSort_Hoare();
                    break;
                case SortAlgs.Shell:
                    ShellSort();
                    break;
                default:
                    break;
            }
        }

        private void Draw(int iPos, int jPos)
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
                else if (fillGap && (i > iPos && i < jPos || i > jPos && i < iPos))
                {
                    colour = dblue;
                }
                else
                {
                    colour = new SolidColorBrush(Color.FromArgb(255, (byte)(array[i] * 255 / this.Height), (byte)(array[i] * 255 / this.Height), (byte)(array[i] * 255 / this.Height)));
                }

                Lines[i].X1 = i * thickness + 1.75 * LeftIndent;
                Lines[i].X2 = Lines[i].X1;
                Lines[i].Y1 = this.Height - 39;
                Lines[i].Y2 = Lines[i].Y1 - array[i];
                Lines[i].Stroke = colour;
            }
        }

        private void Swap(int a, int b)
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

        private void ReverseOrder()
        {
            //reverses the array, such that a sorted ascending array would become a sorted descending array

            int num = array.Length - 1;
            for (int i = 0; i < array.Length / 2; i++)
            {
                Swap(i, num - i);
            }
            Draw(-1, -1);
        }

        #region Bubble / Cocktail Shaker / Comb

        private void BubbleSort()
        {
            //only checks those up to sortI to reduce wasted checks

            if (SortJ < array.Length - (SortI + 1))
            {
                if (array[SortJ] > array[SortJ + 1])
                {
                    Swap(SortJ, SortJ + 1);
                    swappedThisCycle = true;
                }

                compareCount++;
                SortJ++;
            }
            else
            {
                if (!swappedThisCycle)
                {
                    SortI = array.Length;
                }

                swappedThisCycle = false;
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

        private void CocktailShakerSort()
        {
            if (SortJ + gap < array.Length - (SortI) && SortJ + gap >= SortI)
            {
                if (gap * array[SortJ] > gap * array[SortJ + gap])
                {
                    Swap(SortJ, SortJ + gap);
                    swappedThisCycle = true;
                }

                compareCount++;
                SortJ += gap;
            }
            else if (SortI < array.Length - 1)
            {
                if (!swappedThisCycle)
                {
                    SortI = array.Length;
                }

                gap += gap * -2;
                swappedThisCycle = false;

                if (gap > 0)
                {
                    SortI++;
                }
            }
            else
            {
                Stop();
                gap = -1;
                SortI = -1;
                SortJ = -1;
            }

            if (gap > 0)
            {
                Draw(array.Length - SortI - 1, SortJ);
            }
            else
            {
                Draw(SortI, SortJ);
            }

            UpdateInfoLabels();
        }

        private void CombSort()
        {
            if (SortJ + gap < maxIndex)
            {
                if (array[SortJ] > array[SortJ + gap])
                {
                    Swap(SortJ, SortJ + gap);
                    swappedThisCycle = true;
                }

                compareCount++;
                SortJ += gap;
            }
            else if (SortI < array.Length)
            {
                if (gap > comb_K)
                {
                    gap = (int)(gap / comb_K);
                    SortI = 0;
                }
                else if(!swappedThisCycle)
                {
                    SortI = array.Length;
                }
                else
                {
                    maxIndex = SortJ;
                    SortI++;
                }

                swappedThisCycle = false;
                SortJ = 0;
            }
            else
            { 
                Stop();
                maxIndex = 0;
                SortJ = -1;
            }
             
            Draw(maxIndex - 1, SortJ);      

            UpdateInfoLabels();
        }

        #endregion

        #region Insertion / Shell / Binary Insertion

        private void InsertionSort()
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
                Draw(-1,-1);
            }
            else
            {
                Draw(SortI, SortJ + 1);
            }
        }

        private void ShellSort()
        {
            if (SortJ >= 0 && array[SortJ] > array[SortJ + 1])
            {
                Swap(SortJ, SortJ + 1);
                SortJ -= 1;
            }
            else if (SortI + gap < array.Length)
            {
                SortI += gap;
                SortJ = SortI - 1;
            }
            else if (gap > 1)
            {
                // gap /= 2;
                //gapShell = (gap - 1) / 3;

                //2^k - 1, starting at 1
                gap = (gap + 1) / 2 - 1;
                SortI = 0;
                SortJ = 0;
            }
            else
            {
                Stop();
                SortI = -1;
                SortJ = -1;
            }

            Draw(SortI, SortJ);

            compareCount++;
            UpdateInfoLabels();
        }

        private void BinaryInsertSort()
        {
            if (IndexToInsert != SortI)
            {
                if (SortJ > IndexToInsert - 1)
                {
                    Swap(SortJ + 1, SortJ);
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

        private int BinarySearchForSpace(int toInsert, int min, int max)
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

        #region Selection / Quick

        private void SelectionSort()
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

        private void QuickSort_Lomuto()
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
                    Draw(SortJ, currentStart);
                }
                else
                {
                    Swap(pivotIndex, currentEnd);

                    QuickSortsToPerform.Enqueue(new int[] { currentStart, pivotIndex - 1 });
                    QuickSortsToPerform.Enqueue(new int[] { pivotIndex + 1, currentEnd });

                    DequeueQuickSort_Lomuto();
                }
            }
            else if (QuickSortsToPerform.Count > 0)
            {
                DequeueQuickSort_Lomuto();
            }
            else
            {
                Stop();
                Draw(-1, -1);
            }

            UpdateInfoLabels();
        }

        private void QuickSort_Hoare()
        {
            if (currentStart < currentEnd)
            {
                //while (array[SortI] < pivotValue)
                //{
                //    SortI++;
                //}

                //while (array[SortJ] > pivotValue)
                //{
                //    SortJ--;
                //}

                if (array[SortI] < pivotValue)
                {
                    SortI++;
                }

                if (array[SortJ] > pivotValue)
                {
                    SortJ--;
                }

                compareCount += 2;

                if (!(array[SortI] < pivotValue) && !(array[SortJ] > pivotValue))
                {
                    if (SortI >= SortJ)
                    {
                        QuickSortsToPerform.Enqueue(new int[] { currentStart, SortJ });
                        QuickSortsToPerform.Enqueue(new int[] { SortJ + 1, currentEnd });

                        DequeueQuickSort_Hoare();
                    }
                    else
                    {
                        Swap(SortI, SortJ);
                    }
                }

                Draw(SortI, SortJ);
            }
            else if (QuickSortsToPerform.Count > 0)
            {
                DequeueQuickSort_Hoare();
            }
            else
            {
                Stop();
                Draw(-1, -1);
            }

            UpdateInfoLabels();
        }

        private void DequeueQuickSort_Lomuto()
        {
            //retrieves the partition information so the next iteration can perform

            int[] newIndices;

            do
            {
                newIndices = QuickSortsToPerform.Dequeue();
                currentStart = newIndices[0];
                currentEnd = newIndices[1];
            } while (currentStart >= currentEnd && QuickSortsToPerform.Count > 0);

            SortJ = currentStart;
            pivotIndex = currentStart;

            if (currentStart < currentEnd)
            {
                if (runningAlgorithm == SortAlgs.QuickLomutoMedian)
                {
                    int mid = (currentStart + currentEnd) / 2;

                    if (array[mid] < array[currentStart])
                    {
                        Swap(currentStart, mid);
                    }
                    if (array[currentEnd] < array[currentStart])
                    {
                        Swap(currentStart, currentEnd);
                    }
                    if (array[mid] < array[currentEnd])
                    {
                        Swap(mid, currentEnd);
                    }
                }

                pivotValue = array[currentEnd];
            }
        }

        private void DequeueQuickSort_Hoare()
        {
            //retrieves the partition information so the next iteration can perform

            int[] newIndices;

            do
            {
                newIndices = QuickSortsToPerform.Dequeue();
                currentStart = newIndices[0];
                currentEnd = newIndices[1];
            } while (currentStart >= currentEnd && QuickSortsToPerform.Count > 0);

            pivotIndex = currentStart;

            if (currentStart < currentEnd)
            {
                pivotValue = array[(currentEnd + currentStart) / 2];
                SortI = currentStart;
                SortJ = currentEnd;
            }
        }

        #endregion

        #region Start Sort Buttons

        private void StartSort()
        {
            //Shuffle();
            ResetLabels();
            EnableSortButtons(false);
            time = 0;          
            fillGap = true;
        }

        private void EnableSortButtons(bool state)
        {
            BarCount.IsEnabled = state;
            BubbleBtn.IsEnabled = state;
            CocktailBtn.IsEnabled = state;
            CombBtn.IsEnabled = state;
            InsertBtn.IsEnabled = state;
            ShellBtn.IsEnabled = state;
            BinaryInsertBtn.IsEnabled = state;
            SelectBtn.IsEnabled = state;
            QuickLomutoBtn.IsEnabled = state;
            QuickLomutoMedianBtn.IsEnabled = state;
            QuickHoareBtn.IsEnabled = state;

            ShuffleBtn.IsEnabled = state;
            ReverseBtn.IsEnabled = state;
            StopBtn.IsEnabled = !state;
        }

        private void BubbleBtn_Click(object sender, RoutedEventArgs e)
        {
            StartSort();

            runningAlgorithm = SortAlgs.Bubble;

            SortI = 0;
            SortJ = 0;

            fillGap = false;
            SortTimer.Start();
        }

        private void CocktailBtn_Click(object sender, RoutedEventArgs e)
        {
            StartSort();

            runningAlgorithm = SortAlgs.CocktailShaker;

            SortI = 0;
            SortJ = 0;
            gap = 1;

            fillGap = false;
            SortTimer.Start();
        }

        private void CombBtn_Click(object sender, RoutedEventArgs e)
        {
            StartSort();

            runningAlgorithm = SortAlgs.Comb;

            SortI = 0;
            SortJ = 0;
            maxIndex = array.Length;

            fillGap = false;
            gap = (int)(array.Length / comb_K);
            SortTimer.Start();
        }

        private void InsertBtn_Click(object sender, RoutedEventArgs e)
        {
            StartSort();

            runningAlgorithm = SortAlgs.Insertion;

            SortTimer.Start();
        }

        private void ShellBtn_Click(object sender, RoutedEventArgs e)
        {
            StartSort();

            runningAlgorithm = SortAlgs.Shell;

            gap = 1;
            while (gap < array.Length / 2)
            {
                //gapShell = gapShell * 3 + 1;
                gap = (gap + 1) * 2 - 1;
            }

            // gapShell = (int)(array.Length / 2);
            SortTimer.Start();
        }

        private void BinaryInsertBtn_Click(object sender, RoutedEventArgs e)
        {
            StartSort();

            runningAlgorithm = SortAlgs.BinaryInsertion;

            //if this breaks, use SortI = 0, and IndexToInsert = SortI;
            SortI = 1;
            SortJ = 0;  //SortI - 1;
            IndexToInsert = BinarySearchForSpace(SortI, 0, SortI);

            SortTimer.Start();
        }

        private void SelectBtn_Click(object sender, RoutedEventArgs e)
        {
            StartSort();

            runningAlgorithm = SortAlgs.Selection;

            SortI = 0;
            minIndex = SortI;
            SortJ = minIndex + 1;

            fillGap = false;
            SortTimer.Start();
        }

        private void QuickLomutoBtn_Click(object sender, RoutedEventArgs e)
        {
            StartSort();

            runningAlgorithm = SortAlgs.QuickLomuto;

            QuickSortsToPerform.Enqueue(new int[] { 0, array.Length - 1 });
            DequeueQuickSort_Lomuto();

            //bool sorted = true;

            //for (int i = 1; i < array.Length; i++)
            //{
            //    if (array[i] < array[i - 1])
            //    {
            //        sorted = false;
            //        break;
            //    }
            //    Draw(-1, i);
            //}

            //if (sorted)
            //{
            //    Stop();
            //    Draw(-1, -1);
            //}
            //else
            //{
            SortTimer.Start();
            //}

        }

        private void QuickLomutoMedianBtn_Click(object sender, RoutedEventArgs e)
        {
            StartSort();

            runningAlgorithm = SortAlgs.QuickLomutoMedian;

            QuickSortsToPerform.Enqueue(new int[] { 0, array.Length - 1});
            DequeueQuickSort_Lomuto();

            SortTimer.Start();
        }

        private void QuickHoareBtn_Click(object sender, RoutedEventArgs e)
        {
            StartSort();

            runningAlgorithm = SortAlgs.QuickHoare;

            QuickSortsToPerform.Enqueue(new int[] { 0, array.Length - 1 });
            DequeueQuickSort_Hoare();

            SortTimer.Start();
        }

        #endregion

        #region Labels

        private void UpdateInfoLabels()
        {
            ComparesLbl.Content = "Comparisons: " + compareCount;
            SwapsLbl.Content = "Swaps: " + swapCount;

            TimeLbl.Content = "Time Elapsed: " + time.ToString() + "s";
        }

        private void ResetLabels()
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

            AssignArrayValues();  

            for (int i = 0; i < Lines.Length; i++)
            {
                Lines[i] = new Line();

                Lines[i].ToolTip = Math.Round(array[i], 4);
                Lines[i].StrokeThickness = thickness;

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

        private void AssignArrayValues()
        {
            if ((bool)RandomValueCheck.IsChecked)
            {
                AssignRandomArrayValues();
            }
            else
            {
                AssignLinearArrayValues();
            }
        }

        private void RandomValueCheck_Checked(object sender, RoutedEventArgs e)
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
    }
}
