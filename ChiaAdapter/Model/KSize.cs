using System;

namespace ChiaAdapter
{
    public struct KSize
    {
        private KSize(int value, long tempSizeBytes, long finalSizeBytes)
        {
            Value = value;
            TempSizeBytes = tempSizeBytes;
            FinalSizeBytes = finalSizeBytes;
        }

        public int Value { get; }
        public long TempSizeBytes { get; }
        public long FinalSizeBytes { get; }

        public static KSize Create(int size) => size switch
        {
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
