using System.Diagnostics;

namespace Dsmviz.Viewer.Data.Model.Common
{
    /// <summary>
    /// Collection of relation or element types. There can be max 256 different types.
    /// Introduced to reduce memory usage by avoiding storing type string multiple times in memory.
    /// Default only the empty type name is registered.
    /// </summary>
    public class TypeNameRegistration
    {
        private readonly Dictionary<char, string> _registeredTypeNames = new();
        private readonly Dictionary<string, char> _typeNameIds = new();

        public TypeNameRegistration()
        {
            var id = RegisterName("");
            Debug.Assert(id == 0);
        }

        public void Clear()
        {
            _registeredTypeNames.Clear();
            _typeNameIds.Clear();

            var id = RegisterName("");
            Debug.Assert(id == 0);
        }

        public char RegisterName(string typeName)
        {
            Debug.Assert(_registeredTypeNames.Count < 255);
            if (_typeNameIds.TryGetValue(typeName, out var name))
            {
                return name;
            }
            else
            {
                var id = (char)_registeredTypeNames.Count;
                _registeredTypeNames[id] = typeName;
                _typeNameIds[typeName] = id;
                return id;
            }
        }

        public string GetRegisteredName(char typeNameId)
        {
            return _registeredTypeNames.GetValueOrDefault(typeNameId, "");
        }

        public IEnumerable<string> GetRegisteredNames()
        {
            return _registeredTypeNames.Values;
        }
    }
}