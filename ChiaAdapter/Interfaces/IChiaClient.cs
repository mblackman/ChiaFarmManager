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

    /// <summary>
    /// The result from running a command.
    /// </summary>
    public class ClientResult
    {
        /// <summary>
        /// The result from a valid request.
        /// </summary>
        public string Result { get; private set; }

        /// <summary>
        /// Gets whether the operation was a success.
        /// </summary>
        public bool IsSuccess { get; private set; }

        /// <summary>
        /// Gets any error messages.
        /// </summary>
        public string ErrorMessage { get; private set; }

        /// <summary>
        /// Creates a new successful <see cref="ClientResult"/>.
        /// </summary>
        /// <param name="result">The result</param>
        /// <returns>The successful <see cref="ClientResult"/>.</returns>
        public static ClientResult Success(string result) => new ClientResult()
        {
            IsSuccess = true,
            Result = result
        };

        /// <summary>
        /// Creates a new failed <see cref="ClientResult"/>.
        /// </summary>
        /// <param name="errorMessage">The error message.</param>
        /// <returns>The failed <see cref="ClientResult"/>.</returns>
        public static ClientResult Failure(string errorMessage) => new ClientResult()
        {
            IsSuccess = false,
            ErrorMessage = errorMessage
        };
    }
}
