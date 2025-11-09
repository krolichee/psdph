using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psdPH.Serialization
{
    public class IdentityMap
    {
        private readonly Dictionary<object, Guid> _objectToId = new Dictionary<object, Guid>();
        private readonly Dictionary<Guid, object> _idToObject = new Dictionary<Guid, object>();

        public Guid GetOrCreateId(object obj)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));

            if (_objectToId.TryGetValue(obj, out var id))
                return id;

            id = Guid.NewGuid();
            _objectToId[obj] = id;
            _idToObject[id] = obj;
            return id;
        }

        public bool TryGetId(object obj, out Guid id)
        {
            if (obj == null)
            {
                id = default;
                return false;
            }

            return _objectToId.TryGetValue(obj, out id);
        }

        public bool TryGetObject(Guid id, out object obj)
        {
            return _idToObject.TryGetValue(id, out obj);
        }

        public Guid GetId(object obj)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));

            if (_objectToId.TryGetValue(obj, out var id))
                return id;

            throw new KeyNotFoundException($"Object of type {obj.GetType().Name} not found in context");
        }

        public object GetObject(Guid id)
        {
            if (_idToObject.TryGetValue(id, out var obj))
                return obj;

            throw new KeyNotFoundException($"Object with ID {id} not found in context");
        }

        // Для десериализации - явное добавление связи
        public void AddMapping(object obj, Guid id)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));

            if (_idToObject.ContainsKey(id))
                throw new InvalidOperationException($"ID {id} is already mapped to another object");

            _objectToId[obj] = id;
            _idToObject[id] = obj;
        }

        public void Clear()
        {
            _objectToId.Clear();
            _idToObject.Clear();
        }

        public int Count => _objectToId.Count;

        public IdentityMap(Identity[] identities)
        {
            foreach (var item in identities)
            {
                var id = item.Guid;
                var obj = item.Entity;
                _objectToId[obj] = id;
                _idToObject[id] = obj;
            }
        }

        public IdentityMap()
        {
        }

        public object[] Objects => _idToObject.Values.ToArray();
        public Guid[] Ids => _objectToId.Values.ToArray();
    }
}

