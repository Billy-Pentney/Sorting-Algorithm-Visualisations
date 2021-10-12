using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sorting_Algorithms
{
    class MergeSort : Sort
    {
        int[] left;
        int[] right;
        int[] temp;
        int temp_i = 0;
        Queue<int[]> queue = new Queue<int[]>();

        bool finishedNextTurn = false;  
        // indicates that after the current merge round, the algorithm will complete

        // array where data will be placed as it is sorted
        double[] newArr;
        int newArrI = 0;

        public MergeSort(double[] array) : base(array)
        {
            newArr = new double[array.Length];

            queue.Enqueue(new int[] { 0 });
            newArr[0] = array[0];

            // divides array into singletons
            for (int i = 1; i < array.Length; i++)
            { 
                newArr[i] = array[i];
                queue.Enqueue(new int[] { i });
            }

            isFinished = isArraySorted();

            getNextSortedLists();
        }

        public override int getI()
        {
            return newArrI - 1;
        }

        public override int getJ()
        {
            if (SortI < left.Length)
            {
                return left[SortI];
            }
            return -1;
        }

        private void moveNew(int i)
        {
            newArr[newArrI] = array[i];
            newArrI++;

            if (newArrI == newArr.Length)
                newArrI = 0;

            swapCount++;
        }

        public override double[] Run()
        {           
            if (isFinished)
            {
                return newArr;
            }

            if (SortI < left.Length && SortJ < right.Length)
            {
                int left_head = left[SortI];
                int right_head = right[SortJ];

                if (array[left_head] <= array[right_head])
                {
                    temp[temp_i] = left_head;
                    SortI++;
                }
                else
                {
                    temp[temp_i] = right_head;
                    SortJ++;
                }

                moveNew(temp[temp_i]);

                comparisonCount++;
                temp_i++;
            }
            else if (SortJ < right.Length)
            {   
                temp[temp_i] = right[SortJ];
                moveNew(temp[temp_i]);
                temp_i++;
                SortJ++;
            }
            else if (SortI < left.Length)
            {
                temp[temp_i] = left[SortI];
                moveNew(temp[temp_i]);
                temp_i++;
                SortI++;
            }
            else
            {
                queue.Enqueue(temp);
                getNextSortedLists();
            }

            return newArr;
        }

        public override double[] QuickRun()
        {
            if (isFinished)
            {
                return newArr;
            }

            while (SortI < left.Length && SortJ < right.Length)
            {
                int left_head = left[SortI];
                int right_head = right[SortJ];

                if (array[left_head] <= array[right_head])
                {
                    temp[temp_i] = left_head;
                    SortI++;
                }
                else
                {
                    temp[temp_i] = right_head;
                    SortJ++;
                }

                moveNew(temp[temp_i]);

                comparisonCount++;
                temp_i++;
            }
            
            while (SortJ < right.Length)
            {
                temp[temp_i] = right[SortJ];
                moveNew(temp[temp_i]);
                temp_i++;
                SortJ++;
            }

            while (SortI < left.Length)
            {
                temp[temp_i] = left[SortI];
                moveNew(temp[temp_i]);
                temp_i++;
                SortI++;
            }

            queue.Enqueue(temp);
            getNextSortedLists();

            return newArr;
        }

        private void getNextSortedLists()
        {
            if (finishedNextTurn || isFinished || queue.Count == 0)
            {
                isFinished = true;
            }
            else if (queue.Count >= 1)
            {
                left = queue.Dequeue();
                SortI = 0;
                SortJ = 0;

                if (queue.Count == 0)
                {
                    newArrI = 0;
                    right = new int[0];
                    finishedNextTurn = true;
                }
                else
                {
                    right = queue.Dequeue();
                    /*                  
                    if (newArr.Length - 1 - newArrI < temp.Length)
                        newArrI = 0;
                    */
                }

                temp = new int[left.Length + right.Length];
                temp_i = 0;
            }
        }
    }
}
