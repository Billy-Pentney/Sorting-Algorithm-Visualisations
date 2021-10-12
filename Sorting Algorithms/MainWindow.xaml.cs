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
        SolidColorBrush yellow = new SolidColorBrush(Color.FromRgb(250, 250, 80));
        SolidColorBrush green = new SolidColorBrush(Color.FromRgb(100, 250, 100));
        SolidColorBrush blue = new SolidColorBrush(Color.FromRgb(100, 100, 255));
        SolidColorBrush d_blue = new SolidColorBrush(Color.FromRgb(150, 175, 230));

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

        double maxBarH;                                              //maximum height of the bars - dependent on window size

        double minBarVal = 3;
        double maxBarVal = 100;

        int numOfElements = 10;                                     //number of values/lines to sort
        const int minElements = 10;
        const int maxElements = 200;

        double LeftIndent = 200;                                    //how far from the left side of the window to the left-most bar
        double RightIndent = 30;                                    //how far from the right side of the window to the right-most bar
        double TopIndent = 160;                                     //how far from the top side of the window to the top of the highest possible bar

        double currHeight = 600;
        double currWidth = 600;
        double normalHeight;
        double normalWidth;
        double maximisedHeight;
        double maximisedWidth;

        #endregion

        Sort currentSort;
        bool fastRun = false;

        public MainWindow()
        {
            maximisedHeight = SystemParameters.WorkArea.Height;
            maximisedWidth = SystemParameters.WorkArea.Width;

            InitializeComponent();
            LeftIndent = SortButtonsScroller.Width;

            normalHeight = this.Height;
            normalWidth = this.Width;
            currHeight = normalHeight;
            currWidth = normalWidth;

            InitialiseArray();
            InitialiseLines();
            SortButtonsScroller.Height = myCanvas.Height;

            BitmapImage iconSrc = new BitmapImage();
            iconSrc.BeginInit();
            iconSrc.UriSource = new Uri(Environment.CurrentDirectory + "/icon.png");
            iconSrc.EndInit();

            this.Icon = iconSrc;

            BarCount.Value = 50;
            SortTimer.Interval = new TimeSpan(0, 0, 0, 0, 30);

            Shuffle();
            SortTimer.Tick += SortTimer_Tick;
        }

        private void SortTimer_Tick(object sender, EventArgs e)
        {
            IterateSort();
            UpdateInfoLabels(currentSort.getComparisons(), currentSort.getSwaps());
        }

        private void IterateSort()
        {
            // performs next iteration of the sort algorithm and displays the array afterwards
            if (fastRun)
                array = currentSort.QuickRun();
            else
                array = currentSort.Run();

            if (currentSort.hasFinished())
                Stop();
            else
                Draw(currentSort.getI(), currentSort.getJ(), currentSort.getK());
        }

        private void Draw(int iPos, int jPos, int kPos)
        {
            Brush colour;

            for (int i = 0; i < array.Length; i++)
            {
                double position = array[i] / maxBarVal;
                // 0 <= position <= 1
                // so it indicates what proportion of the window height the value is

                if (i == iPos)
                {
                    // primary pointer
                    colour = red;
                }
                else if (i == jPos)
                {
                    // secondary pointer
                    colour = blue;
                }
                else if (i == kPos)
                {
                    // tertiary pointer
                    colour = green;
                }
                else if (fillGap && (i > iPos && i < jPos || i > jPos && i < iPos))
                {
                    // interval between pointers
                    colour = d_blue;
                }
                else
                {
                    colour = new SolidColorBrush(Color.FromArgb(255, (byte)(200 * position), (byte)(180 * position), (byte)(250 * position)));
                }

                Lines[i].Stroke = colour;
                Lines[i].ToolTip = Math.Round(array[i], 2);
                Lines[i].Y1 = currHeight;

                if (!isWindowMaximised())
                {
                    Lines[i].Y1 -= 39;
                }
                else
                {
                    Lines[i].Y1 -= 20;
                }

                Lines[i].Y2 = Lines[i].Y1 - position * maxBarH;
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

            DrawBlank();
        }

        #region Start Sort Buttons

        private void StartSort()
        {
            //Shuffle();
            ResetLabels();
            EnableSortButtons(false);
            fillGap = currentSort.getFillGap();
            SortTimer.Start();
        }

        private void EnableSortButtons(bool state)
        {
            BarCount.IsEnabled = state;

            BubbleGroup.IsEnabled = state;
            InsertionGroup.IsEnabled = state;
            SelectionGroup.IsEnabled = state;
            QuickSortGroup.IsEnabled = state;
            OtherSortsGroup.IsEnabled = state;

            DataFunctionsCheckBoxes.IsEnabled = state;

            ShuffleBtn.IsEnabled = state;
            ReverseBtn.IsEnabled = state;
            StopBtn.IsEnabled = !state;
        }

        private void SortButton_Click(object Sender, RoutedEventArgs e)
        {
            Button btnSender = (Button)Sender;
            switch (btnSender.Content)
            {
                case "Bubble Sort":
                    currentSort = new BubbleSort(array);
                    break;
                case "Cocktail Shaker":
                    currentSort = new CocktailShakerSort(array);
                    break;
                case "Comb Sort":
                    currentSort = new CombSort(array);
                    break;
                case "Insertion Sort":
                    currentSort = new InsertionSort(array);
                    break;
                case "Binary Insertion":
                    currentSort = new BinaryInsertion(array);
                    break;
                case "Shell Sort":
                    currentSort = new ShellSort(array);
                    break;
                case "Selection Sort":
                    currentSort = new SelectionSort(array);
                    break;
                case "Double Selection":
                    currentSort = new DoubleSelectionSort(array);
                    break;
                case "Quick (Lomuto)":
                    currentSort = new LomutoQuickSort(array);
                    break;
                case "Quick (Hoare)":
                    currentSort = new HoareQuickSort(array);
                    break;
                case "Quick (Median)":
                    currentSort = new LomutoMedianQuickSort(array);
                    break;
                case "Merge Sort":
                    currentSort = new MergeSort(array);
                    break;
                case "Cycle Sort":
                    currentSort = new CycleSort(array);
                    break;
                case "Patience Sort":
                    currentSort = new PatienceSort(array);
                    break;
                case "Heap Sort":
                    currentSort = new HeapSort(array);
                    break;
                default:
                    return;
            }

            StartSort();
        }

        #endregion

        #region Labels

        private void UpdateInfoLabels(int comps, int swaps)
        {
            ComparesLbl.Content = comps.ToString();
            SwapsLbl.Content = swaps.ToString();
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
            DrawBlank();
        }

        #endregion

        #region Bars/Lines Size and Shape

        private void BarCount_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            numOfElements = (int)BarCount.Value;
            CountLbl.Content = numOfElements.ToString();
            InitialiseArray();
            InitialiseLines();
        }

        private void InitialiseArray()
        {
            numOfElements = Math.Max(numOfElements, minElements);
            numOfElements = Math.Min(numOfElements, maxElements);

            array = new double[numOfElements];

            if (RandomValueCheck == null)
            {
                AssignLinearArrayValues();
            }
            else
            {
                if ((bool)RandomValueCheck.IsChecked)
                    AssignRandomArrayValues();
                else if ((bool)ExpValueCheck.IsChecked)
                    AssignExpArrayValues();
                else if ((bool)LogValueCheck.IsChecked)
                    AssignLogArrayValues();
                else
                    AssignLinearArrayValues();
            }
        }

        private void InitialiseLines()
        {
            SortButtonsScroller.Height = currHeight;

            if (!isWindowMaximised())
            {
                SortButtonsScroller.Height -= 39;
            }

            maxBarH = currHeight - TopIndent;

            if (Lines != null)
            {
                foreach (Line line in Lines)
                {
                    myCanvas.Children.Remove(line);
                }
            }

            //creates array to store values to be sorted
            Lines = new Line[array.Length];
            //creates array of lines to represent the array values

            double useableWidth = currWidth - LeftIndent - 2 * RightIndent;

            thickness = (int)(useableWidth / numOfElements);
            //thickness inversely proportional to the number of bars

            barGap = (useableWidth - thickness * array.Length) / (array.Length + 1);

            for (int i = 0; i < Lines.Length; i++)
            {
                Lines[i] = new Line();
                Lines[i].StrokeThickness = thickness;

                Lines[i].X1 = LeftIndent + 0.9 * RightIndent + i * (thickness + barGap) + thickness / 2;
                Lines[i].X2 = Lines[i].X1;

                myCanvas.Children.Add(Lines[i]);
            }

            DrawBlank();
        }

        private void AssignRandomArrayValues()
        {
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = minBarVal + rand.NextDouble() * (maxBarVal - minBarVal);
            }
        }

        private void AssignLinearArrayValues()
        {
            array[0] = minBarVal;
            double delta = (maxBarVal - minBarVal) / (array.Length + 1);

            for (int i = 1; i < array.Length; i++)
            {
                array[i] = array[i - 1] + delta;
            }
        }

        private void AssignExpArrayValues()
        {
            //assigns height based on exponential function

            double a = 1.8;         //increase to make exponential increase "tighter" to right end
            double b = -0.1;        //decrease magnitude to reduce change as array length increases

            double baseExp = a * Math.Pow(array.Length, b);
            //double baseExp = 1.5277 * Math.Exp(-0.0022 * array.Length);
            //double baseExp = 1.5 + -0.0024 * array.Length;            //only works at extremes

            for (int i = 0; i < array.Length; i++)
            {
                double pow = i - array.Length;
                double val = Math.Pow(baseExp, pow);

                array[i] = minBarVal + val * (maxBarVal - minBarVal);
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

                array[i] = minBarVal + val * (maxBarVal - minBarVal);
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

                array[i] = minBarVal + val * (maxBarVal - minBarVal) / 2;
            }
        }

        private void LinearValueCheck_Checked(object sender, RoutedEventArgs e)
        {
            InitialiseArray();
            InitialiseLines();
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

            DrawBlank();
        }

        private void DrawBlank()
        {
            // draws without coloured bars
            Draw(-1, -1, -1);
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
            if (!isWindowMaximised())
            {
                currHeight = this.Height;
                currWidth = this.Width;
            }

            InitialiseLines();
        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            switch (this.WindowState)
            {
                case WindowState.Maximized:
                    normalHeight = currHeight;
                    normalWidth = currWidth;
                    currHeight = maximisedHeight;
                    currWidth = maximisedWidth;
                    InitialiseLines();
                    break;
                case WindowState.Normal:
                    currHeight = normalHeight;
                    currWidth = normalWidth;
                    break;
            }
        }

        private bool isWindowMaximised()
        {
            return this.WindowState == WindowState.Maximized;
        }

        private void FastChkBox_Checked(object sender, RoutedEventArgs e)
        {
            fastRun = (bool)FastChkBox.IsChecked;
        }
    }
}
