using System.Threading.Tasks;

namespace YaEcs
{
    public interface IAsyncSystem
    {
        Task ExecuteAsync(IWorld world);
    }
}