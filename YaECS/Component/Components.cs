using System;
using System.Collections;
using System.Collections.Generic;

namespace YaEcs
{
    public class Components : IEnumerable<KeyValuePair<uint, IComponent>>
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
            return new ComponentsEnumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        
        private struct ComponentsEnumerator : IEnumerator<KeyValuePair<uint, IComponent>>
        {
            private readonly Components source;
            private IEnumerator<KeyValuePair<Type, Dictionary<uint, IComponent>>> typeEnumerator;
            private IEnumerator<KeyValuePair<uint, IComponent>>? componentEnumerator;
            
            public ComponentsEnumerator(Components components)
            {
                source = components;
                Current = default;
                componentEnumerator = default;
                typeEnumerator = source.storage.GetEnumerator();;
            }
            
            public bool MoveNext()
            {
                if (componentEnumerator != null && componentEnumerator.MoveNext())
                {
                    Current = componentEnumerator.Current;
                    return true;
                }
                
                if (typeEnumerator.MoveNext())
                {
                    componentEnumerator = typeEnumerator.Current.Value.GetEnumerator();
                    if (componentEnumerator.MoveNext())
                    {
                        Current = componentEnumerator.Current;
                        return true;
                    }
                }

                return false;
            }

            public void Reset()
            {
                Current = default;
                componentEnumerator = default;
                typeEnumerator.Dispose();
                typeEnumerator = source.storage.GetEnumerator();
            }

            public KeyValuePair<uint, IComponent> Current { get; private set; }

            object IEnumerator.Current => Current;

            public void Dispose()
            {
                typeEnumerator.Dispose();
                componentEnumerator?.Dispose();
            }
        }
    }
}