namespace Sorting_Algorithms
{
    class ShellSort : Sort
    {
        private int gap;

        public ShellSort(double[] array) : base(array)
        {
            sortName = "Shell Sort";
            sortDescription = "Shell Sort performs insertion sorts with a decreasing gap between compared elements.";
            worstCase = "O(n^1.5)";
            bestCase = O_of_N_log_N;
            avgCase = O_of_N_log_N;

            fillGapWithColour = true;
            gap = 1;
            while (gap <= max / 2)
            {
                gap = CalculateGap();
            }
        }

        public override double[] Run()
        {
            if (SortJ - gap >= min && SortJ <= max)
            {
                if (Compare(SortJ, SortJ-gap) < 0)
                    Swap(SortJ, SortJ - gap);
                else
                {
                    SortI += 1;
                    SortJ = SortI + gap;
                }
                SortJ -= gap;
            }
            else if (SortI <= max)
            {
                SortI += 1;
                SortJ = SortI;
            }
            else if (gap > 1)
            {

                gap = CalculateGap();
                SortI = min + gap;
                SortJ = SortI;
            }
            else
            {
                isFinished = true;
            }

            comparisonCount++;
            return array;
        }

        private int CalculateGap()
        {
            // Hibbard intervals
            return (gap + 1) * 2 + 1;
        }

        public override double[] QuickRun()
        {
            for (SortJ = SortI; SortJ - gap >= min && SortJ <= max; SortJ -= gap)
            {
                if (Compare(SortJ, SortJ-gap) < 0)
                    Swap(SortJ, SortJ - gap);
                else
                {
                    SortI += 1;
                    SortJ = SortI + gap;
                }
            }

            if (SortI <= max)
            {
                SortI += 1;
                SortJ = SortI;
            }
            else if (gap > 1)
            {
                gap = (gap + 1) / 2 - 1;
                SortI = min + gap;
            }
            else
                isFinished = true;

            return array;
        }
    }
}
