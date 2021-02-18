using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sorting_Algorithms
{
    class BinaryInsertion : Sort
    {
        private int IndexToInsert;

        public BinaryInsertion(double[] array) : base(array)
        {
            //if this breaks, use SortI = 0, and IndexToInsert = SortI;
            SortI = 1;
            SortJ = 0;  //SortI - 1;
            IndexToInsert = BinarySearchForSpace(SortI, 0, SortI);
        }

        public override bool isFinished()
        {
            return SortI >= array.Length;
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

            /////Old Version - much faster since whole for loop completed per tick

            //IndexToInsert = BinarySearchForSpace(array, SortI, 0, SortI);

            //if (IndexToInsert != SortI)
            //{
            //    temp = array[SortI];

            //    for (int J = SortI - 1; J > IndexToInsert - 1; J--)
            //    {
            //        array[J + 1] = array[J];
            //        swapCount++;
            //    }

            //    array[IndexToInsert] = temp;
            //}

            //SortI++;

            return array;
        }

        private int BinarySearchForSpace(int toInsert, int min, int max)
        {
            int mid = (int)(array.Length / 2);

            ///applies binary search to find index where element should be located
            while (min < max)
            {
                mid = (min + max) / 2;

                if (array[toInsert] >= array[mid])
                    min = mid + 1;
                else
                    max = mid;

                comparisonCount++;
                //UpdateInfoLabels();
            }

            return min;
        }
    }
}
