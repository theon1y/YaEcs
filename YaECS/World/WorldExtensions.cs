namespace YaEcs
{
    public static class WorldExtensions
    {
        public static Entity CreateEntity(this IWorld world)
        {
            return world.Entities.Create();
        }

        public static bool DeleteEntity(this IWorld world, uint id)
        {
            if (!world.Entities.Delete(id)) return false;

            world.Components.DeleteAll(id);
            return true;
        }
        
        public static bool AddComponent<T>(this IWorld world, uint entity, T component) where T : IComponent
        {
            var type = component.GetType();
            return world.Components.Add(entity, type, component);
        }

        public static bool TryGetComponent<T>(this IWorld world, uint entity, out T component) where T : IComponent
        {
            var type = typeof(T);
            component = default;
            if (!world.Components.TryGet(entity, type, out var instance)) return false;

            component = (T) instance;
            return true;
        }

        public static bool RemoveComponent<T>(this IWorld world, uint entity) where T : IComponent
        {
            var type = typeof(T);
            return world.Components.Remove(entity, type);
        }
        
        public static void AddSingleton<T1>(this IWorld world, T1 component)
            where T1 : IComponent
        {
            world.AddComponent(world.Entities.Singleton, component);
        }
        
        public static bool TryGetSingleton<T1>(this IWorld world, out T1 component)
            where T1 : IComponent
        {
            return world.TryGetComponent(world.Entities.Singleton, out component);
        }
    }
}