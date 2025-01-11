using Dsmviz.Interfaces.Application.Algorithms;
using Dsmviz.Viewer.Application.Algorithms.Common;

namespace Dsmviz.Viewer.Application.Algorithms.Partition
{
    /// <summary>
    /// This class is responsible for the partitioning calculation which for a given square matrix orders the rows
    /// as close as possible to the lower block triangular form.  The idea being that as many empty cells are pushed
    /// into the upper triangle as possible.  It is probable that some cells will be non-zero - in this
    /// case they are pushed towards the bottom right corner
    /// </summary>
    /// <remarks>
    /// <para>
    /// The algorithm first pushes empty rows to the top and complete rows to the bottom.  Partially complete rows
    /// are then processed using a distance score to push them below but as close as possible to the diagonal</para>
    /// <para>The result of the calculation is contained in the vector class which specifies how the new order of row indexes</para>
    /// </remarks>
    class PartitioningCalculation
    {
        readonly IElementWeightMatrix _dependencyWeights;

        /// <summary>
        /// Constructor of calculation on a given n * n matrix
        /// </summary>
        public PartitioningCalculation(IElementWeightMatrix dependencyWeights)
        {
            _dependencyWeights = dependencyWeights;
        }

        /// <summary>
        /// Run the partition calculation
        /// </summary>
        /// <returns>The result in the form of a vector</returns>
        public SortResult Partition()
        {
            SortResult sortResult = new SortResult(_dependencyWeights.Size);

            DoPartitioning(sortResult);

            return sortResult;
        }

        /// <summary>
        /// the main partitioning algorithm.
        /// </summary>
        /// <param name="sortResult"></param>
        void DoPartitioning(SortResult sortResult)
        {
            // Move all empty rows to the top - save the index of the first non-empty row (start)
            int start = MoveZeroRows(sortResult);

            // Move all the complete rows to the bottom - save the index of the first full row (end)
            int end = MoveFullRows(sortResult, start);

            // For the remaining rows between start and end move to the right empty columns
            end = MoveZeroColumns(sortResult, start, end);

            // Sort the remaining partially complete rows
            ToBlockTriangular(sortResult, start, end);
        }

        int MoveZeroRows(SortResult sortResult)
        {
            int nextSwapRow = 0;

            // bubble zero rows to the top - starting from the bottom - to leave any rows already moved
            // by the user stay in place

            int i = sortResult.GetNumberOfElements() - 1;
            while (i >= nextSwapRow)
            {
                var allZero = true;
                for (int j = 0; j < sortResult.GetNumberOfElements() && allZero; j++)
                {
                    if (i != j)  // the diagonal must obviously be ignored
                    {
                        if (TrueMatrixValue(sortResult, i, j) != 0)
                        {
                            allZero = false;
                        }
                    }
                }

                if (allZero) // swap indexes
                {
                    sortResult.Swap(nextSwapRow, i);
                    nextSwapRow++; // points to next swap position

                    // don't decrement i as it has not yet been tested - we've changed the order remember !!
                }
                else
                {
                    i--; // next row
                }
            }

            return nextSwapRow;
        }

        int MoveFullRows(SortResult sortResult, int start)
        {
            int nextSwapRow = sortResult.GetNumberOfElements() - 1;

            // bubble complete rows to the bottom starting 'start'
            int i = start;

            while (i <= nextSwapRow)
            {
                var allNonZero = true;
                for (int j = 0; j < sortResult.GetNumberOfElements() && allNonZero; j++)
                {
                    if (i != j)
                    {
                        if (TrueMatrixValue(sortResult, i, j) == 0)
                        {
                            allNonZero = false;
                        }
                    }
                }

                if (allNonZero) // swap indexes
                {
                    sortResult.Swap(nextSwapRow, i);
                    nextSwapRow--;
                }
                else
                {
                    i++;
                }
            }

            return nextSwapRow;
        }

        int MoveZeroColumns(SortResult sortResult, int start, int end)
        {
            int j = start;
            int nextSwap = end; //vector.TotalElementCount - 1;

            while (j <= nextSwap)
            {
                var allZeros = true;
                for (int i = 0; i < sortResult.GetNumberOfElements() && allZeros; i++)
                {
                    if (i != j)
                    {
                        if (TrueMatrixValue(sortResult, i, j) != 0)
                        {
                            allZeros = false;
                        }
                    }
                }

                if (allZeros)  // swap indexes
                {
                    sortResult.Swap(nextSwap, j);
                    nextSwap--;

                    // stay on new column in position j
                }
                else
                {
                    j++;
                }
            }

            return nextSwap;
        }

        /// <summary>
        /// Get the dependency weight from the original square matrix given the current vector and i,j indexes
        /// </summary>
        /// <param name="sortResult"></param>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <returns></returns>
        int TrueMatrixValue(SortResult sortResult, int i, int j)
        {
            int weight = _dependencyWeights.GetWeight(sortResult.GetIndex(i), sortResult.GetIndex(j));
            return weight > 0 ? 1 : 0;
        }

        void ToBlockTriangular(SortResult sortResult, int start, int end)
        {
            bool doLoop;

            long currentScore = Score(sortResult);

            // For holding Permutations already examined during one iteration of the outer while loop
            IDictionary<Permutation, object?> permMap =
                new Dictionary<Permutation, object?>(sortResult.GetNumberOfElements() * sortResult.GetNumberOfElements() / 2);
            do
            {
                doLoop = false;

                for (int rowIndex = start; rowIndex <= end; rowIndex++)
                {
                    for (int columnIndex = end; columnIndex > rowIndex; columnIndex--)
                    {
                        if (TrueMatrixValue(sortResult, rowIndex, columnIndex) != 0) // not zero so we want to possibly move it
                        {
                            // now find first zero from the left hand side

                            for (int x = start; x <= end; x++) // here x represents the line index
                            {
                                for (int y = start; y <= end; y++)
                                {
                                    if (x != y)  // ignore the diagonal
                                    {
                                        if (TrueMatrixValue(sortResult, x, y) == 0)
                                        {
                                            Permutation p = new Permutation(columnIndex, y);

                                            if (permMap.TryAdd(p, null))
                                            {
                                                //check score of potential new vector
                                                sortResult.Swap(columnIndex, y);

                                                var newScore = Score(sortResult);

                                                if (newScore > currentScore)
                                                {
                                                    currentScore = newScore;
                                                    doLoop = true; // increasing score so continue
                                                }
                                                else
                                                {
                                                    sortResult.Swap(columnIndex, y); //swap back to original 
                                                }
                                            }
                                            // else permutation already used
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                permMap.Clear();
            }
            while (doLoop);
        }

        long Score(SortResult sortResult)
        {
            // We're trying to maximize the number of empty cells in the upper triangle
            // filled in cells are pushed down - for a set of two orderings the one with the higher
            // score should be the most preferable


            // calculate score for cells in upper triangle (TODO possible optimization between start and end)
            long score = 0;

            for (int i = 0; i < sortResult.GetNumberOfElements() - 1; i++)
            {
                for (int j = i + 1; j < sortResult.GetNumberOfElements(); j++)
                {
                    if (TrueMatrixValue(sortResult, i, j) == 0)
                    {
                        score += CellScore(i, j, sortResult.GetNumberOfElements());
                    }
                }
            }

            return score;
        }

        static long CellScore(int i, int j, int size)
        {
            // a measure of distance from bottom right
            int a = size - i;
            int b = j + 1;

            return a * a * b * b;
        }
    }
}
