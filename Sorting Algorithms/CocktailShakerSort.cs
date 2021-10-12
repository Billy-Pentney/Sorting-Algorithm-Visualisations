namespace Sorting_Algorithms
{
    class CocktailShakerSort : Sort
    {
        protected int gap;

        public CocktailShakerSort(double[] array) : base(array)
        {
            this.gap = 1;
        }

        public override double[] Run()
        {
            if (SortJ + gap < array.Length - (SortI) && SortJ + gap >= SortI)
            {
                if (gap * array[SortJ] > gap * array[SortJ + gap])
                {
                    Swap(SortJ, SortJ + gap);
                    swappedThisCycle = true;
                }

                comparisonCount++;
                SortJ += gap;
            }
            else if (SortI < array.Length - 1)
            {
                if (!swappedThisCycle)
                    SortI = array.Length;

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

            for (; SortJ + gap < array.Length - SortI && SortJ + gap >= SortI; SortJ += gap)
            {
                if (gap * array[SortJ] > gap * array[SortJ + gap])
                {
                    Swap(SortJ, SortJ + gap);
                    swappedThisCycle = true;
                }

                comparisonCount++;
            }

            gap = -gap;

            if (gap > 0)
                SortI++;

            isFinished = !swappedThisCycle || SortI >= array.Length - 1;

            return array;
        }
    }
}
