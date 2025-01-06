namespace Dsmviz.Viewer.Utils
{
    public class SparseArray<T>(T defaultValue)
        where T : IComparable
    {
        private readonly Dictionary<int, T> _values = new();

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
        public bool SetValue(int index, T item)
        {
            T initialValue = GetValue(index);

            if (item.CompareTo(defaultValue) == 0)
            {
                _values.Remove(index);
            }
            else
            {
                _values[index] = item;
            }

            return item.CompareTo(initialValue) != 0;
        }

        /// <summary>
        /// Clears value and returns true if value changed
        /// </summary>
        public bool ClearValue(int index)
        {
            T initialValue = GetValue(index);

            _values.Remove(index);

            return defaultValue.CompareTo(initialValue) != 0;
        }

        /// <summary>
        /// Gets value
        /// </summary>
        public T GetValue(int index)
        {
            return _values.GetValueOrDefault(index, defaultValue);
        }

        /// <summary>
        /// Number of non default values is the array
        /// </summary>
        public int FilledCount => _values.Count;
    }
}