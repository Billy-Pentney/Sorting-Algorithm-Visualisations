using System.Collections.Generic;

namespace Sorting_Algorithms
{
    abstract class QuickSort : Sort
    {
        protected Stack<int[]> queuedRegions = new Stack<int[]>();           //stores all pairs of values for which QuickSort still needs to be performed - if empty, the data is sorted

        protected int regionStart = 0;                                       //start of the current selection in QuickSort
        protected int regionEnd = 0;                                         //end of the current selection in QuickSort
        protected int pivotIndex = -1;                                       //pivot point for the current QuickSort

        public QuickSort(double[] array) : base(array)
        {
            sortName = "Quick Sort";

            if (!isArraySorted())
            {
                queuedRegions.Push(new int[] { 0, array.Length - 1 });
                Dequeue();
            }
            fillGapWithColour = true;
        }

        protected void addToQueue(int a, int b)
        {
            queuedRegions.Push(new int[] { a, b });
        }

        protected int[] popFromQueue()
        {
            return queuedRegions.Pop();
        }

        protected abstract void Dequeue();

        public override int getI()
        {
            return regionStart;
        }

        public override int getJ()
        {
            return regionEnd;
        }

        public override int getK()
        {
            return SortK;
        }
    }

    class HoareQuickSort : QuickSort
    {
        double pivotValue = 0;  // pivot moves during sort, so we must keep the value

        public HoareQuickSort(double[] array) : base(array)
        {
            sortDescription = "Quick Sort recursively subdivides the array, using Hoare Partitioning - the pivot is the midpoint of the current region";
            worstCase = O_of_N_log_N;
            bestCase = O_of_N_log_N;
            avgCase = O_of_N_log_N;
        }

        public override int getK()
        {
            return SortJ;
        }

        public override double[] Run()
        {
            if (regionStart < regionEnd)
                performCycle();
            else if (queuedRegions.Count > 0)
                Dequeue();
            else
                isFinished = true;
            return array;
        }

        public void performCycle()
        {
            // must compare to the actual value. pivotIndex moves during the sort
            bool pivotMoreThanFirst = array[SortI] < pivotValue; 
            bool pivotLessThanEnd = array[SortJ] > pivotValue;
            comparisonCount += 2;

            if (pivotMoreThanFirst)
                SortI++;

            if (pivotLessThanEnd)
                SortJ--;

            if (!(pivotMoreThanFirst || pivotLessThanEnd))
            {
                if (SortI >= SortJ)
                {
                    addToQueue(SortJ + 1, regionEnd);
                    addToQueue(regionStart, SortJ);

                    Dequeue();
                }
                else
                    Swap(SortI, SortJ);
            }
        }

        public override double[] QuickRun()
        {
            while (regionStart < regionEnd) {
                performCycle();
            }
            
            if (queuedRegions.Count > 0)
                Dequeue();
            else
                isFinished = true;

            return array;
        }

        protected override void Dequeue()
        {
            //retrieves the partition information so the next iteration can perform

            int[] newIndices;

            do {
                newIndices = popFromQueue();
                regionStart = newIndices[0];
                regionEnd = newIndices[1];
            } while (regionStart >= regionEnd && queuedRegions.Count > 0);

            SortK = regionStart;

            if (regionStart < regionEnd)
            {
                pivotIndex = (regionEnd + regionStart) / 2;
                pivotValue = array[pivotIndex];
                SortI = regionStart;
                SortJ = regionEnd;
            }
        }
    }

    class LomutoQuickSort : QuickSort
    {
        public LomutoQuickSort(double[] array) : base(array)
        {
            sortDescription = "Quick Sort recursively subdivides the array, using Lomuto partitioning - the pivot is the end of the region";
            worstCase = O_of_N_SQUARED;
            bestCase = O_of_N_log_N;
            avgCase = O_of_N_log_N;
        }

        public override double[] Run()
        {
            if (regionStart < regionEnd)
            {
                if (SortJ < regionEnd)
                {
                    if (Compare(SortJ, pivotIndex) < 0)
                    {
                        Swap(SortJ, SortK);
                        SortK++;
                    }

                    SortJ++;
                }
                else
                {
                    Swap(SortK, regionEnd);

                    addToQueue(SortK + 1, regionEnd);
                    addToQueue(regionStart, SortK - 1);

                    Dequeue();
                }
            }
            else if (queuedRegions.Count > 0)
            {
                Dequeue();
            }
            else
            {
                isFinished = true;
            }

            return array;

        }

        public override double[] QuickRun()
        {
            while (regionStart < regionEnd)
            {
                if (SortJ < regionEnd)
                {
                    if (Compare(SortJ, pivotIndex) < 0)
                    {
                        Swap(SortJ, SortK);
                        SortK++;
                    }

                    SortJ++;
                }
                else
                {
                    Swap(SortK, regionEnd);

                    addToQueue(SortK + 1, regionEnd);
                    addToQueue(regionStart, SortK - 1);

                    Dequeue();
                }
            }
            
            if (queuedRegions.Count > 0)
            {
                Dequeue();
            }
            else
            {
                isFinished = true;
            }

            return array;

        }

        protected override void Dequeue()
        {
            //retrieves the partition information so the next iteration can perform

            int[] newIndices;

            do
            {
                newIndices = popFromQueue();
                regionStart = newIndices[0];
                regionEnd = newIndices[1];
            } while (regionStart >= regionEnd && queuedRegions.Count > 0);

            SortJ = regionStart;
            SortK = regionStart;

            if (regionStart < regionEnd)
            {
                pivotIndex = regionEnd;
            }
        }
    }

    class LomutoMedianQuickSort : LomutoQuickSort
    {
        public LomutoMedianQuickSort(double[] array) : base(array)
        {
            sortDescription = "Quick Sort recursively subdivides the array, using Lomuto Median partitioning - the pivot is the median of the region";
            worstCase = O_of_N_log_N;
            bestCase = O_of_N_log_N;
            avgCase = O_of_N_log_N;
        }

        protected override void Dequeue()
        {
            base.Dequeue();

            if (regionStart < regionEnd)
            {
                int mid = (regionStart + regionEnd) / 2;

                if (Compare(mid, regionStart) < 0)
                    Swap(regionStart, mid);

                if (Compare(regionEnd, regionStart) < 0)
                    Swap(regionStart, regionEnd);

                if (Compare(mid, regionEnd) < 0)
                    Swap(mid, regionEnd);

                pivotIndex = regionEnd;
            }
        }
    }
}
