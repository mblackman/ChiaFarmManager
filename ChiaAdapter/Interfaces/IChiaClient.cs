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
        Task<ClientResult> RunCommandAsync(string arguments, CancellationToken cancellationToken);
    }

    public class ClientResult
    {
        public string Result { get; private set; }
        public bool IsSuccess { get; private set; }
        public string ErrorMessage { get; private set; }

        public static ClientResult Success(string result) => new ClientResult()
        {
            IsSuccess = true,
            Result = result
        };

        public static ClientResult Failure(string errorMessage) => new ClientResult()
        {
            IsSuccess = false,
            ErrorMessage = errorMessage
        };
    }
}
