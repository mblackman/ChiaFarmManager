using System;
using System.Threading.Tasks;

using ChiaAdapter;

namespace ChiaFarmManager
{
    class Program
    {
        private const string chiaPath = @"C:\Users\mateo\AppData\Local\chia-blockchain\app-1.0.3\resources\app.asar.unpacked\daemon\chia.exe";

        static void Main(string[] args)
        {
            var adapter = new CliChiaAdapter(new ProcessChiaClient(chiaPath));

            Console.WriteLine("Loading.");

            Task.Run(async () =>
            {
                FarmSummary summary = await adapter.GetFarmSummaryAsync();
                Console.WriteLine(Utils.ToDisplayString(summary));
            });

            Console.ReadKey();
        }
    }
}
