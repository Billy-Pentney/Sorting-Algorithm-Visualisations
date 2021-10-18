using System;
using System.Collections.Generic;

namespace Sorting_Algorithms
{
    class IntroSort : Sort
    {
        class IntroQNode
        {
            public int beginIndex { get; set; }
            public int endIndex { get; set; }
            public int recursionDepth { get; set; }

            public IntroQNode(int begin, int end, int depth)
            {
                this.beginIndex = begin;
                this.endIndex = end;
                this.recursionDepth = depth;
            }

            public int getInterval()
            {
                return endIndex - beginIndex;
            }
        }

        // enums for sort state
        protected const int QUICK_MODE = 0;
        protected const int INSERTION_MODE = 1;
        protected const int HEAP_MODE = 2;
        readonly int insertionSortThreshold = 16;   // if partition is smaller, use insertion sort

        Queue<IntroQNode> queue = new Queue<IntroQNode>();

        protected Sort currentSort;

        public IntroSort(double[] array) : base(array)
        {
            int maxRecursionDepth = (int)(2 * Math.Log(array.Length));
            queue.Enqueue(new IntroQNode(min, max, maxRecursionDepth));
        }

        public override double[] Run()
        {
            if (MODE != QUICK_MODE) {
                // get the number of swaps/comparisons before the cycle
                int swaps = currentSort.getSwaps();
                int comps = currentSort.getComparisons();

                // run one step of the sort object
                array = currentSort.Run();
                if (currentSort.hasFinished())
                    MODE = QUICK_MODE;

                // add change in swaps/comparisons
                swapCount += currentSort.getSwaps() - swaps;
                comparisonCount += currentSort.getComparisons() - comps;
            }
            else if (queue.Count > 0)
                recurseOn(queue.Dequeue());
            else
                isFinished = true;
            return array;
        }

        public override double[] QuickRun()
        {
            if (MODE != QUICK_MODE)
            {
                // get the number of swaps/comparisons before the cycle
                int swaps = currentSort.getSwaps();
                int comps = currentSort.getComparisons();

                // run one step of the sort object
                array = currentSort.QuickRun();
                if (currentSort.hasFinished())
                    MODE = QUICK_MODE;

                // add change in swaps/comparisons
                swapCount += currentSort.getSwaps() - swaps;
                comparisonCount += currentSort.getComparisons() - comps;
            }
            else if (queue.Count > 0)
                recurseOn(queue.Dequeue());
            else
                isFinished = true;
            return array;
        }

        public override int getI()
        {
            if (MODE != QUICK_MODE)
                return currentSort.getI();

            return SortI;
        }

        public override int getJ()
        {
            if (MODE != QUICK_MODE)
                return currentSort.getJ();

            return SortJ;
        }

        public override int getK()
        {
            if (MODE != QUICK_MODE)
                return currentSort.getK();

            return -1;
        }

        private void recurseOn(IntroQNode poppedNode)
        {
            SortI = poppedNode.beginIndex;
            SortJ = poppedNode.endIndex;

            int recursionDepth = poppedNode.recursionDepth;

            // if size of region less than 16
            if (SortJ - SortI < insertionSortThreshold)
            {
                initialiseInsertionSort(SortI, SortJ);
            }
            // or if max recursion depth exceeded
            else if (recursionDepth == 0)
            {
                initialiseHeapSort(SortI, SortJ);
            }
            else
            {
                // recurse via quick sort
                MODE = QUICK_MODE;

                int pivotIndex = (SortJ + SortI) / 2;
                Swap(pivotIndex, SortJ);
                pivotIndex = Partition(SortI, SortJ);

                queue.Enqueue(new IntroQNode(SortI, pivotIndex - 1, recursionDepth - 1));
                queue.Enqueue(new IntroQNode(pivotIndex + 1, SortJ, recursionDepth - 1));
            }
        }

        protected virtual void initialiseInsertionSort(int start, int end)
        {
            currentSort = new InsertionSort(array);
            currentSort.setBounds(start, end);
            MODE = INSERTION_MODE;
        }

        protected virtual void initialiseHeapSort(int start, int end)
        {
            currentSort = new HeapSort(array);
            currentSort.setBounds(start, end);
            MODE = HEAP_MODE;
        }

        private int Partition(int low, int high)
        {
            int i = low - 1;

            for (int j = low; j < high; j++)
            {
                if (Compare(j, high) <= 0)
                {
                    i++;
                    Swap(i, j);
                }
            }

            Swap(i+1, high);
            return i+1;
        }
    }

    class BinaryIntroSort : IntroSort
    {
        public BinaryIntroSort(double[] array) : base(array)
        {
        }

        protected override void initialiseInsertionSort(int start, int end)
        {
            currentSort = new BinaryInsertion(array);
            currentSort.setBounds(start, end);
            MODE = INSERTION_MODE;
        }
    }
}
