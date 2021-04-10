using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace ChiaAdapter
{
    /// <summary>
    /// Runs a local process to interact with a local Chia executable.
    /// </summary>
    public class ProcessChiaClient : IChiaClient
    {
        private readonly string exePath;

        /// <summary>
        /// Creates a new instance of <see cref="ProcessChiaClient"/>.
        /// </summary>
        /// <param name="exePath">The path to the executable.</param>
        public ProcessChiaClient(string exePath)
        {
            this.exePath = exePath ?? throw new ArgumentNullException(nameof(exePath));
        }

        /// <summary>
        /// Runs the given command on the Chia Client.
        /// </summary>
        /// <param name="arguments">The arguments to pass to the Chia executable.</param>
        /// <returns>The results from the command.</returns>
        public async Task<string> RunCommandAsync(string arguments, CancellationToken cancellationToken)
        {
            var process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = exePath,
                    Arguments = arguments,
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                }
            };

            process.Start();
            await process.WaitForExitAsync(cancellationToken);
            string result = await process.StandardOutput.ReadToEndAsync();
            return result;
        }
    }
}
