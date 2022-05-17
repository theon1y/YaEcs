using System;
using Xunit;
using YaEcs;

namespace Tests.Component
{
    public abstract class ComponentsTestBase<TComponent>
        where TComponent : IComponent, ComponentsTestBase<TComponent>.IIntValue, new()
    {
        protected readonly Components Components = new();
        
        private readonly Type type = typeof(TComponent);
        private const uint Entity = 1;

        [Fact]
        public void ShouldNotAddTwice()
        {
            Assert.True(Components.Add(Entity, type, new TComponent()));
            Assert.False(Components.Add(Entity, type, new TComponent()));
        }

        [Fact]
        public void ShouldAddOtherEntityComponent()
        {
            var instance = new TComponent();
            Components.Add(Entity, type, instance);

            var entity2 = Entity + 1;
            var instance2 = new TComponent();
            Assert.True(Components.Add(entity2, type, instance2));
        }

        [Fact]
        public void ShouldGetComponent()
        {
            var instance = new TComponent();
            Components.Add(Entity, type, instance);
            Components.TryGet(Entity, type, out var component);
            Assert.Equal(instance, component);
        }

        [Fact]
        public void ShouldNotGetIfDoesNotExist()
        {
            Assert.False(Components.TryGet(Entity, type, out var component));
        }

        [Fact]
        public void ShouldNotBeEqualToOtherEntityComponent()
        {
            var instance = new TComponent();
            Components.Add(Entity, type, instance);

            var entity2 = Entity + 1;
            var instance2 = new TComponent();
            Components.Add(entity2, type, instance2);
            Components.TryGet(entity2, type, out var component);
            Assert.NotEqual(instance, component);
        }

        [Fact]
        public void ShouldNotGetOtherEntityComponent()
        {
            var instance = new TComponent();
            Components.Add(Entity, type, instance);
            Assert.False(Components.TryGet(Entity + 1, type, out var component));
        }

        [Fact]
        public void ShouldNotRemoveTwice()
        {
            Components.Add(Entity, type, new TComponent());
            Assert.True(Components.Remove(Entity, type));
            Assert.False(Components.Remove(Entity, type));
        }

        [Fact]
        public void ShouldNotRemoveOtherEntityComponent()
        {
            var instance = new TComponent();
            Components.Add(Entity, type, instance);

            var entity2 = Entity + 1;
            var instance2 = new TComponent();
            Components.Add(entity2, type, instance2);
            
            Components.Remove(Entity, type);
            Components.TryGet(entity2, type, out var component);
            Assert.Equal(instance2, component);
        }
        
        [Fact]
        public void ShouldChangeValue()
        {
            var instance = new TComponent { Value = 1337 };
            Components.Add(Entity, type, instance);

            Components.TryGet(Entity, type, out var component);
            (component as IIntValue).Value = 1;
            
            Components.TryGet(Entity, type, out component);
            Assert.Equal(1, (component as IIntValue).Value);
        }
        
        public interface IIntValue
        {
            int Value { get; set; }
        }
    }
}