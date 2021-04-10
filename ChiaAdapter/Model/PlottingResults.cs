namespace ChiaAdapter
{
    /// <summary>
    /// Represents the results of a plotting operation.
    /// </summary>
    public class PlottingResults
    {
        /// <summary>
        /// Whether the plot as successfully made.
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// Gets or sets the options used for this operation.
        /// </summary>
        public PlottingOptions PlottingOptions { get; set; }
    }
}
