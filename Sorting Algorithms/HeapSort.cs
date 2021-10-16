using System;
using System.Collections.Generic;

namespace Sorting_Algorithms
{
    class HeapSort : Sort
    {
        /*
            each element at index i has
            children at indices 2i + 1 and 2i + 2
            its parent at index floor((i − 1) ∕ 2)
        */

        int root = 0;
        int swap;
        int largest;
        int MODE = 0;
        const int CREATE_HEAP = 0;
        const int SIFT_HEAP = 1;
        const int BETWEEN_SIFTS = 2;
        Queue<int> heapifyQueue = new Queue<int>();

        public HeapSort(double[] array) : base(array)
        {
            root = 0;
            SortJ = max;
            SortI = max + 1;
            MODE = CREATE_HEAP;
        }

        public override void setBounds(int min, int max)
        {
            base.setBounds(min, max);
            SortJ = max;
            SortI = max + 1;
            root = min;
        }

        public void heapify(int i, bool recurse)
        {
            largest = i;
            int left = iLeftChild(i);
            int right = iRightChild(i);

            // if left node is larger
            if (left >= min && left <= max && array[left] > array[largest])
                largest = left;

            // if right node is larger
            if (right > min && right <= max && array[right] > array[largest])
                largest = right;

            comparisonCount += 2;

            // if largest not parent
            if (largest != i)
            {
                Swap(i, largest);

                if (recurse)
                    heapify(largest, recurse);
                else
                    heapifyQueue.Enqueue(largest);
            }
        }

        private int iLeftChild(int i)
        {
            return 2 * i + 1;
        }
        private int iRightChild(int i)
        {
            return 2 * i + 2;
        }
        private int iParent(int i)
        {
            return (int)Math.Floor((double)(i-1)/2);
        }

        public override int getJ()
        {
            if (MODE == CREATE_HEAP)
                return largest;
            return SortJ;
        }

        public override int getK()
        {
            if (MODE == SIFT_HEAP)
                return swap;
            return -1;
        }

        public void siftDown()
        {
            int child = iLeftChild(root);
            swap = root;

            if (array[swap] < array[child])
                swap = child;
            if (child + 1 <= SortJ && array[swap] < array[child + 1])
                swap = child + 1;

            comparisonCount += 2;

            if (swap == root)
            {
                MODE = BETWEEN_SIFTS;
                return;
            }
            else
            {
                Swap(root, swap);
                root = swap;
            }

            if (iLeftChild(root) > SortJ)
                MODE = BETWEEN_SIFTS;
        }

        public override double[] Run()
        {
            if (MODE == CREATE_HEAP)
            {
                if (heapifyQueue.Count > 0)
                    heapify(heapifyQueue.Dequeue(), false);
                else if (SortI > min)
                    heapify(--SortI, false);
                else
                    MODE = BETWEEN_SIFTS;
            }
            else if (MODE == SIFT_HEAP && iLeftChild(root) <= SortJ)
            {
                siftDown();
            }
            else
            {
                Swap(SortI, SortJ);
                SortJ--;
                root = SortI;
                MODE = SIFT_HEAP;
                isFinished = SortJ - SortI < 1;
            }

            return array;
        }

        public override double[] QuickRun()
        {
            if (MODE == CREATE_HEAP)
            {
                do
                {
                    heapify(--SortI, true);
                } while (SortI > min);
                MODE = SIFT_HEAP;
            }

            Swap(SortI, SortJ);
            SortJ--;
            root = SortI;

            while (iLeftChild(root) <= SortJ && MODE == SIFT_HEAP) {
                siftDown();
            }

            isFinished = SortJ - SortI < 1;
            MODE = SIFT_HEAP;

            return array;
        }
    }
}
