namespace ChiaAdapter
{
    /// <summary>
    /// Creates a new instance of <see cref="IChiaAdapter"/>.
    /// </summary>
    public interface IChiaAdapterFactory
    {
        /// <summary>
        /// Creates the new <see cref="IChiaAdapter"/>.
        /// </summary>
        /// <returns>The new <see cref="IChiaAdapter"/>.</returns>
        IChiaAdapter Create();
    }
}
