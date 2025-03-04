using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using T4_PR1_CristianSala.Model;

namespace T4_PR1_CristianSala.Pages
{
    public class EnergeticIndicatorsModel : PageModel
    {
        private readonly EnergeticIndicatorFileManager _manager = new();

        public List<EnergeticIndicator> Indicators { get; set; } = [];
        public List<EnergeticIndicator> HighProdNetaRecords { get; set; } = [];
        public List<EnergeticIndicator> HighGasolinaRecords { get; set; } = [];
        public List<dynamic> AverageProdNetaPerYear { get; set; } = [];
        public List<EnergeticIndicator> HighDemandLowProductionRecords { get; set; } = [];

        public void OnGet()
        {
            Indicators = _manager.LoadIndicators();
            HighProdNetaRecords = _manager.GetRecordsWithProdNetaGreaterThan3000();
            HighGasolinaRecords = _manager.GetRecordsWithGasolinaGreaterThan100();
            AverageProdNetaPerYear = _manager.GetAverageProdNetaPerYear();
            HighDemandLowProductionRecords = _manager.GetRecordsWithHighDemandAndLowProduction();
        }
    }
}
