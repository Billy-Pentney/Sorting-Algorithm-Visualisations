namespace Sorting_Algorithms
{
    class BubbleSort : Sort
    {
        public BubbleSort(double[] array) : base(array) {
            sortName = "Bubble Sort";
            sortDescription = "Bubble sort repeatedly steps through the list, comparing adjacent elements and swapping them if they are in the wrong order.";
            bestCase = O_of_N;
            worstCase = O_of_N_SQUARED;
            avgCase = O_of_N_SQUARED;
        }

        public override double[] Run()
        {
            if (SortJ + 1 <= max - SortI)
            {
                if (Compare(SortJ, SortJ + 1) > 0)
                {
                    Swap(SortJ, SortJ + 1);
                    swappedThisCycle = true;
                }

                SortJ++;
            }
            else
            {
                // early exit if no swaps performed on a pass
                if (!swappedThisCycle)
                    SortI = array.Length;

                swappedThisCycle = false;
                SortJ = min;
                SortI++;
            }

            isFinished = SortI > max;

            return array;
        }

        public override double[] QuickRun()
        {
            swappedThisCycle = false;

            for (SortJ = 0; SortJ + 1 <= max - SortI; SortJ++)
            {
                if (Compare(SortJ, SortJ+1) > 0)
                {
                    Swap(SortJ, SortJ + 1);
                    swappedThisCycle = true;
                }
            }

            SortI++;
            isFinished = !swappedThisCycle || SortI > max;
            // early exit if no swaps performed on a pass

            return array;
        }
    }
}
