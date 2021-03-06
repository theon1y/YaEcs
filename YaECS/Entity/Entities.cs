using System.Collections;
using System.Collections.Generic;

namespace YaEcs
{
    public class Entities : IEntities
    {
        private readonly HashSet<Entity> storage = new();
        private uint lastId;
        
        public Entity Singleton { get; } = new(0);

        public Entity Create()
        {
            ++lastId;
            var entity = new Entity(lastId);
            storage.Add(entity);
            return entity;
        }

        public bool Delete(uint id)
        {
            return storage.Remove(new Entity(id));
        }

        public IEnumerator<Entity> GetEnumerator()
        {
            return storage.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}