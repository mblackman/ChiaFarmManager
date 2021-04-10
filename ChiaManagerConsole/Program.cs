﻿using System;
using System.Threading;

using ChiaAdapter;

using ChiaFarmManager;

using Common;

using Microsoft.Extensions.CommandLineUtils;

namespace ChiaManagerConsole
{
    class Program
    {
        private readonly static CancellationTokenSource plottingCancellationToken = new CancellationTokenSource();

        static void Main(string[] args)
        {
            CommandLineApplication commandLineApplication = new(throwOnUnexpectedArg: false);

            CommandOption tempDirectories = commandLineApplication.Option(
              "-t|--temp <temp-directories>",
              "Set the list of directories to use as temp plotting space.",
              CommandOptionType.MultipleValue);

            CommandOption destinationDirectories = commandLineApplication.Option(
              "-d|--dest <destination-directories>",
              "Set the list of directories to put final plots.",
              CommandOptionType.MultipleValue);

            CommandOption chiaExe = commandLineApplication.Option(
              "-e|--exe <chia-exe>",
              "The location of the local Chia executable.",
              CommandOptionType.SingleValue);

            CommandOption kSize = commandLineApplication.Option(
              "-k <k-size>",
              "The size of plots to make.",
              CommandOptionType.SingleValue);

            commandLineApplication.HelpOption("-? | -h | --help");

            commandLineApplication.OnExecute(async () =>
            {
                if (!tempDirectories.HasValue())
                {
                    Console.WriteLine("Temp directory required.");
                }
                else if (!destinationDirectories.HasValue())
                {
                    Console.WriteLine("Destination directory required.");
                }
                else if (chiaExe.HasValue())
                {
                    var adapter = new CliChiaAdapter(new ProcessChiaClient(chiaExe.Value()));
                    var logger = new ConsoleLogger<PlottingManager>();
                    var options = new PlottingManagerOptions();

                    if (kSize.HasValue() && int.TryParse(kSize.Value(), out int setSize))
                    {
                        options.KSize = setSize;
                    }

                    var manager = new PlottingManager(logger, adapter, tempDirectories.Values, destinationDirectories.Values, options);
                    await manager.Plot(plottingCancellationToken.Token);
                }

                return 0;
            });
            commandLineApplication.Execute(args);
        }

        static void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            plottingCancellationToken.Cancel();
        }
    }
}
