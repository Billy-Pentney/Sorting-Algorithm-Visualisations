using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

namespace Sorting_Algorithms
{

    public partial class MainWindow : Window
    {
        double[] array;     //actual data being sorted
        Line[] Lines;   //array of visual lines to represent data values
        Random rand = new Random();
        int thickness = 20;
        DispatcherTimer timer = new DispatcherTimer();

        int SortI = 0;
        int SortJ = 0;
        int minIndex = -1;

        const int minHeight = 50;
        int maxHeight;

        double LeftIndent = 100;
        double TopIndent = 150;

        int compareCount = 0;
        int swapCount = 0;

        int SortType = 0;

        int IndexToInsert = 0;

        public MainWindow()
        {
            InitializeComponent();

            maxHeight = (int)(this.Height - TopIndent);

            timer.Interval = new TimeSpan(0, 0, 0, 0, thickness);
            array = new double[(int)(this.Width - 2 * LeftIndent) / thickness];
            Lines = new Line[array.Length];

            for (int i = 0; i < array.Length; i++)
            {
                array[i] = rand.NextDouble() * maxHeight + 50;

                Lines[i] = new Line();

                myCanvas.Children.Add(Lines[i]);
            }

            Shuffle();
            timer.Tick += Timer_Tick;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (SortI < array.Length)
            {
                IterateSort();
            }
            else
            {
                Stop();
            }
        }

        public void IterateSort()
        {
            //applies the correct sort based on the button clicked

            switch (SortType)
            {
                case 0:
                    BubbleSort();
                    break;
                case 1:
                    InsertionSort();
                    break;
                case 2:
                    SelectionSort();
                    break;
                case 3:
                    BinaryInsertSort();
                    break;
                default:
                    break;
            }
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

            Draw(-1,-1);
            SortI = 0;
        }

        public void Draw(int iPos, int jPos)
        {
            Brush colour;

            for (int i = 0; i < array.Length; i++)
            {
                if (i == iPos)
                {
                    colour = new SolidColorBrush(Color.FromRgb((byte)255, (byte)100, (byte)100));
                }
                else if (i == jPos)
                {
                    colour = new SolidColorBrush(Color.FromRgb((byte)100, (byte)100, (byte)255));
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
                Lines[i].StrokeThickness = thickness;
            }
        }

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

            Draw(SortI, SortJ);

            UpdateInfoLabels();
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
            Draw(SortI, SortJ + 1);
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

            Draw(SortI, SortJ);
            UpdateInfoLabels();

        }

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

            Draw(SortI, SortJ + 1);
            UpdateInfoLabels();

        }

        public int BinarySearchForSpace(int toInsert, int min, int max)
        {
            int mid = (int)(array.Length / 2);

            while (min < max)
            {
                mid = (min + max) / 2;

                //if (array[toInsert] == array[mid])
                //{
                //    return mid;
                //}
                //else 
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

            //Lines[FirstIndex].X1 = FirstIndex * thk;
            //Lines[FirstIndex].X2 = FirstIndex * thk;
            //Lines[SecondIndex].X1 = SecondIndex * thk;
            //Lines[SecondIndex].X2 = SecondIndex * thk;

            //Lines[FirstIndex].Y1 = this.Height;
            //Lines[FirstIndex].Y2 = this.Height - array[FirstIndex];
            //Lines[SecondIndex].Y1 = this.Height;
            //Lines[SecondIndex].Y2 = this.Height - array[SecondIndex];
        }

        private void BubbleBtn_Click(object sender, RoutedEventArgs e)
        {
            Shuffle();

            ResetLabels();

            EnableSortButtons(false);

            SortType = 0;
            timer.Start();    
        }

        private void InsertBtn_Click(object sender, RoutedEventArgs e)
        {
            Shuffle();

            ResetLabels();

            EnableSortButtons(false);

            SortType = 1;
            timer.Start();
        }

        private void SelectBtn_Click(object sender, RoutedEventArgs e)
        {
            Shuffle();

            ResetLabels();
            EnableSortButtons(false);

            SortType = 2;

            minIndex = SortI;
            SortJ = minIndex + 1;

            timer.Start();
        }

        private void BinaryInsertBtn_Click(object sender, RoutedEventArgs e)
        {
            Shuffle();

            ResetLabels();
            EnableSortButtons(false);

            SortType = 3;

            SortI = 1;
            SortJ = SortI - 1;

            IndexToInsert = SortI;

            timer.Start();
        }

        public void EnableSortButtons(bool state)
        {
            BubbleBtn.IsEnabled = state;
            InsertBtn.IsEnabled = state;
            SelectBtn.IsEnabled = state;
            BinaryInsertBtn.IsEnabled = state;

            ShuffleBtn.IsEnabled = state;
            StopBtn.IsEnabled = !state;
        }

        private void ShuffleBtn_Click(object sender, RoutedEventArgs e)
        {
            Shuffle();

            ResetLabels();

            EnableSortButtons(true);
        }

        private void UpdateInfoLabels()
        {
            ComparesLbl.Content = "Comparisons: " + compareCount;
            SwapsLbl.Content = "Swaps: " + swapCount;
        }

        public void ResetLabels()
        {
            compareCount = 0;
            swapCount = 0;

            UpdateInfoLabels();
        }

        private void StopBtn_Click(object sender, RoutedEventArgs e)
        {
            Stop();
        }

        private void Stop()
        {
            timer.Stop();
            EnableSortButtons(true);

            SortI = 0;
            SortJ = 0;
        }
    }
}
