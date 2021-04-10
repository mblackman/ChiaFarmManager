using System;

namespace ChiaAdapter
{
    /// <summary>
    /// Utilities to help with processes.
    /// </summary>
    public static class Utils
    {
        /// <summary>
        /// Tries to parse a string with storage size information.
        /// </summary>
        /// <param name="parse">The string to parse.</param>
        /// <param name="value">The value of storage size in GiB.</param>
        /// <returns>True if the string could be parsed, else false.</returns>
        public static bool TryParseStringToGiBs(string parse, out float value)
        {
            const int sizeFactor = 1024;

            value = default;

            if (string.IsNullOrEmpty(parse))
            {
                return false;
            }

            var parameters = parse.Split(' ');

            if (parameters.Length != 2)
            {
                return false;
            }

            if (!float.TryParse(parameters[0].Trim(), out float parsedValue))
            {
                return false;
            }

            int multiplicationFactor;

            switch (parameters[1].Trim().ToLower())
            {
                case "gib":
                    multiplicationFactor = 0;
                    break;

                case "tib":
                    multiplicationFactor = 1;
                    break;

                case "pib":
                    multiplicationFactor = 2;
                    break;

                default:
                    Console.WriteLine("Unknown denomination of size: " + parameters[1]);
                    return false;
            }

            value = parsedValue * sizeFactor * multiplicationFactor;
            return true;
        }
    }
}
