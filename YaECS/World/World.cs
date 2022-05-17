using System;
using System.Collections.Generic;
using System.Linq;

namespace YaEcs
{
    public class World : IDisposable
    {
        public readonly Components Components;
        public readonly Entities Entities;

        private readonly UpdateStepRegistry updateStepRegistry;  
        private readonly Dictionary<int, List<ISystem>> updateSystems;
        private readonly List<ISystem> disposeSystems;

        public World(IEnumerable<IInitializeSystem> initializeSystems, IEnumerable<IUpdateSystem> updateSystems,
            IEnumerable<IDisposeSystem> disposeSystems, UpdateStepRegistry updateStepRegistry)
        {
            this.updateStepRegistry = updateStepRegistry;
            this.updateSystems = updateSystems
                .GroupBy(x => x.UpdateStep.Priority)
                .ToDictionary(
                    x => x.Key,
                    x => x.Cast<ISystem>().ToList());
            this.disposeSystems = disposeSystems.Cast<ISystem>().ToList();
            Components = new Components();
            Entities = new Entities();
            Initialize(initializeSystems);
        }

        public void Initialize(IEnumerable<IInitializeSystem> initializeSystems)
        {
            foreach (var system in initializeSystems)
            {
                system.Execute(this);
            }
        }

        public void Update()
        {
            foreach (var step in updateStepRegistry.UpdateSteps)
            {
                if (!updateSystems.TryGetValue(step.Priority, out var systems)) continue;
                
                ExecuteSystems(systems);
            }
        }

        public void Dispose()
        {
            ExecuteSystems(disposeSystems);
        }

        private void ExecuteSystems(List<ISystem> systems)
        {
            foreach (var system in systems)
            {
                system.Execute(this);
            }
        }
    }
}