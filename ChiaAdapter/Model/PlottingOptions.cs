using System;
using System.Collections.Generic;
using System.Linq;

namespace ChiaAdapter
{
    public class PlottingOptions
    {
        public PlottingOptions(string tempDirectory, string finalDirectory)
        {
            TempDirectory = tempDirectory ?? throw new ArgumentNullException(nameof(tempDirectory));
            FinalDirectory = finalDirectory ?? throw new ArgumentNullException(nameof(finalDirectory));
        }

        public string TempDirectory { get; }
        public string FinalDirectory { get; }
        public int? Size { get; set; }
        public int? NumberOfPlots { get; set; }
        public int? MemoryBufferSize { get; set; }
        public int? NumberOfThreads { get; set; }

        public string ToParameterString()
        {
            var parameters = new List<(string, string)>();

            parameters.Add(("-t", TempDirectory));
            parameters.Add(("-d", FinalDirectory));

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
    }
}
