namespace YaEcs
{
    public interface IDisposeSystem : IAsyncSystem
    {
        int Priority { get; }
    }
}