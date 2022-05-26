namespace YaEcs.Bootstrap
{
    public static class UpdateSteps
    {
        public static UpdateStep First = new(nameof(First), 10);
        public static UpdateStep EarlyUpdate = new(nameof(EarlyUpdate), 20);
        public static UpdateStep Update = new(nameof(Update), 30);
        public static UpdateStep LateUpdate = new(nameof(LateUpdate), 40);
    }
}