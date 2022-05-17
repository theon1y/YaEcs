namespace YaEcs
{
    public interface IUpdateSystem : ISystem
    {
        UpdateStep UpdateStep { get; }
    }
}