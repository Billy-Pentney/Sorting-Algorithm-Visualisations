using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sorting_Algorithms
{
    class BinaryInsertion : InsertionSort
    {
        private int IndexToInsert;

        public BinaryInsertion(double[] array) : base(array)
        {
            //if this breaks, use SortI = 0, and IndexToInsert = SortI;
            SortI = 1;
            SortJ = 0;
            IndexToInsert = BinarySearchForSpace(SortI, 0, SortI);
        }

        public override int getKPointer()
        {
            if (IndexToInsert < SortJ + 1)
                return IndexToInsert;

            return -1;
        }

        public override double[] Run()
        {
            if (IndexToInsert != SortI)
            {
                if (SortJ > IndexToInsert - 1)
                {
                    Swap(SortJ + 1, SortJ);
                    SortJ--;
                }
                else
                {
                    IndexToInsert = SortI;
                }
            }
            else if (++SortI < array.Length)
            {
                IndexToInsert = BinarySearchForSpace(SortI, 0, SortI);
                SortJ = SortI - 1;
            }

            isFinished = (SortI >= array.Length);

            return array;
        }

        private int BinarySearchForSpace(int toInsert, int min, int max)
        {
            int mid;

            ///applies binary search to find index where element should be located
            while (min < max)
            {
                mid = min + (max - min) / 2;

                if (array[toInsert] >= array[mid])
                    min = mid + 1;
                else
                    max = mid;

                comparisonCount++;
            }

            return min;
        }
    }
}
