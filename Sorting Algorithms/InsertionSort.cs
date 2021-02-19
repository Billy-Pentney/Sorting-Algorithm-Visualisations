using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sorting_Algorithms
{
    class InsertionSort : Sort
    {
        public InsertionSort(double[] array) : base(array)
        {
            fillGapWithColour = true;
        }

        public override int getJPointer()
        {
            return SortJ + 1;
        }

        public override double[] Run()
        {
            if (SortJ >= 0 && array[SortJ] > array[SortJ + 1])
            {
                Swap(SortJ, SortJ + 1);
                SortJ -= 1;
            }
            else
            {
                SortI++;
                SortJ = SortI - 1;
            }

            comparisonCount++;
            isFinished = (SortI >= array.Length);

            return array;
        }
    }
}
