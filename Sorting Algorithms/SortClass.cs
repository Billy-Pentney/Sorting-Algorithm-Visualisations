using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sorting_Algorithms
{
    abstract class Sort
    {
        protected int SortI = 0;  // main pointer
        protected int SortJ = 0;  // secondary pointer
        protected Boolean swappedThisCycle = false;
        protected double[] array;
        protected int swapCount;
        protected int comparisonCount;

        public Sort(double[] array)
        {
            this.array = array;
            this.swapCount = 0;
            this.comparisonCount = 0;
        }

        // performs next step of sort (called on a timer)
        public abstract double[] Run();

        protected void Swap(int a, int b)
        {
            //swaps array values and lines for indexes a and b in the respected arrays

            double temp = array[a];
            array[a] = array[b];
            array[b] = temp;

/*            Line tempLine = Lines[a];
            Lines[a] = Lines[b];
            Lines[b] = tempLine;*/

            swapCount++;
        }

        // indicates whether sort has been fully executed
        public abstract bool isFinished();

        public int getIPointer() { return SortI; }
        public int getJPointer() { return SortJ; }
        
        public int getComparisons() { return comparisonCount; }
        public int getSwaps() { return swapCount; }
    }
}
