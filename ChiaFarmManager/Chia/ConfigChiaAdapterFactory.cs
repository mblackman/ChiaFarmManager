using System;
using System.IO;

using ChiaAdapter;

using Microsoft.Extensions.Configuration;

namespace ChiaFarmManager.Chia
{
    /// <summary>
    /// Creates a new <see cref="IChiaAdapter"/> from an <see cref="IConfiguration"/>.
    /// </summary>
    public class ConfigChiaAdapterFactory : IChiaAdapterFactory
    {
        private readonly string chiaPath;

        /// <summary>
        /// Creates a new instance of <see cref="ConfigChiaAdapterFactory"/>.
        /// </summary>
        /// <param name="configuration">The current configuration.</param>
        public ConfigChiaAdapterFactory(IConfiguration configuration)
        {
            var applicationPath = configuration.GetValue<string>("ExePath");

            if (applicationPath != null)
            {
                if (!File.Exists(applicationPath))
                {
                    throw new ArgumentException("Chia exe path not found. " + applicationPath);
                }

                chiaPath = applicationPath;
            }
            else
            {
                throw new InvalidOperationException("Chia application path must be set.");
            }
        }

        /// <summary>
        /// Creates the new <see cref="IChiaAdapter"/>.
        /// </summary>
        /// <returns>The new <see cref="IChiaAdapter"/>.</returns>
        public IChiaAdapter Create() => new CliChiaAdapter(new ProcessChiaClient(chiaPath));
    }
}
