namespace Sorting_Algorithms
{
    class SelectionSort : Sort
    {
        protected int indexOfMin;

        public SelectionSort(double[] array) : base(array)
        {
            sortName = "Selection Sort";
            sortDescription = "Selection Sort builds a sorted region by means of linear passes to identify the minimum unsorted element.";
            bestCase = O_of_N_SQUARED;
            avgCase = O_of_N_SQUARED;
            worstCase = O_of_N_SQUARED;

            SortI = min;
            indexOfMin = SortI;
            SortJ = indexOfMin + 1;
        }

        public override int getK()
        {
            return indexOfMin;
        }

        public override double[] Run()
        {
            if (SortJ <= max && SortJ > indexOfMin)
            {
                //finds index of smallest element in array
                if (Compare(SortJ, indexOfMin) < 0)
                    indexOfMin = SortJ;

                SortJ++;
            }
            else if (SortI <= max)
            {
                Swap(SortI, indexOfMin);

                SortI++;
                indexOfMin = SortI;
                SortJ = indexOfMin + 1;
            }

            isFinished = (SortI > max);

            return array;
        }

        public override double[] QuickRun()
        {
            for (SortJ = SortI; SortJ <= max; SortJ++)
            {
                if (Compare(SortJ, indexOfMin) < 0)
                    indexOfMin = SortJ;
            }

            Swap(SortI, indexOfMin);

            SortI++;
            indexOfMin = SortI;

            isFinished = (SortI > max);

            return array;
        }
    }
}
