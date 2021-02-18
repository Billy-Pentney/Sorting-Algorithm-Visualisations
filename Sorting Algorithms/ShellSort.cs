using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sorting_Algorithms
{
    class ShellSort : Sort
    {
        private int gap;

        public ShellSort(double[] array) : base(array)
        {
            gap = 1;
            while (gap < array.Length / 2)
            {
                //gap = gap * 3 + 1;
                gap = (gap + 1) * 2 - 1;
            }
        }

        public override bool isFinished()
        {
            return gap == -1;
        }

        public override double[] Run()
        {
            if (SortJ - gap >= 0 && SortJ < array.Length)
            {
                if (array[SortJ] < array[SortJ - gap])
                {
                    Swap(SortJ, SortJ - gap);
                }
                else
                {
                    SortI += 1;
                    SortJ = SortI + gap;
                }
                SortJ -= gap;
            }
            else if (SortI < array.Length)
            {
                SortI += 1;
                SortJ = SortI;
            }
            else if (gap > 1)
            {
                // gap /= 2;
                //gapShell = (gap - 1) / 3;

                //2^k - 1, starting at 1
                gap = (gap + 1) / 2 - 1;
                SortI = gap;
                SortJ = SortI;
            }
            else
            {
                gap = -1;
                SortI = -1;
                SortJ = -1;
            }

            comparisonCount++;
            return array;
        }
    }
}
