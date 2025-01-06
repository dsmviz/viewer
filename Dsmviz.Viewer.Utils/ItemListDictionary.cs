namespace Dsmviz.Viewer.Utils
{
    public class ItemListDictionary<TK, T> where TK : notnull
    {
        private readonly Dictionary<TK, List<T>> _itemLists = new();
        private readonly List<T> _emptyList = [];

        public void Clear()
        {
            _itemLists.Clear();
        }

        public void AddItem(TK key, T item)
        {
            if (!_itemLists.ContainsKey(key))
            {
                _itemLists[key] = [];
            }
            _itemLists[key].Add(item);
        }

        public void RemoveItem(TK key, T item)
        {
            if (_itemLists.ContainsKey(key))
            {
                _itemLists[key].Remove(item);

                if (_itemLists[key].Count == 0)
                {
                    _itemLists.Remove(key);
                }
            }
        }

        public void RemoveItems(TK key)
        {
            if (_itemLists.ContainsKey(key))
            {
                _itemLists.Remove(key);
            }
        }

        public IEnumerable<T> GetItems(TK key)
        {
            return _itemLists.GetValueOrDefault(key, _emptyList);
        }

        public int GetItemCount(TK key)
        {
            if (_itemLists.TryGetValue(key, out var list))
            {
                return list.Count;
            }
            else
            {
                return 0;
            }
        }

        public int GetTotalItemCount()
        {
            int count = 0;

            foreach (List<T> items in _itemLists.Values)
            {
                count += items.Count;
            }

            return count;
        }


    }
}
