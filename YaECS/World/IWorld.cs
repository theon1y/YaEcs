using System;

namespace YaEcs
{
    public interface IWorld : IDisposable
    {
        IComponents Components { get; }
        IEntities Entities { get; }
        void Initialize();
        void Update();
    }
}