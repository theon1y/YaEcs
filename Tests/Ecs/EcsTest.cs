using System;
using System.Linq;
using System.Numerics;
using Xunit;
using YaEcs;

namespace Tests.Ecs
{
    public class EcsTest
    {
        private static readonly UpdateStep TestUpdateStep = new("Test", 1);
        private static readonly Vector3 Vector123 = new(1, 2, 3);
        private readonly CreateEntitiesSystem[] initializeSystems = { new() };
        private readonly MoveSystem[] updateSystems = { new() };
        private readonly DeleteSystem[] disposeSystems = { new() };
        private readonly UpdateStepRegistry updateStepRegistry = new(new[] { TestUpdateStep });
        
        [Fact]
        public void ShouldCreateWorld()
        {
            var world = new YaEcs.World(initializeSystems, updateSystems, disposeSystems, updateStepRegistry);
            Assert.Equal(5, world.Entities.Count());

            var initializeSystem = initializeSystems[0];
            AssertComponent<TransformComponent>(world, initializeSystem.transformEntity,
                transform => transform.Position == new Vector3(1, 2 ,3));
        }

        [Fact]
        public void ShouldMoveEntities()
        {
            var world = new YaEcs.World(initializeSystems, updateSystems, disposeSystems, updateStepRegistry);
            var initializeSystem = initializeSystems[0];
            
            AssertComponent<TransformComponent>(world, initializeSystem.transformEntity,
                transform => transform.Position == Vector123);
            AssertComponent<TransformComponent>(world, initializeSystem.moveEntityUp,
                transform => transform.Position == Vector3.Zero);
            AssertComponent<TransformComponent>(world, initializeSystem.moveEntityRight,
                transform => transform.Position == Vector3.Zero);
            
            world.Update();
            
            AssertComponent<TransformComponent>(world, initializeSystem.transformEntity,
                transform => transform.Position == Vector123);
            AssertComponent<TransformComponent>(world, initializeSystem.moveEntityUp,
                transform => transform.Position == Vector3.UnitY);
            AssertComponent<TransformComponent>(world, initializeSystem.moveEntityRight,
                transform => transform.Position == Vector3.UnitX);
            
            world.Update();
            
            AssertComponent<TransformComponent>(world, initializeSystem.transformEntity,
                transform => transform.Position == Vector123);
            AssertComponent<TransformComponent>(world, initializeSystem.moveEntityUp,
                transform => transform.Position == 2 * Vector3.UnitY);
            AssertComponent<TransformComponent>(world, initializeSystem.moveEntityRight,
                transform => transform.Position == 2* Vector3.UnitX);
        }

        [Fact]
        public void ShouldDeleteEntities()
        {
            var world = new YaEcs.World(initializeSystems, updateSystems, disposeSystems, updateStepRegistry);
            world.Update();
            world.Dispose();
            Assert.Empty(world.Entities);
            Assert.Empty(world.Components);
        }

        private static void AssertComponent<TComponent>(YaEcs.World world, YaEcs.Entity entity,
            Func<TComponent, bool> predicate) where TComponent : IComponent
        {
            Assert.True(world.TryGetComponent<TComponent>(entity, out var component));
            Assert.True(predicate(component));
        }

        private class TransformComponent : IComponent
        {
            public Vector3 Position;
        }

        private class MoveDirectionComponent : IComponent
        {
            public Vector3 Direction;
        }

        private class CreateEntitiesSystem : IInitializeSystem
        {
            public YaEcs.Entity emptyEntity;
            public YaEcs.Entity transformEntity;
            public YaEcs.Entity stillEntity;
            public YaEcs.Entity moveEntityUp;
            public YaEcs.Entity moveEntityRight;
            
            public void Execute(YaEcs.World world)
            {
                emptyEntity = world.Create();

                transformEntity = world.Create(new TransformComponent { Position = Vector123 });

                stillEntity = world.Create(new MoveDirectionComponent());

                moveEntityUp = world.Create(new TransformComponent { Position = Vector3.Zero },
                    new MoveDirectionComponent { Direction = Vector3.UnitY });

                moveEntityRight = world.Create(new TransformComponent { Position = Vector3.Zero },
                    new MoveDirectionComponent { Direction = Vector3.UnitX });
            }
        }

        private class MoveSystem : IUpdateSystem
        {
            public UpdateStep UpdateStep => TestUpdateStep;
            
            public void Execute(YaEcs.World world)
            {
                world.ForEach<TransformComponent, MoveDirectionComponent>((_, transform, move) =>
                {
                    transform.Position += move.Direction;
                });
            }
        }

        private class DeleteSystem : IDisposeSystem
        {
            public void Execute(YaEcs.World world)
            {
                foreach (var entity in world.Entities.ToList())
                {
                    world.DeleteEntity(entity);
                }
            }
        }
    }
}