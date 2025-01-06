namespace Dsmviz.Viewer.Utils
{
    public class IntSparseMatrix() : SparseMatrix<int>(0)
    {
        public int AddValue(int row, int column, int delta)
        {
            int initialValue = GetValue(row, column);
            int updatedValue = initialValue + delta;
            SetValue(row, column, updatedValue);
            return updatedValue;
        }

        public int SubtractValue(int row, int column, int delta)
        {
            int initialValue = GetValue(row, column);
            int updatedValue = initialValue - delta;
            SetValue(row, column, updatedValue);
            return updatedValue;
        }
    }
}
