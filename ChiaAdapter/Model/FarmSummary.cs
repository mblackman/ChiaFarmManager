namespace ChiaAdapter
{
    /// <summary>
    /// The summary information of a Chia farm.
    /// </summary>
    public class FarmSummary
    {
        /// <summary>
        /// Gets of sets the current <see cref="FarmStatus"/>.
        /// </summary>
        public FarmStatus FarmStatus { get; set; }

        /// <summary>
        /// Gets of sets the total chia farmed.
        /// </summary>
        public float TotalChiaFarmed { get; set; }

        /// <summary>
        /// Gets or sets the user transaction fees.
        /// </summary>
        public float UserTransactionFees { get; set; }

        /// <summary>
        /// Gets or sets the current block rewards.
        /// </summary>
        public float BlockRewards { get; set; }

        /// <summary>
        /// Gets or sets the height farmed.
        /// </summary>
        public int LastHeightFarmed { get; set; }

        /// <summary>
        /// Gets or sets the plot count.
        /// </summary>
        public int PlotCount { get; set; }

        /// <summary>
        /// Gets or sets the total size of plots in GiB.
        /// </summary>
        public float TotalSizeOfPlotsGiB { get; set; }

        /// <summary>
        /// Gets or sets the estimated network space in GiB.
        /// </summary>
        public float EstimatedNetworkSpaceGiB { get; set; }

        /// <summary>
        /// Gets or sets the expected time to win.
        /// </summary>
        public string ExpectedTimeToWin { get; set; }
    }
}
