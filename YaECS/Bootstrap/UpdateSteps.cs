namespace YaEcs.Bootstrap
{
    public static class UpdateSteps
    {
        public static UpdateStep EarlyUpdate = new UpdateStep(nameof(EarlyUpdate), 1024);
        public static UpdateStep Update = new UpdateStep(nameof(Update), 2048);
        public static UpdateStep LateUpdate = new UpdateStep(nameof(LateUpdate), 4096);
    }
}