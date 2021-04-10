using System;

namespace ChiaAdapter
{
    /// <summary>
    /// Information about Chia k-sizes.
    /// </summary>
    public struct KSize
    {
        private KSize(int value, long tempSizeBytes, long finalSizeBytes)
        {
            Value = value;
            TempSizeBytes = tempSizeBytes;
            FinalSizeBytes = finalSizeBytes;
        }

        /// <summary>
        /// The k-size value.
        /// </summary>
        public int Value { get; }

        /// <summary>
        /// The estimated size of Chia plot temp files during the plotting process.
        /// </summary>
        public long TempSizeBytes { get; }

        /// <summary>
        /// The estimated size of the final plot file after plotting.
        /// </summary>
        public long FinalSizeBytes { get; }

        /// <summary>
        /// Creates a new instance of <see cref="KSize"/> based on the given size.
        /// </summary>
        /// <param name="size">The k-size value.</param>
        /// <returns>The <see cref="KSize"/> for the given value.</returns>
        public static KSize Create(int size) => size switch
        {
            28 => new KSize(size, GbToBtyes(1f), GbToBtyes(1f)),
            32 => new KSize(size, GbToBtyes(356.5f), GbToBtyes(108.9f)),
            33 => new KSize(size, GbToBtyes(632.4f), GbToBtyes(224.2f)),
            34 => new KSize(size, GbToBtyes(1263.8f), GbToBtyes(461.5f)),
            35 => new KSize(size, GbToBtyes(2528.7f), GbToBtyes(949.3f)),
            _ => throw new ArgumentException("Unsupported k size: " + size)
        };

        private static long GbToBtyes(float sizeGB)
        {
            return Convert.ToInt64(sizeGB * Math.Pow(10, 8));
        }
    }
}
