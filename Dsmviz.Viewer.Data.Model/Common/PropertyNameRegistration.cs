using System.Diagnostics;

namespace Dsmviz.Viewer.Data.Model.Common
{
    /// <summary>
    /// Collection of property names. There can be max 256 different names.
    /// Introduced to reduce memory usage by avoiding storing type string multiple times in memory.
    /// Default only the no property name is registered.
    /// </summary>
    public class PropertyNameRegistration
    {
        private readonly Dictionary<char, string> _registeredPropertyNames = new();
        private readonly Dictionary<string, char> _propertyNameIds = new();

        public void Clear()
        {
            _registeredPropertyNames.Clear();
            _propertyNameIds.Clear();
        }

        public char RegisterName(string propertyName)
        {
            Debug.Assert(_registeredPropertyNames.Count < 255);
            if (_propertyNameIds.TryGetValue(propertyName, out var name))
            {
                return name;
            }
            else
            {
                var id = (char)_registeredPropertyNames.Count;
                _registeredPropertyNames[id] = propertyName;
                _propertyNameIds[propertyName] = id;
                return id;
            }
        }

        public string GetRegisteredName(char propertyNameId)
        {
            return _registeredPropertyNames.GetValueOrDefault(propertyNameId, "");
        }

        public IEnumerable<string> GetRegisteredNames()
        {
            return _registeredPropertyNames.Values;
        }
    }
}
