using System.Linq;
using Xunit;
using YaEcs;

namespace Tests.Entity
{
    public class EntitiesTest
    {
        private readonly Entities entities = new();
        
        [Fact]
        public void ShouldCreateEntity()
        {
            var entity = entities.Create();
            Assert.NotEqual<uint>(0, entity.Id);
        }

        [Fact]
        public void ShouldCreateSequentially()
        {
            for (uint i = 1; i <= 10; ++i)
            {
                var entity = entities.Create();
                Assert.Equal(i, entity.Id);
            }
        }

        [Fact]
        public void ShouldIterate()
        {
            var createdEntities = Enumerable.Range(0, 10)
                .Select(_ => entities.Create())
                .ToHashSet();
            Assert.Equal(createdEntities.Count, entities.Count());

            var notIteratedEntities = createdEntities;
            notIteratedEntities.ExceptWith(entities);
            Assert.Empty(notIteratedEntities);
        }

        [Fact]
        public void ShouldNotRemoveTwice()
        {
            var entity = entities.Create();
            Assert.True(entities.Delete(entity));
            Assert.False(entities.Delete(entity));
        }

        [Fact]
        public void ShouldRemove()
        {
            var createdEntities = Enumerable.Range(0, 10)
                .Select(_ => entities.Create())
                .ToHashSet();
            var removedEntity = createdEntities.First();
            entities.Delete(removedEntity);
            
            Assert.Equal(createdEntities.Count - 1, entities.Count());
            
            var notIteratedEntities = createdEntities;
            notIteratedEntities.ExceptWith(entities);

            Assert.Single(notIteratedEntities);
            Assert.Equal(notIteratedEntities.First(), removedEntity);
        }

        [Fact]
        public void ShouldRemoveAll()
        {
            var createdEntities = Enumerable.Range(0, 2)
                .Select(_ => entities.Create())
                .ToHashSet();
            foreach (var entity in createdEntities)
            {
                entities.Delete(entity);
            }

            Assert.Empty(entities);
        }
    }
}