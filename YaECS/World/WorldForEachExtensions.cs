using System;
using System.Runtime.CompilerServices;

namespace YaEcs
{
    public static class WorldForEachExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ForEach<T1>(this IWorld world, Action<Entity, T1> action)
            where T1 : IComponent
        {
            foreach (var entity in world.Entities)
            {
                if (world.TryGetComponent<T1>(entity, out var component))
                {
                    action(entity, component);
                }
            }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ForEach<T1, T2>(this IWorld world, Action<Entity, T1, T2> action)
            where T1 : IComponent
            where T2 : IComponent
        {
            foreach (var entity in world.Entities)
            {
                if (!world.TryGetComponent<T1>(entity, out var component1)) continue;
                if (world.TryGetComponent<T2>(entity, out var component2))
                {
                    action(entity, component1, component2);
                }
            }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ForEach<T1, T2, T3>(this IWorld world, Action<Entity, T1, T2, T3> action)
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
        {
            foreach (var entity in world.Entities)
            {
                if (!world.TryGetComponent<T1>(entity, out var component1)) continue;
                if (!world.TryGetComponent<T2>(entity, out var component2)) continue;
                if (world.TryGetComponent<T3>(entity, out var component3))
                {
                    action(entity, component1, component2, component3);
                }
            }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ForEach<T1, T2, T3, T4>(this IWorld world, Action<Entity, T1, T2, T3, T4> action)
            where T1 : IComponent
            where T2 : IComponent
            where T3 : IComponent
            where T4 : IComponent
        {
            foreach (var entity in world.Entities)
            {
                if (!world.TryGetComponent<T1>(entity, out var component1)) continue;
                if (!world.TryGetComponent<T2>(entity, out var component2)) continue;
                if (!world.TryGetComponent<T3>(entity, out var component3)) continue;
                if (world.TryGetComponent<T4>(entity, out var component4))
                {
                    action(entity, component1, component2, component3, component4);
                }
            }
        }
    }
}