using System.Collections.Generic;
using System.Linq;

namespace YaEcs
{
    public class UpdateStepRegistry
    {
        private readonly Dictionary<int, UpdateStep> priorityToStep = new();
        private readonly Dictionary<string, UpdateStep> nameToStep = new();
        private readonly List<UpdateStep> sortedSteps = new();

        public UpdateStepRegistry(IEnumerable<UpdateStep> steps)
        {
            foreach (var step in steps.OrderBy(x => x.Priority))
            {
                sortedSteps.Add(step);
                priorityToStep.Add(step.Priority, step);
                nameToStep.Add(step.Name, step);
            }
        }

        public IReadOnlyList<UpdateStep> UpdateSteps => sortedSteps;
    }
}