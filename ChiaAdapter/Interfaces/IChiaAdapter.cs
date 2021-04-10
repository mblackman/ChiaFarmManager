using System.Threading;
using System.Threading.Tasks;

namespace ChiaAdapter
{
    /// <summary>
    /// A service for getting information about a current Chia instance.
    /// </summary>
    public interface IChiaAdapter
    {
        /// <summary>
        /// Gets the current <see cref="GetFarmSummaryAsync"/>.
        /// </summary>
        /// <returns>The <see cref="FarmSummary"/> for this <see cref="IChiaAdapter"/>.</returns>
        Task<FarmSummary> GetFarmSummaryAsync();

        /// <summary>
        /// Creates plots.
        /// </summary>
        /// <param name="plottingOptions">The options of how to create the plots.</param>
        /// <param name="cancellationToken">A token to cancel to create plot request.</param>
        /// <returns>The result from the operation.</returns>
        Task<PlottingResults> CreatePlotsAsync(PlottingOptions plottingOptions, CancellationToken cancellationToken);
    }
}
