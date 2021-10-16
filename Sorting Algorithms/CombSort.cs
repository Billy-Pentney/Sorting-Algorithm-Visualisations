namespace Sorting_Algorithms
{
    class CombSort : Sort
    {
        private int gap;    // gap between the elements being compared
        private const double comb_K = 1.3;      // constant factor used to adjust gap

        public CombSort(double[] array) : base(array)
        {
            gap = (int)(max + 1 / comb_K);
        }

        public override double[] Run()
        {
            if (SortJ + gap <= max)
            {
                if (Compare(SortJ, SortJ+gap) > 0)
                {
                    Swap(SortJ, SortJ + gap);
                    swappedThisCycle = true;
                }

                SortJ += gap;
            }
            else if (SortI <= max)
            {
                if (gap > comb_K)
                {
                    gap = (int)(gap / comb_K);
                    SortI = min;
                }
                else if (!swappedThisCycle)
                    SortI = max + 1;
                else
                {
                    max = SortJ - 1;
                    SortI++;
                }

                swappedThisCycle = false;
                SortJ = min;
            }
            else
                isFinished = true;

            return array;
        }

        public override double[] QuickRun()
        {
            swappedThisCycle = false;

            for (SortJ = 0; SortJ + gap <= max; SortJ++)
            {
                if (Compare(SortJ, SortJ+gap) > 0)
                {
                    Swap(SortJ, SortJ+gap);
                    swappedThisCycle = true;
                }
            }

            if (gap > comb_K)
            {
                gap = (int)(gap / comb_K);
                SortI = min;
            }
            else if (!swappedThisCycle)
                SortI = max + 1;
            else
            {
                max = SortJ - 1;
                SortI++;
            }

            isFinished = !swappedThisCycle || SortI > max;

            return array;
        }
    }
}
