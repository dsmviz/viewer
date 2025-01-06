namespace Dsmviz.Viewer.Utils
{
    public class IntSparseArray() : SparseArray<int>(0)
    {
        public int IncrementValue(int index)
        {
            int initialValue = GetValue(index);
            int updatedValue = initialValue + 1;
            SetValue(index, updatedValue);
            return updatedValue;
        }

        public int DecrementValue(int index)
        {
            int initialValue = GetValue(index);
            int updatedValue = initialValue - 1;
            SetValue(index, updatedValue);
            return updatedValue;
        }
    }
}
