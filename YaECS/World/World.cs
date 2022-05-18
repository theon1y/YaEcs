using System.Collections.Generic;
using System.Linq;

namespace YaEcs
{
    public class World : IWorld
    {
        public IComponents Components { get; }
        public IEntities Entities { get; }

        private readonly UpdateStepRegistry updateStepRegistry;
        private readonly List<IInitializeSystem> initializeSystems;
        private readonly Dictionary<int, List<ISystem>> updateSystems;
        private readonly List<ISystem> disposeSystems;

        public World(UpdateStepRegistry updateStepRegistry,
            IEnumerable<IInitializeSystem> initializeSystems,
            IEnumerable<IUpdateSystem> updateSystems,
            IEnumerable<IDisposeSystem> disposeSystems,
            IComponents components, IEntities entities)
        {
            this.updateStepRegistry = updateStepRegistry;
            this.initializeSystems = initializeSystems.ToList();
            this.updateSystems = updateSystems
                .GroupBy(x => x.UpdateStep.Priority)
                .ToDictionary(
                    x => x.Key,
                    x => x.Cast<ISystem>().ToList());
            this.disposeSystems = disposeSystems.Cast<ISystem>().ToList();
            Components = components;
            Entities = entities;
        }

        public void Initialize()
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