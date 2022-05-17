﻿using System.Collections.Generic;
using System.Linq;
using Xunit;
using YaEcs;

namespace Tests.World
{
    public class WorldTest
    {
        private readonly IInitializeSystem[] emptyInitialize = { };
        private readonly IUpdateSystem[] emptyUpdate = { };
        private readonly IDisposeSystem[] emptyDispose = { };
        private readonly UpdateStep testUpdateStep = new("Test", 0);
        private readonly UpdateStepRegistry registry;

        public WorldTest()
        {
            registry = new UpdateStepRegistry(new[] { testUpdateStep });
        }
        
        [Fact]
        public void ShouldInitialize()
        {
            var world = new YaEcs.World(emptyInitialize, emptyUpdate, emptyDispose, registry);
            Assert.NotNull(world);
        }
        
        [Fact]
        public void ShouldExecuteInitializeSystems()
        {
            var initialize = Enumerable.Range(0, 2)
                .Select(_ => new InitializeSystem())
                .ToList();
            var world = new YaEcs.World(initialize, emptyUpdate, emptyDispose, registry);
            foreach (var system in initialize)
            {
                Assert.True(system.IsInitialized);
            }
        }

        [Fact]
        public void ShouldExecuteUpdateSystems()
        {
            var update = Enumerable.Range(0, 2)
                .Select(_ => new UpdateSystem { UpdateStep = testUpdateStep })
                .ToList();
            var wold = new YaEcs.World(emptyInitialize, update, emptyDispose, registry);
            wold.Update();
            foreach (var system in update)
            {
                Assert.Equal(1, system.FramesCount);
            }
        }

        [Fact]
        public void ShouldExecuteUpdateInOrder()
        {
            var updateStep1 = new UpdateStep("Step1", 1);
            var updateStep2 = new UpdateStep("Step2", 2);
            var steps = new[] { updateStep2, updateStep1 };
            var updateStepRegistry = new UpdateStepRegistry(steps);
            var system1 = new UpdateSystem { UpdateStep = updateStep1 };
            var system2 = new UpdateOrderSystem { UpdateStep = updateStep2, OtherSystem = system1 };
            var update = new IUpdateSystem[] { system2, system1 };
            var world = new YaEcs.World(emptyInitialize, update, emptyDispose, updateStepRegistry);
            world.Update();
        }

        [Fact]
        public void ShouldExecuteDisposeSystems()
        {
            var dispose = Enumerable.Range(0, 2)
                .Select(_ => new DisposeSystem())
                .ToList();
            var world = new YaEcs.World(emptyInitialize, emptyUpdate, dispose, registry);
            world.Dispose();
            foreach (var system in dispose)
            {
                Assert.True(system.IsDisposed);
            }
        }
        
        private class InitializeSystem : IInitializeSystem
        {
            public bool IsInitialized;
            
            public void Execute(YaEcs.World world)
            {
                IsInitialized = true;
            }
        }
        
        private class UpdateSystem : IUpdateSystem
        {
            public UpdateStep UpdateStep { get; init; }

            public int FramesCount;
            
            public void Execute(YaEcs.World world)
            {
                ++FramesCount;
            }
        }

        private class UpdateOrderSystem : IUpdateSystem
        {
            public UpdateStep UpdateStep { get; init; }
            public UpdateSystem OtherSystem;
            
            public int FramesCount;
            public void Execute(YaEcs.World world)
            {
                Assert.True(OtherSystem.FramesCount > FramesCount);
                ++FramesCount;
            }
        }

        private class DisposeSystem : IDisposeSystem
        {
            public bool IsDisposed;
            
            public void Execute(YaEcs.World world)
            {
                IsDisposed = true;
            }
        }
    }
}