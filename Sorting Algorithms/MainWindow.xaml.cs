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

        double minBarVal = 3;
        double maxBarVal = 100;

        int numOfElements = 10;                                     //number of values/lines to sort
        const int minElements = 10;
        const int maxElements = 200;

        #endregion

        Sort currentSort;
        bool fastRun = false;

        public MainWindow()
        {
            InitializeComponent();
            InitialiseArray();

            SetWindowIcon();

            SortTimer.Interval = new TimeSpan(0, 0, 0, 0, 30);
            SortTimer.Tick += SortTimer_Tick;
        }

        private void SetWindowIcon()
        {
            BitmapImage iconSrc = new BitmapImage();
            iconSrc.BeginInit();
            iconSrc.UriSource = new Uri(Environment.CurrentDirectory + "/icon.png");
            iconSrc.EndInit();

            Icon = iconSrc;
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

            if (myCanvas == null)
                return;

            double maxBarH = myCanvas.ActualHeight;

            for (int i = 0; i < array.Length; i++)
            {
                double h_proportion = array[i] / maxBarVal;
                // 0 < array[i] < 100
                // we have to convert that to the height of the canvas

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
                    colour = new SolidColorBrush(Color.FromArgb(255, (byte)(200 * h_proportion), (byte)(180 * h_proportion), (byte)(250 * h_proportion)));
                }

                Lines[i].Stroke = colour;
                Lines[i].ToolTip = Math.Round(array[i], 2);
                Lines[i].Y1 = maxBarH;
                Lines[i].Y2 = (1 - h_proportion) * maxBarH;
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
            HeapSortsGroup.IsEnabled = state;
            OtherSortsGroup.IsEnabled = state;

            FastChkBox.IsEnabled = state;

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
            UpdateInfoLabels(0, 0);
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
            DrawWithoutIndices();
        }

        #endregion

        #region Bars/Lines Size and Shape

        private void BarCount_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            numOfElements = (int)BarCount.Value;
            CountLbl.Content = numOfElements.ToString();
            InitialiseArray();
            UpdateBars();
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
            if (myCanvas == null || double.IsNaN(myCanvas.ActualHeight))
                return;

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

            double usableWidth = myCanvas.ActualWidth;

            thickness = (int)(usableWidth / numOfElements);
            //thickness inversely proportional to the number of bars

            barGap = (usableWidth - thickness * array.Length) / (array.Length + 1);

            for (int i = 0; i < Lines.Length; i++)
            {
                Lines[i] = new Line();
                Lines[i].StrokeThickness = thickness;

                Lines[i].X1 = i * (thickness + barGap) + thickness / 2;
                Lines[i].X2 = Lines[i].X1;

                myCanvas.Children.Add(Lines[i]);
            }
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

        private void DataStyleChanged(object sender, RoutedEventArgs e)
        {
            if (array == null)
                return;

            switch (((RadioButton)sender).Content)
            {
                case "Random":
                    AssignRandomArrayValues();
                    break;
                case "Logarithm":
                    AssignLogArrayValues();
                    break;
                case "Exponential":
                    AssignExpArrayValues();
                    break;
                default:
                    AssignLinearArrayValues();
                    break;
            }

            DrawWithoutIndices();
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
            DrawWithoutIndices();
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

            DrawWithoutIndices();
        }

        private void DrawWithoutIndices()
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

        private void FastChkBox_Checked(object sender, RoutedEventArgs e)
        {
            fastRun = (bool)FastChkBox.IsChecked;
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateBars();
        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            UpdateBars();
        }

        public void UpdateBars()
        {
            InitialiseLines();
            if (currentSort == null && SortTimer.IsEnabled)
                Draw(currentSort.getI(), currentSort.getJ(), currentSort.getK());
            else
                DrawWithoutIndices();
        }

        private void myCanvas_Initialized(object sender, EventArgs e)
        {
            //Shuffle();
        }
    }
}
