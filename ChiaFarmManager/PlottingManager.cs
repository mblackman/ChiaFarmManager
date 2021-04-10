using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using ChiaAdapter;

using Common;

namespace ChiaFarmManager
{
    public class PlottingManager
    {
        private const string StartPlottingFormat = "Started k = {0} plot in temp directory: {1} with destination: {2}";
        private const string EndPlottingFormat = "Finished plotting k = {0} plot in temp directory: {1} with destination: {2}";

        private readonly KSize PlotSize = KSize.Create(32);

        private readonly ILogger<PlottingManager> logger;
        private readonly IChiaAdapter chiaAdapter;
        private readonly IEnumerable<string> tempFolders;
        private readonly IEnumerable<string> destinationFolders;
        private readonly PlottingManagerOptions plottingManagerOptions;

        public PlottingManager(ILogger<PlottingManager> logger, IChiaAdapter chiaAdapter, IEnumerable<string> tempFolders, IEnumerable<string> destinationFolders)
            : this(logger, chiaAdapter, tempFolders, destinationFolders, new PlottingManagerOptions())
        {

        }

        public PlottingManager(ILogger<PlottingManager> logger, IChiaAdapter chiaAdapter, IEnumerable<string> tempFolders, IEnumerable<string> destinationFolders, PlottingManagerOptions plottingManagerOptions)
        {
            this.logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
            this.chiaAdapter = chiaAdapter ?? throw new System.ArgumentNullException(nameof(chiaAdapter));
            this.tempFolders = tempFolders ?? throw new System.ArgumentNullException(nameof(tempFolders));
            this.destinationFolders = destinationFolders ?? throw new System.ArgumentNullException(nameof(destinationFolders));
            this.plottingManagerOptions = plottingManagerOptions;
        }

        public async Task Plot(CancellationToken cancellationToken)
        {
            List<Task<PlottingResults>> plotTasks = new();
            Dictionary<string, int> runningPlots = new();

            logger.LogInfo("Starting process to fill out directories.");

            var currentDestination = GetNextAvailableDestination(runningPlots);

            if (currentDestination is null)
            {
                return;
            }

            foreach (string tempDirectory in tempFolders)
            {
                plotTasks.Add(CreateNextPlot(currentDestination, tempDirectory, runningPlots, cancellationToken));

                currentDestination = GetNextAvailableDestination(runningPlots);

                if (currentDestination is null)
                {
                    break;
                }
            }

            while (plotTasks.Any())
            {
                var finishedTask = await Task.WhenAny(plotTasks);
                plotTasks.Remove(finishedTask);
                var result = await finishedTask;
                runningPlots[result.DestinationDirectory]--;

                logger.LogInfo(string.Format(EndPlottingFormat, PlotSize.Value, result.TempDirectory, result.DestinationDirectory));

                currentDestination = GetNextAvailableDestination(runningPlots);

                if (currentDestination != null)
                {
                    plotTasks.Add(CreateNextPlot(currentDestination, result.TempDirectory, runningPlots, cancellationToken));
                }
            }
        }

        private Task<PlottingResults> CreateNextPlot(string destination, string tempDirectory, Dictionary<string, int> runningPlots, CancellationToken cancellationToken)
        {
            ClearDirectory(tempDirectory);
            Task<PlottingResults> item = chiaAdapter.CreatePlots(new PlottingOptions(tempDirectory, destination), cancellationToken);
            runningPlots[destination] = runningPlots.GetValueOrDefault(destination) + 1;

            logger.LogInfo(string.Format(StartPlottingFormat, PlotSize.Value, tempDirectory, destination));

            return item;
        }

        private void ClearDirectory(string directoryPath)
        {
            logger.LogDebug("Clearing directory: " + directoryPath);

            foreach (var file in new DirectoryInfo(directoryPath).EnumerateFiles())
            {
                file.Delete();
            }
        }

        private string GetNextAvailableDestination(IReadOnlyDictionary<string, int> potentialPlots)
        {
            return destinationFolders
                .Select(f => new DirectoryInfo(f))
                .Where(i => i.Exists && !string.IsNullOrEmpty(i.Root.Name))
                .Select(i => (i, new DriveInfo(i.Root.Name)))
                .OrderBy(i => i.Item2.AvailableFreeSpace)
                .FirstOrDefault(i =>
                {
                    string directoryName = i.i.FullName;
                    long potentialPlotsSize = potentialPlots.GetValueOrDefault(directoryName) * PlotSize.FinalSizeBytes;
                    return i.Item2.AvailableFreeSpace - potentialPlotsSize > PlotSize.FinalSizeBytes;
                }).i?.FullName;
        }
    }
}
