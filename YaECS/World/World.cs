using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YaEcs
{
    public class World : IWorld
    {
        public IComponents Components { get; }
        public IEntities Entities { get; }

        private readonly UpdateStepRegistry updateStepRegistry;
        private readonly List<IInitializeSystem> initializeSystems;
        private readonly Dictionary<int, List<ISystem>> updateSystems;
        private readonly List<IDisposeSystem> disposeSystems;

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
            this.disposeSystems = disposeSystems.ToList();
            Components = components;
            Entities = entities;
        }

        public async Task InitializeAsync()
        {
            var initializeGroups = initializeSystems
                .GroupBy(x => x.Priority)
                .OrderBy(x => x.Key);
            foreach (var systems in initializeGroups)
            {
                await ExecuteSystemsAsync(systems);
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

        public ValueTask DisposeAsync()
        {
            return ExecuteSystemsAsync(disposeSystems);
        }

        private void ExecuteSystems(List<ISystem> systems)
        {
            foreach (var system in systems)
            {
                system.Execute(this);
            }
        }

        private async ValueTask ExecuteSystemsAsync(IEnumerable<IAsyncSystem> systems)
        {
            var tasks = systems
                .Select(x => x.ExecuteAsync(this))
                .ToList();
            await Task.WhenAll(tasks);
        }
    }
}