using System;
using System.Collections.Generic;

namespace Sorting_Algorithms
{
    abstract class HeapSort : Sort
    {
        protected int root = 0;
        protected int swap;
        protected const int CREATE_HEAP = 0;
        protected const int SIFT_HEAP = 1;
        protected const int BETWEEN_SIFTS = 2;
        protected Queue<int> heapifyQueue = new Queue<int>();

        protected HeapSort(double[] array) : base(array)
        {
        }

        protected int iLeftChild(int i)
        {
            return 2 * i + 1;
        }
        protected int iRightChild(int i)
        {
            return 2 * i + 2;
        }
        protected int iParent(int i)
        {
            return (int)Math.Floor((double)(i - 1) / 2);
        }

        public override int getK()
        {
            if (MODE == SIFT_HEAP)
                return swap;
            return -1;
        }

        protected abstract void heapify(int i, bool recurse);

        public override void setBounds(int min, int max)
        {
            base.setBounds(min, max);
            SortJ = max;
            SortI = max + 1;
            root = min;
        }
    }

    class MaxHeapSort : HeapSort
    {
        /*
            each element at index i has
            children at indices 2i + 1 and 2i + 2
            its parent at index floor((i − 1) ∕ 2)
        */

        int largest = 0;

        public MaxHeapSort(double[] array) : base(array)
        {
            root = 0;
            SortJ = max;
            SortI = max + 1;
            MODE = CREATE_HEAP;
        }

        protected override void heapify(int i, bool recurse)
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

        public override int getJ()
        {
            if (MODE == CREATE_HEAP)
                return largest;
            return SortJ;
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

        protected void siftDown()
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
    }

    /*    class MinHeapSort : HeapSort
        {
            *//*
                each element at index i has
                children at indices 2i + 1 and 2i + 2
                its parent at index floor((i − 1) ∕ 2)
            *//*

            int smallest;

            public MinHeapSort(double[] array) : base(array)
            {
                root = 0;
                SortJ = min;
                SortI = min + 1;
                MODE = CREATE_HEAP;
            }

            protected override void heapify(int i, bool recurse)
            {
                // create min heap

                smallest = i;
                int left = iLeftChild(i);
                int right = iRightChild(i);

                // if left node is larger
                if (left >= min && left <= max && array[left] < array[smallest])
                    smallest = left;

                // if right node is larger
                if (right > min && right <= max && array[right] < array[smallest])
                    smallest = right;

                comparisonCount += 2;

                // if largest not parent
                if (smallest != i)
                {
                    Swap(i, smallest);

                    if (recurse)
                        heapify(smallest, recurse);
                    else
                        heapifyQueue.Enqueue(smallest);
                }
            }

            public override int getJ()
            {
                if (MODE == CREATE_HEAP)
                    return smallest;
                return SortJ;
            }

            public override double[] Run()
            {
                if (MODE == CREATE_HEAP)
                {
                    if (heapifyQueue.Count > 0)
                        heapify(heapifyQueue.Dequeue(), false);
                    else if (SortI < max)
                        heapify(++SortI, false);
                    else
                        MODE = BETWEEN_SIFTS;
                }
                else if (MODE == SIFT_HEAP && iLeftChild(root) <= SortJ)
                {
                    siftUp();
                }
                else
                {
                    Swap(SortI, SortJ);
                    SortJ++;
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

                while (iLeftChild(root) <= SortJ && MODE == SIFT_HEAP)
                {
                    siftDown();
                }

                isFinished = SortJ - SortI < 1;
                MODE = SIFT_HEAP;

                return array;
            }
        }
    */
}
