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
            pos = min;
        }

        public override int getK()
        {
            return pos;
        }

        public override double[] QuickRun()
        {
            pos = SortI;

            for (int i = SortI + 1; i <= max; i++)
            {
                if (Compare(i, SortI) < 0)
                    pos++;
            }

            if (pos != SortI)
            {
                while (Compare(SortI, pos) == 0)
                    pos++;

                Swap(pos, SortI);
            }

            if (pos == SortI)
                SortI++;

            isFinished = (SortI >= max);

            return array;
        }

        public override double[] Run()
        {
            if (SortJ <= max)
            {
                if (Compare(SortJ, SortI) < 0)
                    pos++;
                SortJ++;
            }
            else
            {
                while (pos != SortI && Compare(SortI, pos) == 0)
                {
                    pos++;
                }

                if (pos != SortI)
                    Swap(pos, SortI);
                else
                    SortI++;

                SortJ = SortI + 1;
                pos = SortI;
            }

            isFinished = (SortI >= max);

            return array;
        }
    }
}
