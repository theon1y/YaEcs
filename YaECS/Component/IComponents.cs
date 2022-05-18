using System;
using System.Collections.Generic;

namespace YaEcs
{
    public interface IComponents : IEnumerable<KeyValuePair<uint, IComponent>>
    {
        bool Add(uint entity, Type type, IComponent component);
        bool TryGet(uint entity, Type type, out IComponent component);
        bool Remove(uint entity, Type type);
        void DeleteAll(uint entity);
    }
}