namespace Dsmviz.Interfaces.Application.Algorithms
{
    public interface IElementWeightMatrix
    {
        /// <summary>
        /// The size of the matrix as determined by the number of children of the element for which this matrix was created.
        /// </summary>
        int Size { get; }
        /// <summary>
        /// The weight value of a selected cell in the matrix.
        /// </summary>
        int GetWeight(int i, int j);
    }
}
