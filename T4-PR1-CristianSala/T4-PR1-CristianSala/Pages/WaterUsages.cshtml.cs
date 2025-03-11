using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using T4_PR1_CristianSala.Model;

namespace T4_PR1_CristianSala.Pages
{
    public class WaterUsagesModel : PageModel
    {
        private readonly WaterUsageFileManager _manager;

        public WaterUsagesModel()
        {
            _manager = new WaterUsageFileManager();
            Usages = new List<WaterUsage>();
            Top10Municipis = new List<WaterUsage>();
            AverageConsumByComarca = new List<dynamic>();
            SuspiciousValues = new List<WaterUsage>();
            MunicipisWithIncreasingTrend = new List<dynamic>();
        }

        public List<WaterUsage> Usages { get; set; }
        public List<WaterUsage> Top10Municipis { get; set; }
        public List<dynamic> AverageConsumByComarca { get; set; }
        public List<WaterUsage> SuspiciousValues { get; set; }
        public List<dynamic> MunicipisWithIncreasingTrend { get; set; }

        public void OnGet()
        {
            Usages = _manager.LoadUsages();
            Top10Municipis = _manager.GetTop10MunicipisWithHighestConsum();
            AverageConsumByComarca = _manager.GetAverageUsageByComarca();
            SuspiciousValues = _manager.GetSuspiciousUsageValues();
            MunicipisWithIncreasingTrend = _manager.GetMunicipisWithIncreasingUsageLast5Years();
        }
    }
}
