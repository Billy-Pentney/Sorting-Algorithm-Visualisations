using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sorting_Algorithms
{
    class DoubleSelectionSort : SelectionSort
    {
        private int maxIndex;

        public DoubleSelectionSort(double[] array) : base(array)
        {
            maxIndex = array.Length - SortI - 1;
        }

        public override double[] Run()
        {
            if (SortJ < array.Length - SortI)
            {
                //finds minimum value
                if (array[SortJ] < array[minIndex])
                    minIndex = SortJ;
                // finds maximum value
                if (array[array.Length - SortJ - 1] > array[maxIndex])
                    maxIndex = array.Length - SortJ - 1;

                comparisonCount += 2;
                SortJ++;
            }
            else
            {
                int firstSwapPos = SortI;
                int secondSwapPos = array.Length - SortI - 1;

                if (minIndex != maxIndex)
                {
                    Swap(firstSwapPos, minIndex);

                    if (maxIndex == firstSwapPos)
                    {
                        maxIndex = minIndex;
                        //if the maximum is moved by the first swap, then its index must be known
                    }

                    Swap(secondSwapPos, maxIndex);
                }
                else
                {
                    //min and max are in each other's spaces
                    Swap(minIndex, maxIndex);
                }

                SortI++;
                minIndex = SortI;
                maxIndex = array.Length - SortI - 1;
                SortJ = minIndex + 1;
            }
            
            isFinished = (SortI >= array.Length / 2);

            return array;
        }

        public override double[] QuickRun()
        {
            for (SortJ = SortI; SortJ < array.Length - SortI; SortJ++)
            {
                // finds minimum value
                if (array[SortJ] < array[minIndex])
                    minIndex = SortJ;
                // finds maximum value
                if (array[array.Length - SortJ - 1] > array[maxIndex])
                    maxIndex = array.Length - SortJ - 1;

                comparisonCount += 2;
            }

            int firstSwapPos = SortI;
            int secondSwapPos = array.Length - SortI - 1;

            if (minIndex != maxIndex)
            {
                Swap(firstSwapPos, minIndex);

                if (maxIndex == firstSwapPos)
                    maxIndex = minIndex;
                    //if the maximum is moved by the first swap, then its index must be known

                Swap(secondSwapPos, maxIndex);
            }
            else
            {
                //min and max are in each other's spaces
                Swap(minIndex, maxIndex);
            }

            SortI++;
            minIndex = SortI;
            maxIndex = array.Length - SortI - 1;
            SortJ = minIndex + 1;

            isFinished = (SortI >= array.Length / 2);

            return array;
        }
    }
}
