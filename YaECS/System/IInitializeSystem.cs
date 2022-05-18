namespace YaEcs
{
    public interface IInitializeSystem : IAsyncSystem
    {
        int Priority { get; }
    }
}