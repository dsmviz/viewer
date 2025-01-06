
namespace Dsmviz.Viewer.Utils
{
    public class SparseMatrix<T>(T defaultValue)
        where T : IComparable
    {
        private readonly Dictionary<long, T> _values = new();

        /// <summary>
        /// Clears all values
        /// </summary>
        public void Clear()
        {
            _values.Clear();
        }

        /// <summary>
        /// Sets value and returns true if value changed
        /// </summary>
        public bool SetValue(int row, int column, T item)
        {
            long key = LongKey.Create(row, column);

            T initialValue = GetValueInternal(key);
            if (item.CompareTo(defaultValue) == 0)
            {
                _values.Remove(key);
            }
            else
            {
                _values[key] = item;
            }

            return item.CompareTo(initialValue) != 0;
        }

        /// <summary>
        /// Clears value and returns true if value changed
        /// </summary>
        public bool ClearValue(int row, int column)
        {
            long key = LongKey.Create(row, column);
            T initialValue = GetValueInternal(key);
            _values.Remove(key);
            return defaultValue.CompareTo(initialValue) != 0;
        }

        /// <summary>
        /// Gets value
        /// </summary>
        public T GetValue(int row, int column)
        {
            long key = LongKey.Create(row, column);
            return GetValueInternal(key);
        }

        /// <summary>
        /// Gets value using long key
        /// </summary>
        private T GetValueInternal(long key)
        {
            return _values.GetValueOrDefault(key, defaultValue);
        }

        /// <summary>
        /// Number of non default values is the matrix
        /// </summary>
        public int FilledCount => _values.Count;
    }
}
