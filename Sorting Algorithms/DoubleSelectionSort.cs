namespace Sorting_Algorithms
{
    class DoubleSelectionSort : SelectionSort
    {
        private int indexOfMax;

        public DoubleSelectionSort(double[] array) : base(array)
        {
            indexOfMax = max - SortI - 1;
        }

        public override double[] Run()
        {
            if (SortJ < array.Length - SortI)
            {
                //finds minimum value
                if (Compare(SortJ, indexOfMin) < 0)
                    indexOfMin = SortJ;
                // finds maximum value
                if (Compare(max-SortJ, indexOfMax) > 0)
                    indexOfMax = array.Length - SortJ - 1;

                SortJ++;
            }
            else
            {
                int firstSwapPos = SortI;
                int secondSwapPos = max - SortI;

                if (indexOfMin != indexOfMax)
                {
                    Swap(firstSwapPos, indexOfMin);

                    // if the maximum is moved by the first swap, then we know what index it is
                    if (indexOfMax == firstSwapPos)
                        indexOfMax = indexOfMin;

                    Swap(secondSwapPos, indexOfMax);
                }
                else
                {
                    // min and max need to be swapped
                    Swap(indexOfMin, indexOfMax);
                }

                SortI++;
                indexOfMin = SortI;
                indexOfMax = max - SortI;
                SortJ = indexOfMin + 1;
            }
            
            isFinished = (SortI > max / 2);

            return array;
        }

        public override double[] QuickRun()
        {
            for (SortJ = SortI; SortJ <= max - SortI; SortJ++)
            {
                // finds minimum value
                if (Compare(SortJ, indexOfMin) < 0)
                    indexOfMin = SortJ;
                // finds maximum value
                if (Compare(max-SortJ, indexOfMax) > 0)
                    indexOfMax = max - SortJ;
            }

            int firstSwapPos = SortI;
            int secondSwapPos = max - SortI;

            if (indexOfMin != indexOfMax)
            {
                Swap(firstSwapPos, indexOfMin);

                if (indexOfMax == firstSwapPos)
                    indexOfMax = indexOfMin;
                    //if the maximum is moved by the first swap, then its index must be known

                Swap(secondSwapPos, indexOfMax);
            }
            else
            {
                // min and max need to be swapped
                Swap(indexOfMin, indexOfMax);
            }

            SortI++;
            indexOfMin = SortI;
            indexOfMax = max - SortI;
            SortJ = indexOfMin + 1;

            isFinished = (SortI > max / 2);

            return array;
        }
    }
}
