namespace Sorting_Algorithms
{
    class CombSort : Sort
    {
        private int maxIndex;
        private int gap;
        private const double comb_K = 1.3;

        public CombSort(double[] array) : base(array)
        {
            maxIndex = array.Length;
            gap = (int)(array.Length / comb_K);
        }

        public override double[] Run()
        {
            if (SortJ + gap < maxIndex)
            {
                if (array[SortJ] > array[SortJ + gap])
                {
                    Swap(SortJ, SortJ + gap);
                    swappedThisCycle = true;
                }

                comparisonCount++;
                SortJ += gap;
            }
            else if (SortI < array.Length)
            {
                if (gap > comb_K)
                {
                    gap = (int)(gap / comb_K);
                    SortI = 0;
                }
                else if (!swappedThisCycle)
                {
                    SortI = array.Length;
                }
                else
                {
                    maxIndex = SortJ;
                    SortI++;
                }

                swappedThisCycle = false;
                SortJ = 0;
            }
            else
            {
                isFinished = true;
            }

            return array;
        }
    }
}
