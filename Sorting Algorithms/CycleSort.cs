using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sorting_Algorithms
{
    class CycleSort : Sort
    {
        int pos = 0;

        public CycleSort(double[] array) : base(array)
        {
            SortJ = SortI + 1;
            pos = 0;
        }

        public override int getIPointer()
        {
            return pos;
        }

        public override double[] Run()
        {
            // FAST VERSION
            pos = SortI;

            for (int i = SortI + 1; i < array.Length; i++)
            {
                comparisonCount++;
                if (array[i] < array[SortI])
                    pos++;
            }

            if (pos != SortI)
            {
                comparisonCount++;
                while (array[SortI] == array[pos])
                {
                    pos++;
                }

                Swap(pos, SortI);
            }

            if (pos == SortI)
            {
                SortI++;
            }


            // SLOW VERSION
            //if (SortJ < array.Length)
            /*            {
                            comparisonCount++;
                            if (array[SortJ] < array[SortI])
                                pos++;
                            SortJ++;
                        }
                        else if (pos != SortI)
                        {
                            comparisonCount++;
                            if (array[SortI] == array[pos]) 
                                pos++;
                        }
                        else
                        {
                            Swap(SortI, pos);

                            if (pos == SortI)
                            {
                                SortI++;
                            }

                            SortJ = SortI + 1;
                            pos = SortI;
                        }*/

            isFinished = (SortI >= array.Length - 1);

            return array;
        }
    }
}
