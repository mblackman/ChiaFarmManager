
using System;
using System.Threading.Tasks;

using ChiaAdapter;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ChiaFarmManager.Pages.Chia
{
    public class FarmSummaryModel : PageModel
    {
        private readonly Lazy<IChiaAdapter> chiaAdapter;

        public FarmSummaryModel(IChiaAdapterFactory chiaAdapterFactory)
        {
            if (chiaAdapterFactory is null)
            {
                throw new ArgumentNullException(nameof(chiaAdapterFactory));
            }

            chiaAdapter = new Lazy<IChiaAdapter>(() => chiaAdapterFactory.Create());
        }

        [BindProperty]
        public FarmSummary FarmSummary { get; set; }

        public async Task OnGetAsync()
        {
            this.FarmSummary = await this.chiaAdapter.Value.GetFarmSummaryAsync();
        }
    }
}
