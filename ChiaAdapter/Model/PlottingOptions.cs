using System;
using System.Collections.Generic;
using System.Linq;

namespace ChiaAdapter
{
    /// <summary>
    /// Options that can be set during the plotting process.
    /// </summary>
    public class PlottingOptions
    {
        /// <summary>
        /// Creates a new instance of <see cref="PlottingOptions"/>.
        /// </summary>
        /// <param name="tempDirectory">The directory to store the temp files.</param>
        /// <param name="finalDirectory">The directory to store the final files.</param>
        public PlottingOptions(string tempDirectory, string finalDirectory)
        {
            TempDirectory = tempDirectory ?? throw new ArgumentNullException(nameof(tempDirectory));
            DestinationDirectory = finalDirectory ?? throw new ArgumentNullException(nameof(finalDirectory));
        }

        /// <summary>
        /// Gets the temp directory.
        /// </summary>
        public string TempDirectory { get; }

        /// <summary>
        /// Gets the final directory.
        /// </summary>
        public string DestinationDirectory { get; }

        /// <summary>
        /// Gets the k-size of the plot.
        /// </summary>
        public int? Size { get; set; }

        /// <summary>
        /// Gets the number of plots of make.
        /// </summary>
        public int? NumberOfPlots { get; set; }

        /// <summary>
        /// Gets the size of the memory buffer.
        /// </summary>
        public int? MemoryBufferSize { get; set; }

        /// <summary>
        /// Gets the number of threads.
        /// </summary>
        public int? NumberOfThreads { get; set; }

        /// <summary>
        /// Converts these options to a parameter string to use with the chia cli.
        /// </summary>
        /// <returns>A stringified version of these options.</returns>
        public string ToParameterString()
        {
            var parameters = new List<(string, string)>
            {
                ("-t", FormatPath(TempDirectory)),
                ("-d", FormatPath(DestinationDirectory))
            };

            if (Size.HasValue)
            {
                parameters.Add(("-k", Size.Value.ToString()));
            }

            if (NumberOfPlots.HasValue)
            {
                parameters.Add(("-n", NumberOfPlots.Value.ToString()));
            }

            if (MemoryBufferSize.HasValue)
            {
                parameters.Add(("-b", MemoryBufferSize.Value.ToString()));
            }

            if (NumberOfThreads.HasValue)
            {
                parameters.Add(("-r", NumberOfThreads.Value.ToString()));
            }

            return string.Join(' ', parameters.Select(p => string.Join(' ', p.Item1, p.Item2)));
        }

        private static string FormatPath(string path)
        {
            if (!path.StartsWith('"'))
            {
                path = "\"" + path;
            }
            if (!path.EndsWith('"'))
            {
                path += "\"";
            }

            return path;
        }
    }
}
