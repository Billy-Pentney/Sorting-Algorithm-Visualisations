namespace Sorting_Algorithms
{
    class CocktailShakerSort : Sort
    {
        protected int gap;

        public CocktailShakerSort(double[] array) : base(array)
        {
            this.gap = 1;
        }

/*        public override int getIPointer()
        {
            if (gap > 0)
            {
                return SortI;
            }
            return SortJ;
        }

        public override int getJPointer()
        {
            if (gap > 0)
            {
                return SortJ;
            }
            return SortI;
        }*/

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

                gap += gap * -2;
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
    }
}
