using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sorting_Algorithms
{
    class SlowSort : Sort
    {
        Queue<int[]> queue = new Queue<int[]>();

        public SlowSort(double[] array) : base(array)
        {
            queue.Enqueue(new int[] { min, max });
            sortName = "Slow Sort";
            sortDescription = "Slow sort performs a 'multiply-and-surrender' recursion, sorting the two halves of the array to extract the maximum element, before reducing its range.";
            bestCase = ">" + O_of_N_SQUARED;
            avgCase = "O(n^[log(n)])";
            worstCase = O_of_INFINITY;
        }

        public override double[] Run()
        {
            if (queue.Count > 0)
            {
                int[] range = queue.Dequeue();
                SortI = range[0];
                SortJ = range[1];
                sort(SortI, SortJ);
            }
            else
                isFinished = true;
            return array;
        }

        private void sort(int start, int end)
        {
            if (start >= end) {
                return;
            }

            int mid = (start + end) / 2;

            sort(start, mid);
            sort(mid + 1, end);
            if (Compare(mid, end) > 0)
                Swap(mid, end);
            queue.Enqueue(new int[] { start, end-1 });
        }
    }
}
