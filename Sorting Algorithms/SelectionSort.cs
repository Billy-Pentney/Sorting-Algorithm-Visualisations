namespace Sorting_Algorithms
{
    class SelectionSort : Sort
    {
        protected int indexOfMin;

        public SelectionSort(double[] array) : base(array)
        {
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
