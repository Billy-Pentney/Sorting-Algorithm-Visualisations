namespace Sorting_Algorithms
{
    class BubbleSort : Sort
    {
        public BubbleSort(double[] array) : base(array) { }

        public override double[] Run()
        {
            if (SortJ + 1 < array.Length - SortI)
            {
                if (array[SortJ] > array[SortJ + 1])
                {
                    Swap(SortJ, SortJ + 1);
                    swappedThisCycle = true;
                }

                comparisonCount++;
                SortJ++;
            }
            else
            {
                // early exit if no swaps performed on a pass
                if (!swappedThisCycle)
                {
                    SortI = array.Length;
                }

                swappedThisCycle = false;
                SortJ = 0;
                SortI++;
            }

            isFinished = (SortI >= array.Length);

            return array;
        }
    }
}
