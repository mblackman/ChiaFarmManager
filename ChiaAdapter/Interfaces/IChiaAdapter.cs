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
    }
}
