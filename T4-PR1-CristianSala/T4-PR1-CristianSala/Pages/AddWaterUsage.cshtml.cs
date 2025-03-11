using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using T4_PR1_CristianSala.Model;

namespace T4_PR1_CristianSala.Pages
{
    public class AddWaterUsageModel : PageModel
    {
        private readonly WaterUsageFileManager _manager;

        public AddWaterUsageModel()
        {
            _manager = new WaterUsageFileManager();
            UsageInputModel = new WaterUsageInputModel();
        }

        [BindProperty]
        public WaterUsageInputModel UsageInputModel { get; set; }

        public class WaterUsageInputModel
        {
            [Required(ErrorMessage = "L'any és obligatori")]
            [Range(1900, 3000, ErrorMessage = "L'any ha de ser vàlid")]
            public int Any { get; set; } = 2024;

            [Required(ErrorMessage = "El codi de comarca és obligatori")]
            [Range(1, int.MaxValue, ErrorMessage = "El codi de comarca ha de ser un número positiu")]
            public int CodiComarca { get; set; }

            [Required(ErrorMessage = "El nom de la comarca és obligatori")]
            public string Comarca { get; set; } = string.Empty;

            [Required(ErrorMessage = "La població és obligatòria")]
            [Range(1, int.MaxValue, ErrorMessage = "La població ha de ser un número positiu")]
            public int Poblacio { get; set; }

            [Required(ErrorMessage = "El consum domèstic de xarxa és obligatori")]
            [Range(0, int.MaxValue, ErrorMessage = "El consum domèstic ha de ser un número positiu")]
            public int DomesticXarxa { get; set; }

            [Required(ErrorMessage = "El consum d'activitats econòmiques és obligatori")]
            [Range(0, int.MaxValue, ErrorMessage = "El consum d'activitats econòmiques ha de ser un número positiu")]
            public int ActivitatsEconomiquesIFontsPropies { get; set; }

            [Required(ErrorMessage = "El consum total és obligatori")]
            [Range(0, int.MaxValue, ErrorMessage = "El consum total ha de ser un número positiu")]
            public int Total { get; set; }

            [Required(ErrorMessage = "El consum domèstic per càpita és obligatori")]
            [Range(0, double.MaxValue, ErrorMessage = "El consum domèstic per càpita ha de ser un número positiu")]
            public double ConsumDomesticPerCapita { get; set; }
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var usage = new WaterUsage
            {
                Any = UsageInputModel.Any,
                CodiComarca = UsageInputModel.CodiComarca,
                Comarca = UsageInputModel.Comarca,
                Poblacio = UsageInputModel.Poblacio,
                DomesticXarxa = UsageInputModel.DomesticXarxa,
                ActivitatsEconomiquesIFontsPropies = UsageInputModel.ActivitatsEconomiquesIFontsPropies,
                Total = UsageInputModel.Total,
                ConsumDomesticPerCapita = UsageInputModel.ConsumDomesticPerCapita
            };

            _manager.SaveUsage(usage);

            return RedirectToPage("./WaterUsages");
        }
    }
}
