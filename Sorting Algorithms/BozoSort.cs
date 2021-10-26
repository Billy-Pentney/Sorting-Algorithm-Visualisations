using System;

namespace Sorting_Algorithms
{
    class BozoSort : Sort
    {
        Random rand = new Random();

        public BozoSort(double[] array) : base(array)
        {
            sortName = "Bozo Sort";
            sortDescription = "Bozo Sort randomly swaps pairs of elements while the list is unsorted.";
            avgCase = "O(n!)";
            bestCase = "O(n)";
            worstCase = "O(∞)";
        }

        public override double[] Run()
        {
            if (!isArraySorted())
            {
                SortI = rand.Next(min, max);
                SortJ = rand.Next(min, max);
                Swap(SortI, SortJ);
            }
            else
            {
                isFinished = true;
            }

            return array;
        }
    }
}
