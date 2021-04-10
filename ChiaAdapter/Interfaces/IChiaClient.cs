using System.Threading;
using System.Threading.Tasks;

namespace ChiaAdapter
{
    /// <summary>
    /// Interface to interact with a Chia Node, Farmer, Wallet.
    /// </summary>
    public interface IChiaClient
    {
        /// <summary>
        /// Runs the given command on the Chia Client.
        /// </summary>
        /// <param name="arguments">The arguments to pass to the Chia executable.</param>
        /// <returns>The results from the command.</returns>
        Task<string> RunCommandAsync(string arguments, CancellationToken cancellationToken);
    }
}
