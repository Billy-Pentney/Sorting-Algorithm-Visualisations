using System;

namespace Sorting_Algorithms
{
    abstract class Sort
    {
        protected int SortI = 0;  // main pointer
        protected int SortJ = 0;  // secondary pointer
        protected int SortK = 0;  // tertiary pointer
        protected bool swappedThisCycle = false;
        protected double[] array;
        protected int swapCount;
        protected int comparisonCount;
        protected bool fillGapWithColour = false;
        protected bool isFinished = false;
        protected int min = 0;
        protected int max = 0;

        public Sort(double[] array)
        {
            this.array = array;
            this.swapCount = 0;
            this.comparisonCount = 0;
            setBounds(0, array.Length - 1);
        }

        protected void updateArray(double[] array)
        {
            this.array = array;
        }

        public bool isArraySorted()
        {
            // linear pass to determine if array is sorted (so heavy algorithms do not need to be started)

            for (int i = min; i < max; i++)
            {
                comparisonCount++;
                if (array[i] > array[i + 1])
                    return false;
            }
            return true;
        }

        public bool getFillGap()
        {
            // indicates whether the gap between the SortI and SortJ pointers should be filled or not
            // default: false -> can be overridden depending on sort
            return fillGapWithColour;
        }

        // performs next step of sort per function call (Slow)
        public abstract double[] Run();

        // iterates several steps of sort per function call (Fast)
        public virtual double[] QuickRun()
        {
            return Run();
        }

        public virtual void setBounds(int min, int max)
        {
            this.min = min;
            this.max = max;
        }

        protected virtual void Swap(int a, int b)
        {
            //swaps array values and lines for indexes a and b in the respected arrays

            if (a < min || b < min || a > max || b > max)
                throw new Exception("Attempt to swap at an index out of given bounds");

            double temp = array[a];
            array[a] = array[b];
            array[b] = temp;

            swapCount++;
        }

        protected virtual int Compare(int a, int b)
        {
            if (a == b)
                return 0;

            if (a < min || b < min || a > max || b > max)
                throw new Exception("Attempt to swap at an index out of given bounds");

            comparisonCount++;

            // returns < 0 if array[a] < array[b], 0 if equal, or > 0 if array[a] > array[b]
            return array[a].CompareTo(array[b]);
        }

        // indicates whether sort has been fully executed
        public bool hasFinished()
        {
            return isFinished;
        }

        public virtual int getI() { return SortI; }
        public virtual int getJ() { return SortJ; }
        public virtual int getK() { return -1; }

        public int getComparisons() { return comparisonCount; }
        public int getSwaps() { return swapCount; }
    }
}
