using System;
using System.Threading.Tasks;

namespace YaEcs
{
    public interface IWorld : IAsyncDisposable
    {
        IComponents Components { get; }
        IEntities Entities { get; }
        Task InitializeAsync();
        void Update();
    }
}