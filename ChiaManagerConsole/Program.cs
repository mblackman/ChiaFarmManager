using System;
using System.Threading;
using System.Threading.Tasks;

using ChiaAdapter;

using ChiaFarmManager;

using Common;

namespace ChiaManagerConsole
{
    class Program
    {
        private readonly static CancellationTokenSource plottingCancellationToken = new CancellationTokenSource();

        static async Task Main(string[] args)
        {
            var adapter = new CliChiaAdapter(new ProcessChiaClient(@"C:\Users\mateo\AppData\Local\chia-blockchain\app-1.0.3\resources\app.asar.unpacked\daemon\chia.exe"));
            var logger = new ConsoleLogger<PlottingManager>();
            var tempFolders = new[] { @"G:\t1", @"G:\t2", @"H:\t3", @"E:\Chia\t4" };
            var destinationFolders = new[] { @"Y:\", @"Z:\" };
            var manager = new PlottingManager(logger, adapter, tempFolders, destinationFolders);
            await manager.Plot(plottingCancellationToken.Token);
        }

        static void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            plottingCancellationToken.Cancel();
        }
    }
}
