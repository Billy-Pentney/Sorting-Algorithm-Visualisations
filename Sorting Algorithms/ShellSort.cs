namespace Sorting_Algorithms
{
    class ShellSort : Sort
    {
        private int gap;

        public ShellSort(double[] array) : base(array)
        {
            fillGapWithColour = true;
            gap = 1;
            while (gap <= max / 2)
            {
                //gap = gap * 3 + 1;
                gap = (gap + 1) * 2 - 1;
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
                // gap /= 2;
                //gapShell = (gap - 1) / 3;

                //2^k - 1, starting at 1
                gap = (gap + 1) / 2 - 1;
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
