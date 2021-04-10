using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using ChiaAdapter;

using Common;

namespace ChiaFarmManager
{
    /// <summary>
    /// Helps manage plotting Chia plots across many destinations.
    /// </summary>
    public class PlottingManager
    {
        private const int MaxPlottingFailures = 3;

        private const string StartPlottingFormat = "Started k = {0} plot in temp directory: {1} with destination: {2}";
        private const string EndPlottingFormat = "Finished plotting k = {0} plot in temp directory: {1} with destination: {2}";

        private readonly ILogger logger;
        private readonly IChiaAdapter chiaAdapter;
        private readonly IEnumerable<string> tempFolders;
        private readonly IEnumerable<string> destinationFolders;
        private readonly PlottingManagerOptions plottingManagerOptions;


        /// <summary>
        /// Creates a new instance of <see cref="PlottingManager"/>.
        /// </summary>
        /// <param name="logger">The logger to use.</param>
        /// <param name="chiaAdapter">The adapter to get/set data.</param>
        /// <param name="tempFolders">The folder to use as temp plot store.</param>
        /// <param name="destinationFolders">The destination folders to place the final plots.</param>
        /// <param name="plottingManagerOptions">Options for the manager.</param>
        public PlottingManager(ILogger logger, IChiaAdapter chiaAdapter, IEnumerable<string> tempFolders, IEnumerable<string> destinationFolders, PlottingManagerOptions plottingManagerOptions)
        {
            this.logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
            this.chiaAdapter = chiaAdapter ?? throw new System.ArgumentNullException(nameof(chiaAdapter));
            this.tempFolders = tempFolders ?? throw new System.ArgumentNullException(nameof(tempFolders));
            this.destinationFolders = destinationFolders ?? throw new System.ArgumentNullException(nameof(destinationFolders));
            this.plottingManagerOptions = plottingManagerOptions;
        }

        /// <summary>
        /// Plots to the destination drives until they are full.
        /// </summary>
        /// <param name="cancellationToken">A token to use to cancel sub-processes.</param>
        /// <returns>The task from the operation.</returns>
        public async Task Plot(CancellationToken cancellationToken)
        {
            List<Task<PlottingResults>> plotTasks = new();
            Dictionary<string, int> runningPlots = new();
            int numberPlotFailures = 0;

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

                if (result.IsSuccess)
                {
                    runningPlots[result.PlottingOptions.DestinationDirectory]--;

                    logger.LogInfo(string.Format(EndPlottingFormat, plottingManagerOptions.KSize, result.PlottingOptions.TempDirectory, result.PlottingOptions.DestinationDirectory));

                    currentDestination = GetNextAvailableDestination(runningPlots);

                    if (currentDestination != null)
                    {
                        plotTasks.Add(CreateNextPlot(currentDestination, result.PlottingOptions.TempDirectory, runningPlots, cancellationToken));
                    }
                }
                else
                {
                    numberPlotFailures++;

                    if (numberPlotFailures >= MaxPlottingFailures)
                    {
                        logger.LogError("Exceed maximum number of failed plots. Canceling job.");
                        return;
                    }
                }
            }

            logger.LogInfo("Finished filling out directories.");
        }

        private Task<PlottingResults> CreateNextPlot(string destination, string tempDirectory, Dictionary<string, int> runningPlots, CancellationToken cancellationToken)
        {
            ClearDirectory(tempDirectory);
            PlottingOptions plottingOptions = new PlottingOptions(tempDirectory, destination)
            {
                Size = plottingManagerOptions.KSize,
                NumberOfThreads = plottingManagerOptions.ThreadsPerPlot
            };
            Task<PlottingResults> item = chiaAdapter.CreatePlotsAsync(plottingOptions, cancellationToken);
            runningPlots[destination] = runningPlots.GetValueOrDefault(destination) + 1;

            logger.LogInfo(string.Format(StartPlottingFormat, plottingManagerOptions.KSize, tempDirectory, destination));

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
            var plotSize = KSize.Create(plottingManagerOptions.KSize);
            return destinationFolders
                .Select(f => new DirectoryInfo(f))
                .Where(i => i.Exists && !string.IsNullOrEmpty(i.Root.Name))
                .Select(i => (i, new DriveInfo(i.Root.Name)))
                .OrderBy(i => i.Item2.AvailableFreeSpace)
                .FirstOrDefault(i =>
                {
                    string directoryName = i.i.FullName;
                    long potentialPlotsSize = potentialPlots.GetValueOrDefault(directoryName) * plotSize.FinalSizeBytes;
                    return i.Item2.AvailableFreeSpace - potentialPlotsSize > plotSize.FinalSizeBytes;
                }).i?.FullName;
        }
    }
}
