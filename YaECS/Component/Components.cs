using System;
using System.Collections;
using System.Collections.Generic;

namespace YaEcs
{
    public partial class Components : IComponents
    {
        private readonly Dictionary<Type, Dictionary<uint, IComponent>> storage = new();

        public bool Add(uint entity, Type type, IComponent component)
        {
            if (!storage.TryGetValue(type, out var components))
            {
                components = new Dictionary<uint, IComponent>();
                storage[type] = components;
            }
            if (components.ContainsKey(entity)) return false;

            components[entity] = component;
            return true;
        }

        public bool TryGet(uint entity, Type type, out IComponent component)
        {
            component = default;
            if (!storage.TryGetValue(type, out var components)) return false;

            return components.TryGetValue(entity, out component);
        }

        public bool Remove(uint entity, Type type)
        {
            if (!storage.TryGetValue(type, out var components)) return false;

            return components.Remove(entity);
        }

        public void DeleteAll(uint entity)
        {
            foreach (var components in storage.Values)
            {
                components.Remove(entity);
            }
        }

        public IEnumerator<KeyValuePair<uint, IComponent>> GetEnumerator()
        {
            return new ComponentEnumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}