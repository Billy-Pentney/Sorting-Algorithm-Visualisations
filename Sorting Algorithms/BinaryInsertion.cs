namespace Sorting_Algorithms
{
    class BinaryInsertion : InsertionSort
    {
        private int IndexToInsert;

        public BinaryInsertion(double[] array) : base(array)
        {
        }

        public override void setBounds(int min, int max)
        {
            base.setBounds(min, max);
            SortI = min;
            SortJ = SortI + 1;
            IndexToInsert = BinarySearchForSpace(SortI, min, SortI);
        }

        public override int getK()
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
            else if (++SortI <= max)
            {
                IndexToInsert = BinarySearchForSpace(SortI, min, SortI);
                SortJ = SortI - 1;
            }

            isFinished = SortI > max;

            return array;
        }

        public override double[] QuickRun()
        {
            IndexToInsert = BinarySearchForSpace(SortI, min, SortI);

            for (SortJ = SortI - 1; SortJ > IndexToInsert - 1; SortJ--)
            {
                Swap(SortJ, SortJ + 1);
            }

            SortI++;
            isFinished = SortI > max;

            return array;
        }

        private int BinarySearchForSpace(int toInsert, int low, int high)
        {
            int mid;

            ///applies binary search to find index where element should be located
            while (low < high)
            {
                mid = low + (high - low) / 2;

                if (Compare(toInsert, mid) >= 0)
                    low = mid + 1;
                else
                    high = mid;
            }

            return low;
        }
    }
}
