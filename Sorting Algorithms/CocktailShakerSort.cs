namespace Sorting_Algorithms
{
    class CocktailShakerSort : Sort
    {
        protected int gap;      // the gap between the elements currently being compared

        public CocktailShakerSort(double[] array) : base(array)
        {
            this.gap = 1;
        }

        public override double[] Run()
        {
            if (SortJ+gap <= max-SortI && SortJ+gap >= SortI)
            {
                if (Compare(SortJ, SortJ+gap) > 0)
                {
                    Swap(SortJ, SortJ + gap);
                    swappedThisCycle = true;
                }

                SortJ += gap;
            }
            else if (SortI < max)
            {
                if (!swappedThisCycle)
                    SortI = max + 1;

                gap = -gap;
                swappedThisCycle = false;

                if (gap > 0)
                    SortI++;
            }
            else
            {
                isFinished = true;
            }

            return array;
        }

        public override double[] QuickRun()
        {
            swappedThisCycle = false;

            for (; SortJ + gap <= max - SortI && SortJ + gap >= SortI; SortJ += gap)
            {
                if (Compare(SortJ, SortJ+gap) > 0)
                {
                    Swap(SortJ, SortJ + gap);
                    swappedThisCycle = true;
                }

                comparisonCount++;
            }

            gap = -gap;

            if (gap > min)
                SortI++;

            isFinished = !swappedThisCycle || SortI >= max;

            return array;
        }

        protected override int Compare(int a, int b)
        {
            // when moving up the list, gap > 0
            // when moving down the list, gap < 0
            // multiplying by gap, ensures the elements are compared correctly
            return base.Compare(a,b) * gap;
        }
    }
}
