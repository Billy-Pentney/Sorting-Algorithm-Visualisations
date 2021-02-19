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
        protected bool swappedThisCycle = false;
        protected double[] array;
        protected int swapCount;
        protected int comparisonCount;
        protected bool fillGapWithColour = false;
        protected bool isFinished = false;

        public Sort(double[] array)
        {
            this.array = array;
            this.swapCount = 0;
            this.comparisonCount = 0;
        }

        public bool isArraySorted()
        {
            // linear pass to determine if array is sorted (so heavy algorithms do not need to be started)

            for (int i = 1; i < array.Length; i++)
            {
                comparisonCount++;
                if (array[i] < array[i - 1])
                    return false;
            }
            return true;
        }

        public bool getFillGap()
        {
            return fillGapWithColour;
        }

        // performs next step of sort (called on a timer)
        public abstract double[] Run();

        protected virtual void Swap(int a, int b)
        {
            //swaps array values and lines for indexes a and b in the respected arrays

            double temp = array[a];
            array[a] = array[b];
            array[b] = temp;

            swapCount++;
        }

        // indicates whether sort has been fully executed
        public bool hasFinished()
        {
            return isFinished;
        }

        public virtual int getIPointer() { return SortI; }
        public virtual int getJPointer() { return SortJ; }
        public virtual int getKPointer() { return -1; }

        public int getComparisons() { return comparisonCount; }
        public int getSwaps() { return swapCount; }
    }
}
