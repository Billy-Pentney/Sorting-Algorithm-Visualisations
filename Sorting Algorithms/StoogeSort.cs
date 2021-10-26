using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sorting_Algorithms
{
    class StoogeSort : Sort
    {
        Stack<int[]> queue = new Stack<int[]>();

        public StoogeSort(double[] array) : base(array)
        {
            sortName = "Stooge Sort";
            sortDescription = "Stooge Sort partitions the list based on thirds, recursively sorting the first 2/3, then the last 2/3, then the first 2/3 again.";
            bestCase = "O(n^2.71...)";
            worstCase = bestCase;
            avgCase = bestCase;

            queue.Push(new int[] { min, max });
        }

        public override double[] Run()
        {
            if (queue.Count > 0)
            {
                int[] interval = queue.Pop();
                SortI = interval[0];
                SortJ = interval[1];

                if (Compare(SortI, SortJ) > 0)
                    Swap(SortI, SortJ);

                double range = SortJ - SortI + 1;
                if (range > 2)
                {
                    int third = (int)(range / 3);
                    int[] firstPartition = new int[] { SortI, SortJ - third };
                    int[] secondPartition = new int[] { SortI + third, SortJ };
                    queue.Push(firstPartition);
                    queue.Push(secondPartition);
                    queue.Push(firstPartition);
                }
            }
            else
                isFinished = true;

            return array;
        }

        public override double[] QuickRun()
        {
            for (int i = 0; i < 10; i++)
            {
                Run();
                if (queue.Count == 0)
                    break;
            }
            return array;
        }
    }
}