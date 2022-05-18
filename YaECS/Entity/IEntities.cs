using System.Collections.Generic;

namespace YaEcs
{
    public interface IEntities : IEnumerable<Entity>
    {
        Entity Singleton { get; }
        Entity Create();
        bool Delete(uint id);
    }
}