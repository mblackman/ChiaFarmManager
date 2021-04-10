namespace ChiaFarmManager
{
    public class PlottingManagerOptions
    {
        private const int DefaultThreads = 2;
        private const int DefaultKSize = 32;
        private const int DefaultTotalMemoryPerPlotMB = 4750;

        public long TotalMemoryPerPlotMB { get; set; } = DefaultTotalMemoryPerPlotMB;

        public int ThreadsPerPlot { get; set; } = DefaultThreads;

        public int KSize { get; set; } = DefaultKSize;
    }
}
