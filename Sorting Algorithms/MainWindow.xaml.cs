using System;
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
        SolidColorBrush yellow = new SolidColorBrush(Color.FromRgb(255, 255, 175));
        SolidColorBrush green = new SolidColorBrush(Color.FromRgb(100, 255, 100));
        SolidColorBrush blue = new SolidColorBrush(Color.FromRgb(50, 175, 255));
        SolidColorBrush light_blue = new SolidColorBrush(Color.FromRgb(175, 220, 255));
        SolidColorBrush orange = new SolidColorBrush(Color.FromRgb(255, 210, 160));
        SolidColorBrush pink = new SolidColorBrush(Color.FromRgb(255, 175, 220));
        SolidColorBrush lilac = new SolidColorBrush(Color.FromRgb(220, 175, 255));

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
        const int maxElements = 250;

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

            BarCountSlider.Maximum = maxElements;
            BarCountSlider.Minimum = minElements;
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

                // primary pointer
                if (i == iPos)
                    colour = red;
                // secondary pointer
                else if (i == jPos)
                    colour = blue;
                // tertiary pointer
                else if (i == kPos)
                    colour = green;
                // interval between pointers
                else if (fillGap && (i > iPos && i < jPos || i > jPos && i < iPos))
                    colour = yellow;
                else
                    colour = new SolidColorBrush(Color.FromArgb(255, (byte)(100 + 150 * h_proportion), (byte)(75 + 175 * h_proportion), (byte)(150 + 100 * h_proportion)));

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
            BarCountSlider.IsEnabled = state;

            BubbleGroup.IsEnabled = state;
            InsertionGroup.IsEnabled = state;
            SelectionGroup.IsEnabled = state;
            QuickSortGroup.IsEnabled = state;
            HeapSortsGroup.IsEnabled = state;
            HybridSortsGroup.IsEnabled = state;
            CountingSortsGroup.IsEnabled = state;
            OtherSortsGroup.IsEnabled = state;

            FastRunChkBox.IsEnabled = state;

            ParametersStackPanel.IsEnabled = state;

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
                case "Intro Sort":
                    currentSort = new IntroSort(array);
                    break;
                case "Binary Intro Sort":
                    currentSort = new BinaryIntroSort(array);
                    break;
                case "Bucket Sort":
                    currentSort = new BucketSort(array, minBarVal, maxBarVal);
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
            numOfElements = (int)BarCountSlider.Value;
            ElementCountLbl.Content = numOfElements.ToString();
            InitialiseArray();
            UpdateBars();
        }

        private void InitialiseArray()
        {
            numOfElements = Math.Max(numOfElements, minElements);
            numOfElements = Math.Min(numOfElements, maxElements);

            array = new double[numOfElements];

            if (DataFunctionComboBox != null) {
                ComboBoxItem chosenComboBoxItem = (ComboBoxItem)DataFunctionComboBox.SelectedItem;

                if (chosenComboBoxItem == null)
                    AssignLinearArrayValues();
                else
                    GenerateArrayValuesOfType(chosenComboBoxItem.Content.ToString());
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

        private void AssignSinArrayValues()
        {
            //assigns height based on sin function

            double a = 2 * Math.PI;
            double b = Math.PI / 2;

            for (int i = 0; i < array.Length; i++)
            {
                double val = 1 + Math.Sin(a * i / array.Length + b);

                array[i] = minBarVal + val * (maxBarVal - minBarVal) / 2;
            }
        }

        private void AssignLogisticArrayValues()
        {
            //assigns height based on cos function

            double a = Math.PI;           // multiplier
            double b = -Math.PI;          // phase shift 

            for (int i = 0; i < array.Length; i++)
            {
                // -1 <= cos(x) <= 1, so add 1 to result to yield 0 <= val <= 2
                double val = 1 + Math.Cos(a *  i / array.Length + b);
                // then 0 <= val/2 <= 1, so we can find the proportion of the bar value in the interval
                array[i] = minBarVal + val * (maxBarVal - minBarVal) / 2;
            }
        }

        private void ArrayGeneratingFunction_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (array == null)
                return;

            ComboBox comboBox = (ComboBox)sender;
            ComboBoxItem selectedItem = (ComboBoxItem)comboBox.SelectedItem;

            if (selectedItem != null)
                GenerateArrayValuesOfType(selectedItem.Content.ToString());

            DrawWithoutIndices();
        }

        public void GenerateArrayValuesOfType(string selectedType)
        {
            switch (selectedType)
            {
                case "Logarithm":
                    AssignLogArrayValues();
                    break;
                case "Random":
                    AssignRandomArrayValues();
                    break;
                case "Exponential":
                    AssignExpArrayValues();
                    break;
                case "Logistic":
                    AssignLogisticArrayValues();
                    break;
                case "Sinusoidal":
                    AssignSinArrayValues();
                    break;
                default:
                    AssignLinearArrayValues();
                    break;
            }
        }

        #endregion

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

        private void FastChkBox_Checked(object sender, RoutedEventArgs e)
        {
            fastRun = (bool)FastRunChkBox.IsChecked;
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
