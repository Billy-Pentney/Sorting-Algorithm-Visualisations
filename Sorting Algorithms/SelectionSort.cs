using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sorting_Algorithms
{
    class SelectionSort : Sort
    {
        protected int minIndex;

        public SelectionSort(double[] array) : base(array)
        {
            SortI = 0;
            minIndex = SortI;
            SortJ = minIndex + 1;
        }

        public override int getK()
        {
            return minIndex;
        }

        public override double[] Run()
        {
            if (SortJ < array.Length && SortJ > minIndex)
            {
                //finds smallest instance in array

                if (array[SortJ] < array[minIndex])
                {
                    minIndex = SortJ;
                }

                comparisonCount++;
                SortJ++;
            }
            else if (SortI < array.Length)
            {
                Swap(SortI, minIndex);

                SortI++;
                minIndex = SortI;
                SortJ = minIndex + 1;
            }

            isFinished = (SortI >= array.Length);

            return array;
        }

        public override double[] QuickRun()
        {
            for (SortJ = SortI; SortJ < array.Length; SortJ++)
            {
                if (array[SortJ] < array[minIndex])
                    minIndex = SortJ;

                comparisonCount++;
            }

            Swap(SortI, minIndex);

            SortI++;
            minIndex = SortI;

            isFinished = (SortI >= array.Length);

            return array;
        }
    }
}
