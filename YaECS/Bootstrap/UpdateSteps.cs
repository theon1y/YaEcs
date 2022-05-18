namespace YaEcs.Bootstrap
{
    public static class UpdateSteps
    {
        public static UpdateStep First = new(nameof(First), 1);
        public static UpdateStep EarlyUpdate = new(nameof(EarlyUpdate), 1024);
        public static UpdateStep Update = new(nameof(Update), 2048);
        public static UpdateStep LateUpdate = new(nameof(LateUpdate), 4096);
    }
}