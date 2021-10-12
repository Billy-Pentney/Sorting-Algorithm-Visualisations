# Sorting-Algorithm-Visualisations

A simple WPF C# App which demonstrates a variety of common sorting algorithms used to sort a series of bars. 
Its primary purpose is for understanding the function and relative efficiency of each sort and to help highlight the advantages and disadvantages of each algorithm.

Current Algorithms:
  - Bubble Sorts
    - Standard Bubble Sort
    - Cocktail Shaker Sort
    - Comb Sort
  - Insertion Sorts
    - Standard Insertion Sort
    - Binary Insertion
  - Selection Sorts
    - Selection Sort
    - Double Selection Sort
    - Shell Sort
  - Quick Sort
    - w/ Lomuto Partitioning
    - w/ Hoare Partitioning
    - w/ Lomuto Median Partitioning
  - Other Sorts
    - Merge Sort
    - Cycle Sort
    - Patience Sort
  
The program also includes the ability to change the data distribution function, which further demonstrates the strengths and weaknesses of each algorithm.
Example:
    Insertion Sorts typically perform worse with data sets containing a high number of "small" values, since on each pass, a bar is more likely to be further from its correct position.

Currently, the program can sort on:
  - Randomly-generated values 
  - Linearly Interpolated values (constant difference between terms)
  - Exponential values (i.e. Negatively Skew, producing more "small" values)
  - Logarithmic values (i.e. Positively Skew, producing more "large" values)