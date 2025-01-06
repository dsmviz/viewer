

using Dsmviz.Interfaces.Application.Algorithms;
using Dsmviz.Interfaces.Application.Matrix;
using Dsmviz.Interfaces.Data.Entities;

namespace Dsmviz.Viewer.Application.Editing.Data
{
    public class ElementWeightsMatrix : ICloneable, IElementWeightMatrix
    {
        readonly int[,] _weights;
        readonly int _size;

        public ElementWeightsMatrix(int size)
        {
            _size = size;
            _weights = new int[_size, _size];
        }

        public ElementWeightsMatrix(IDependencyWeightMatrix matrix, IElement element)
        {
            IList<IElement> children = element.Children;

            _size = children.Count;
            _weights = new int[_size, _size];

            for (int i = 0; i < children.Count; i++)
            {
                IElement provider = children[i];

                for (int j = 0; j < children.Count; j++)
                {
                    if (j != i)
                    {
                        IElement consumer = children[j];

                        int weight = matrix.GetDerivedDependencyWeight(consumer, provider);

                        SetWeight(i, j, weight);
                    }
                }
            }
        }

        public object Clone()
        {
            ElementWeightsMatrix sm = new ElementWeightsMatrix(Size);

            for (int row = 0; row < Size; row++)
            {
                for (int column = 0; column < Size; column++)
                {
                    sm.SetWeight(row, column, GetWeight(row, column));
                }
            }

            return sm;
        }

        public int Size => _size;

        public void SetWeight(int row, int column, int weight)
        {
            CheckRowIndex(row);
            CheckColumnIndex(column);

            _weights[row, column] = weight;
        }

        public int GetWeight(int row, int column)
        {
            CheckRowIndex(row);
            CheckColumnIndex(column);

            return _weights[row, column];
        }

        private void CheckRowIndex(int row)
        {
            if (row < 0 || row >= _size)
            {
                throw new ArgumentOutOfRangeException(nameof(row));
            }
        }

        private void CheckColumnIndex(int column)
        {
            if (column < 0 || column >= _size)
            {
                throw new ArgumentOutOfRangeException(nameof(column));
            }
        }
    }
}
