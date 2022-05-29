namespace YaEcs
{
    public static class WorldCreateExtensions
    {
        public static Entity Create(this IWorld world)
        {
            return world.CreateEntity();
        }
        
        public static Entity Create<T1>(this IWorld world, T1 component)
            where T1 : IComponent
        {
            var entity = world.CreateEntity();
            world.AddComponent(entity, component);
            return entity;
        }
        
        public static Entity Create<T1, T2>(this IWorld world, T1 component1, T2 component2)
            where T1 : IComponent
            where T2 : IComponent
        {
            var entity = world.CreateEntity();
            world.AddComponent(entity, component1);
            world.AddComponent(entity, component2);
            return entity;
        }
        
        public static Entity Create<T1, T2, T3>(this IWorld world, T1 component1, T2 component2, T3 component3)
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
        {
            var entity = world.CreateEntity();
            world.AddComponent(entity, component1);
            world.AddComponent(entity, component2);
            world.AddComponent(entity, component3);
            return entity;
        }
        
        public static Entity Create<T1, T2, T3, T4>(this IWorld world, T1 component1, T2 component2, T3 component3, T4 component4)
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
        {
            var entity = world.CreateEntity();
            world.AddComponent(entity, component1);
            world.AddComponent(entity, component2);
            world.AddComponent(entity, component3);
            world.AddComponent(entity, component4);
            return entity;
        }
    }
}