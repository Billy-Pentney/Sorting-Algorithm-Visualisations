using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sorting_Algorithms
{
    abstract class QuickSort : Sort
    {
        protected Queue<int[]> queuedRegions = new Queue<int[]>();            //stores all pairs of values for which QuickSort still needs to be performed - if empty, the data is sorted

        protected int currentEnd = 0;                                         //end of the current selection in QuickSort
        protected int currentStart = 0;                                       //start of the current selection in QuickSort
        protected int pivotIndex = 0;                                         //current index being compared to pivotValue in QuickSort
        protected double pivotValue = 0;                                      //pivot point for the current QuickSort

        public QuickSort(double[] array) : base(array)
        {
            if (!isArraySorted())
            {
                queuedRegions.Enqueue(new int[] { 0, array.Length - 1 });
                Dequeue();
            }
            fillGapWithColour = true;
        }

        protected abstract void Dequeue();

        public override int getIPointer()
        {
            return currentStart;
        }

        public override int getJPointer()
        {
            return currentEnd;
        }

        public override int getKPointer()
        {
            return pivotIndex;
        }
    }

    class HoareQuickSort : QuickSort
    {
        public HoareQuickSort(double[] array) : base(array)
        {
        }

        public override int getKPointer()
        {
            return SortJ;
        }

        public override double[] Run()
        {
            if (currentStart < currentEnd)
            {
                bool firstCheck = array[SortI] < pivotValue;
                bool secondCheck = array[SortJ] > pivotValue;

                if (firstCheck)
                    SortI++;

                if (secondCheck)
                    SortJ--;

                comparisonCount += 2;

                if (!(firstCheck || secondCheck))
                {
                    if (SortI >= SortJ)
                    {
                        queuedRegions.Enqueue(new int[] { currentStart, SortJ });
                        queuedRegions.Enqueue(new int[] { SortJ + 1, currentEnd });

                        Dequeue();
                    }
                    else
                    {
                        Swap(SortI, SortJ);
                    }
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

        protected override void Dequeue()
        {
            //retrieves the partition information so the next iteration can perform

            int[] newIndices;

            do {
                newIndices = queuedRegions.Dequeue();
                currentStart = newIndices[0];
                currentEnd = newIndices[1];
            } while (currentStart >= currentEnd && queuedRegions.Count > 0);

            pivotIndex = currentStart;

            if (currentStart < currentEnd)
            {
                pivotValue = array[(currentEnd + currentStart) / 2];
                SortI = currentStart;
                SortJ = currentEnd;
            }
        }
    }

    class LomutoQuickSort : QuickSort
    {
        public LomutoQuickSort(double[] array) : base(array)
        {
        }

        public override double[] Run()
        {
            if (currentStart < currentEnd)
            {
                if (SortJ < currentEnd)
                {
                    if (array[SortJ] < pivotValue)
                    {
                        Swap(SortJ, pivotIndex);
                        pivotIndex++;
                    }

                    comparisonCount++;
                    SortJ++;
                }
                else
                {
                    Swap(pivotIndex, currentEnd);

                    queuedRegions.Enqueue(new int[] { currentStart, pivotIndex - 1 });
                    queuedRegions.Enqueue(new int[] { pivotIndex + 1, currentEnd });

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

        protected override void Dequeue()
        {
            //retrieves the partition information so the next iteration can perform

            int[] newIndices;

            do
            {
                newIndices = queuedRegions.Dequeue();
                currentStart = newIndices[0];
                currentEnd = newIndices[1];
            } while (currentStart >= currentEnd && queuedRegions.Count > 0);

            SortJ = currentStart;
            pivotIndex = currentStart;

            if (currentStart < currentEnd)
            {
                pivotValue = array[currentEnd];
            }
        }
    }

    class LomutoMedianQuickSort : LomutoQuickSort
    {
        public LomutoMedianQuickSort(double[] array) : base(array)
        {
        }

        protected override void Dequeue()
        {
            base.Dequeue();

            if (currentStart < currentEnd)
            {
                int mid = (currentStart + currentEnd) / 2;

                if (array[mid] < array[currentStart])
                    Swap(currentStart, mid);

                if (array[currentEnd] < array[currentStart])
                    Swap(currentStart, currentEnd);

                if (array[mid] < array[currentEnd])
                    Swap(mid, currentEnd);

                pivotValue = array[currentEnd];
            }
        }
    }
}
