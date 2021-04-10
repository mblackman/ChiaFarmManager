using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

using Common;

namespace ChiaAdapter
{
    /// <summary>
    /// Runs a local process to interact with a local Chia executable.
    /// </summary>
    public class ProcessChiaClient : IChiaClient
    {
        private readonly string exePath;
        private readonly ILogger<ProcessChiaClient> logger;

        /// <summary>
        /// Creates a new instance of <see cref="ProcessChiaClient"/>.
        /// </summary>
        /// <param name="exePath">The path to the executable.</param>
        public ProcessChiaClient(string exePath, ILogger<ProcessChiaClient> logger)
        {
            this.exePath = exePath ?? throw new ArgumentNullException(nameof(exePath));
            this.logger = logger;
        }

        /// <summary>
        /// Runs the given command on the Chia Client.
        /// </summary>
        /// <param name="arguments">The arguments to pass to the Chia executable.</param>
        /// <returns>The results from the command.</returns>
        public async Task<ClientResult> RunCommandAsync(string arguments, CancellationToken cancellationToken)
        {
            logger.LogDebug("Running command: Chia " + arguments);

            var process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = exePath,
                    Arguments = arguments,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                }
            };

            process.Start();
            await process.WaitForExitAsync(cancellationToken);

            if (process.ExitCode == 0)
            {
                // Gracefully ended.
                string result = await process.StandardOutput.ReadToEndAsync();

                logger.LogDebug("Successful result from command: Chia " + arguments);
                logger.LogDebug(result);

                return ClientResult.Success(result);
            }
            else
            {
                // Something went wrong.
                string error = await process.StandardError.ReadToEndAsync();
                logger.LogDebug("Failed from command: Chia " + arguments);
                logger.LogDebug(error);

                return ClientResult.Failure(error);
            }
        }
    }
}
