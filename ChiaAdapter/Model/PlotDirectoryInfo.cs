using System;

namespace ChiaAdapter
{
    public class PlotDirectoryInfo : IEquatable<PlotDirectoryInfo>
    {
        public PlotDirectoryInfo(string path, int numberPlots, int numberPlotsRemaining)
        {
            Path = path ?? throw new ArgumentNullException(nameof(path));
            NumberPlots = numberPlots;
            NumberPlotsRemaining = numberPlotsRemaining;
        }

        public string Path { get; }
        public int NumberPlots { get; }
        public int NumberPlotsRemaining { get; }

        public bool Equals(PlotDirectoryInfo other)
        {
            if (other is null) return false;

            return string.CompareOrdinal(Path, other.Path) == 0;
        }
    }
}
