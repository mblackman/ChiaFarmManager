using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ChiaAdapter
{
    /// <summary>
    /// Cross platform command-line service to interact with Chia farms.
    /// </summary>
    public class CliChiaAdapter : IChiaAdapter
    {
        private readonly IChiaClient chiaClient;

        /// <summary>
        /// Creates a new instance of <see cref="CliChiaAdapter"/>.
        /// </summary>
        /// <param name="chiaClient">The client to interact with a Chia node.</param>
        public CliChiaAdapter(IChiaClient chiaClient)
        {
            this.chiaClient = chiaClient ?? throw new ArgumentNullException(nameof(chiaClient));
        }

        /// <summary>
        /// Creates plots.
        /// </summary>
        /// <param name="plottingOptions">The options of how to create the plots.</param>
        /// <param name="cancellationToken">A token to cancel to create plot request.</param>
        /// <returns>The result from the operation.</returns>
        public async Task<PlottingResults> CreatePlotsAsync(PlottingOptions plottingOptions, CancellationToken cancellationToken)
        {
            if (plottingOptions is null)
            {
                throw new ArgumentNullException(nameof(plottingOptions));
            }

            const string plotCommandFormat = "plots create {0}";
            var results = await chiaClient.RunCommandAsync(string.Format(plotCommandFormat, plottingOptions.ToParameterString()), cancellationToken);

            return new PlottingResults()
            {
                IsSuccess = results.IsSuccess,
                PlottingOptions = plottingOptions
            };
        }

        /// <summary>
        /// Gets the current <see cref="GetFarmSummaryAsync"/>.
        /// </summary>
        /// <returns>The <see cref="FarmSummary"/> for this <see cref="IChiaAdapter"/>.</returns>
        public async Task<FarmSummary> GetFarmSummaryAsync()
        {
            var farmSummary = new FarmSummary();
            var summaryCommandResults = await chiaClient.RunCommandAsync("farm summary", CancellationToken.None);

            if (!summaryCommandResults.IsSuccess)
            {
                return null;
            }

            string[] lines = summaryCommandResults.Result.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
            Dictionary<string, string> parameters = lines.Select(l => l.Split(':')).Where(l => l.Length == 2).ToDictionary(k => k[0].Trim().ToLower(), v => v[1].Trim().ToLower());

            foreach (var parameter in parameters)
            {
                switch (parameter.Key)
                {
                    case "farming status":

                        break;

                    case "total chia farmed":
                        if (float.TryParse(parameter.Value, out float totalChiaFarmed))
                        {
                            farmSummary.TotalChiaFarmed = totalChiaFarmed;
                        }
                        break;

                    case "user transaction fees":
                        if (float.TryParse(parameter.Value, out float userTransactionFees))
                        {
                            farmSummary.UserTransactionFees = userTransactionFees;
                        }
                        break;

                    case "block rewards":
                        if (float.TryParse(parameter.Value, out float blockRewards))
                        {
                            farmSummary.BlockRewards = blockRewards;
                        }
                        break;

                    case "last height farmed":
                        if (int.TryParse(parameter.Value, out int lastHeightFarmed))
                        {
                            farmSummary.LastHeightFarmed = lastHeightFarmed;
                        }
                        break;

                    case "plot count":
                        if (int.TryParse(parameter.Value, out int plotCount))
                        {
                            farmSummary.PlotCount = plotCount;
                        }
                        break;

                    case "total size of plots":
                        if (Utils.TryParseStringToGiBs(parameter.Value, out float totalSizeOfPlots))
                        {
                            farmSummary.TotalSizeOfPlotsGiB = totalSizeOfPlots;
                        }
                        break;

                    case "estimated network space":
                        if (Utils.TryParseStringToGiBs(parameter.Value, out float estimatedNetworkSpace))
                        {
                            farmSummary.EstimatedNetworkSpaceGiB = estimatedNetworkSpace;
                        }
                        break;

                    case "expected time to win":
                        farmSummary.ExpectedTimeToWin = parameter.Value;
                        break;
                }
            }

            return farmSummary;
        }
    }
}
