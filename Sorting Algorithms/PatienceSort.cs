using System.Collections.Generic;
using System.Linq;

namespace Sorting_Algorithms
{
    class PatienceSort : Sort
    {
        List<Stack<int>> piles = new List<Stack<int>>();
        double[] newArr;

        public PatienceSort(double[] array) : base(array)
        {
            newArr = new double[array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                newArr[i] = array[i];
            }
        }

        public override double[] Run()
        {
            // adds each array element to the piles
            if (SortJ < array.Length)
            {
                double curr = array[SortJ];

                // find first pile with top card >= current
                foreach (Stack<int> pile in piles)
                {
                    int top = pile.Peek();
                    comparisonCount++;
                    if (array[top] >= curr)
                    {
                        pile.Push(SortJ);
                        SortJ++;
                        return newArr;
                    }
                }

                // if no pile found, add a new one
                Stack<int> newPile = new Stack<int>();
                newPile.Push(SortJ);
                piles.Add(newPile);
                SortJ++;
            }
            else if (SortI < array.Length)
            {
                int minPile = 0;
                double minCard = -1;

                // find pile with smallest top card
                for (int i = piles.Count - 1; i > -1; i--)
                {
                    int top = piles[i].Peek();
                    comparisonCount++;
                    if (minCard == -1 || array[top] < minCard)
                    {
                        minPile = i;
                        minCard = array[top];
                    }
                }

                Stack<int> pile = piles[minPile];

                swapCount++;
                newArr[SortI] = array[pile.Pop()];

                // delete empty piles
                if (pile.Count == 0)
                    piles.Remove(pile);

                SortI++;
            }
            else
            {
                isFinished = true;
            }

            return newArr;
        }
    }
}
