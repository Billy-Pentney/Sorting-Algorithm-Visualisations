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

        protected int regionStart = 0;                                       //start of the current selection in QuickSort
        protected int regionEnd = 0;                                         //end of the current selection in QuickSort
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
            return pivotIndex;
        }
    }

    class HoareQuickSort : QuickSort
    {
        public HoareQuickSort(double[] array) : base(array)
        {
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
            bool pivotMoreThanFirst = array[SortI] < pivotValue;
            bool pivotLessThanEnd = array[SortJ] > pivotValue;

            if (pivotMoreThanFirst)
                SortI++;

            if (pivotLessThanEnd)
                SortJ--;

            comparisonCount += 2;

            if (!(pivotMoreThanFirst || pivotLessThanEnd))
            {
                if (SortI >= SortJ)
                {
                    queuedRegions.Enqueue(new int[] { regionStart, SortJ });
                    queuedRegions.Enqueue(new int[] { SortJ + 1, regionEnd });

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
                newIndices = queuedRegions.Dequeue();
                regionStart = newIndices[0];
                regionEnd = newIndices[1];
            } while (regionStart >= regionEnd && queuedRegions.Count > 0);

            pivotIndex = regionStart;

            if (regionStart < regionEnd)
            {
                pivotValue = array[(regionEnd + regionStart) / 2];
                SortI = regionStart;
                SortJ = regionEnd;
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
            if (regionStart < regionEnd)
            {
                if (SortJ < regionEnd)
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
                    Swap(pivotIndex, regionEnd);

                    queuedRegions.Enqueue(new int[] { regionStart, pivotIndex - 1 });
                    queuedRegions.Enqueue(new int[] { pivotIndex + 1, regionEnd });

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
                    Swap(pivotIndex, regionEnd);

                    queuedRegions.Enqueue(new int[] { regionStart, pivotIndex - 1 });
                    queuedRegions.Enqueue(new int[] { pivotIndex + 1, regionEnd });

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
                newIndices = queuedRegions.Dequeue();
                regionStart = newIndices[0];
                regionEnd = newIndices[1];
            } while (regionStart >= regionEnd && queuedRegions.Count > 0);

            SortJ = regionStart;
            pivotIndex = regionStart;

            if (regionStart < regionEnd)
            {
                pivotValue = array[regionEnd];
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

            if (regionStart < regionEnd)
            {
                int mid = (regionStart + regionEnd) / 2;

                if (array[mid] < array[regionStart])
                    Swap(regionStart, mid);

                if (array[regionEnd] < array[regionStart])
                    Swap(regionStart, regionEnd);

                if (array[mid] < array[regionEnd])
                    Swap(mid, regionEnd);

                pivotValue = array[regionEnd];
            }
        }
    }
}
