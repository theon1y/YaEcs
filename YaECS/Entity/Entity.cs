namespace YaEcs
{
    public readonly struct Entity
    {
        public readonly uint Id;

        public Entity(uint id)
        {
            Id = id;
        }

        public static implicit operator Entity(uint id) => new(id);

        public static implicit operator uint(Entity entity) => entity.Id;
    }
}