using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sorting_Algorithms
{
    class BucketSort : Sort
    {
        List<double>[] buckets;
        readonly int NUM_OF_BUCKETS = 10;
        int currentBucketIndex = -1;

        const int MAKE_BUCKETS = 0;
        const int SORT_BUCKETS = 1;
        const int CONCAT_BUCKETS = 2;

        // uses insertion sort if bucket contains fewer than this number of items
        // otherwise uses recursive call to bucket sort
        const int INSERT_BUCKET_THRESHOLD = 6;  

        readonly double minArrValue = 3.0;
        readonly double maxArrValue = 100.0;
        readonly double rangeOfBucket = 0;

        Sort currentSort;

        public BucketSort(double[] array, double minArrValue, double maxArrValue) : base(array)
        {
            // no need to sort fewer than two elements
            if (array.Length < 2)
            {
                isFinished = true;
                return;
            }

            // must be able to split the array into singletons
            NUM_OF_BUCKETS = Math.Min(array.Length, NUM_OF_BUCKETS);

            // initialise list of buckets
            this.buckets = new List<double>[NUM_OF_BUCKETS];

            // initialise each bucket
            for (int i = 0; i < buckets.Length; i++)
            {
                buckets[i] = new List<double>();
            }

            this.minArrValue = minArrValue;
            this.maxArrValue = maxArrValue;
            this.MODE = MAKE_BUCKETS;
            this.rangeOfBucket = (this.maxArrValue - this.minArrValue) / NUM_OF_BUCKETS;
            this.fillGapWithColour = true;
        }

        private int getBucketIndex(double arrVal)
        {
            return (int)Math.Max(Math.Min(Math.Floor((arrVal - minArrValue) / rangeOfBucket), buckets.Length - 1),0);
        }

        public override int getI()
        {
            if (currentSort is BucketSort)
                return SortK + currentSort.getI();
            return SortK;
        }

        public override int getJ()
        {
            if (MODE == MAKE_BUCKETS)
                return Math.Max(0, SortJ - 1);
            else if (currentSort is BucketSort)
                return SortK + currentSort.getJ();
            else if (currentSort is InsertionSort)
                return SortK + currentSort.getMax();
            else if (currentBucketIndex > -1 && currentBucketIndex < buckets.Length)
                return SortK + buckets[currentBucketIndex].Count;
            
            return SortK;
        }

        public override int getK()
        {
            if (currentSort is InsertionSort)
                return SortK + currentSort.getI() - 1;
            else if (currentSort is BucketSort)
                return SortK + currentSort.getK();
            return SortK;
        }

        public override double[] Run()
        {
            if (MODE == MAKE_BUCKETS)
            {
                addNextValueToBucket();
                if (SortJ > max)
                    MODE = SORT_BUCKETS;
            }
            else if (MODE == SORT_BUCKETS)
            {
                if (currentSort != null && !currentSort.hasFinished())
                    sortCurrentBucket();
                else
                    initNextBucketSort();
            }
            else
            {
                isFinished = true;
                array = concatBuckets();
                return array;
            }

            return concatBuckets();
        }

        public override double[] QuickRun()
        {
            if (MODE == MAKE_BUCKETS) {
                int iterations = Math.Min(array.Length / NUM_OF_BUCKETS, max - SortJ);

                for (; iterations > -1; iterations--) {
                    addNextValueToBucket();
                }

                if (SortJ > max)
                    MODE = SORT_BUCKETS;
            }
            else if (MODE == SORT_BUCKETS)
            {
                while (currentSort != null && !currentSort.hasFinished())
                    sortCurrentBucket();

                initNextBucketSort();
            }
            else { 
                isFinished = true;
                array = concatBuckets();
                return array;
            }

            return concatBuckets();
        }

        private void addNextValueToBucket()
        {
            double value = array[SortJ];
            int index = getBucketIndex(value);
            // add the value to its bucket
            buckets[index].Add(value);
            swapCount++;
            SortJ++;
        }

        private void initNextBucketSort()
        {
            if (currentBucketIndex > -1)
                SortK += buckets[currentBucketIndex].Count;

            currentBucketIndex++;

            if (currentBucketIndex < buckets.Length)
                currentSort = beginNewSort(currentBucketIndex, buckets[currentBucketIndex].ToArray());
            else
                MODE = CONCAT_BUCKETS;
        }

        private void sortCurrentBucket()
        {
            int prevSwaps = currentSort.getSwaps();
            int prevComparisons = currentSort.getComparisons();
            // run next step of child sort and store results in the current bucket
            buckets[currentBucketIndex] = currentSort.Run().ToList();
            swapCount += currentSort.getSwaps() - prevSwaps;
            comparisonCount += currentSort.getComparisons() - prevComparisons;
        }

        protected virtual Sort beginNewSort(int bucketIndex, double[] bucket)
        {
            double minBucketValue = minArrValue + bucketIndex * rangeOfBucket;
            double maxBucketValue = minBucketValue + rangeOfBucket;

            if (bucket.Length < 2)
                return null;

            if (bucket.Length <= INSERT_BUCKET_THRESHOLD)
                return new InsertionSort(bucket);

            return new BucketSort(bucket, minBucketValue, maxBucketValue);
        }

        private double[] concatBuckets()
        {
            double[] concat = new double[array.Length];
            int index = 0;

            foreach (List<double> bucket in buckets)
            {
                for (int i = 0; i < bucket.Count; i++)
                {
                    concat[index] = bucket[i];
                    index++;
                }
            }

            return concat;
        }
    }
}
