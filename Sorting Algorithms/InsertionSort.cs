namespace Sorting_Algorithms
{
    class InsertionSort : Sort
    {
        public InsertionSort(double[] array) : base(array)
        {
            fillGapWithColour = true;
        }

        public override void setBounds(int min, int max)
        {
            base.setBounds(min, max);
            SortI = min+1;
            SortJ = SortI;
        }

        public override int getJ()
        {
            return SortJ;
        }

        public override double[] Run()
        {
            if (SortJ > min && SortJ <= max && Compare(SortJ-1, SortJ) > 0)
            {
                Swap(SortJ-1, SortJ);
                SortJ -= 1;
            }
            else if (++SortI <= max)
            {
                SortJ = SortI;
            }
            else
                isFinished = true;

            return array;
        }

        public override double[] QuickRun()
        {
            for (SortJ = SortI - 1; SortJ >= min; SortJ--)
            {
                if (Compare(SortJ, SortJ+1) > 0)
                    Swap(SortJ, SortJ + 1);
                else
                    break;
            }

            SortI++;
            isFinished = SortI > max;

            return array;
        }
    }
}
