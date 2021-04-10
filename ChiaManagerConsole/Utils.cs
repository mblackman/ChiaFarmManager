using System;

using ChiaAdapter;

namespace ChiaManagerConsole
{
    public static class Utils
    {
        private static readonly string FarmSummaryFormat =
            "Status: {0}" + Environment.NewLine
            + "Total Chia Farmed: {1}" + Environment.NewLine
            + "Plot Count: {2}" + Environment.NewLine
            + "Total Size of Plots (GiB): {3}" + Environment.NewLine
            + "Estimated Network Space (GiB): {4}" + Environment.NewLine
            + "Expected Time to Win: {5}" + Environment.NewLine;
        public static string ToDisplayString<T>(T value) => value switch
        {
            FarmSummary farmSummary => string.Format(FarmSummaryFormat, farmSummary.FarmStatus, farmSummary.TotalChiaFarmed, farmSummary.PlotCount, farmSummary.TotalSizeOfPlotsGiB, farmSummary.EstimatedNetworkSpaceGiB, farmSummary.ExpectedTimeToWin),
            _ => value.ToString()
        };
    }
}
