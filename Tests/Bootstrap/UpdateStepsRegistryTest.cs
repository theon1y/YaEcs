using System;
using System.Linq;
using Xunit;
using YaEcs;

namespace Tests
{
    public class UpdateStepsRegistryTest
    {
        [Fact]
        public void ShouldInitialize()
        {
            var registry = new UpdateStepRegistry(Enumerable.Empty<UpdateStep>());
            Assert.Empty(registry.UpdateSteps);
        }

        [Fact]
        public void ShouldSort()
        {
            var steps = new UpdateStep[] { new("2", 2), new("1", 1) };
            var registry = new UpdateStepRegistry(steps);
            var sortedSteps = steps.OrderBy(x => x.Priority).ToList();
            Assert.Equal(registry.UpdateSteps, sortedSteps);
        }

        [Fact]
        public void ShouldNotAddStepWithTheSameNameTwice()
        {
            var steps = new UpdateStep[] { new("2", 2), new("2", 1) };
            Assert.Throws<ArgumentException>(() => new UpdateStepRegistry(steps));
        }

        [Fact]
        public void ShouldNotAddStepWithTheSamePriorityTwice()
        {
            var steps = new UpdateStep[] { new("2", 1), new("1", 1) };
            Assert.Throws<ArgumentException>(() => new UpdateStepRegistry(steps));
        }
    }
}