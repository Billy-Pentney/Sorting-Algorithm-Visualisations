﻿using System;
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

        public override bool isFinished()
        {
            return SortI >= array.Length;
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

            return array;
        }
    }
}
