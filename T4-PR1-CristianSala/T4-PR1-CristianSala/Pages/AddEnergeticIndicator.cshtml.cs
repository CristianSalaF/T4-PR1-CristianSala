using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using T4_PR1_CristianSala.Model;

namespace T4_PR1_CristianSala.Pages
{
    public class AddEnergeticIndicatorModel : PageModel
    {
        private readonly EnergeticIndicatorFileManager _manager = new();

        [BindProperty]
        public EnergeticIndicatorInputModel EnergeticIndicatorInput { get; set; } = new();

        public class EnergeticIndicatorInputModel
        {
            [Required(ErrorMessage = "L'any és obligatori")]
            public string Data { get; set; } = "2024";

            [Required(ErrorMessage = "El valor és obligatori")]
            [Range(0, double.MaxValue, ErrorMessage = "El valor ha de ser positiu")]
            public double PBEE_Hidroelectr { get; set; }

            [Required(ErrorMessage = "El valor és obligatori")]
            [Range(0, double.MaxValue, ErrorMessage = "El valor ha de ser positiu")]
            public double PBEE_Carbo { get; set; }

            [Required(ErrorMessage = "El valor és obligatori")]
            [Range(0, double.MaxValue, ErrorMessage = "El valor ha de ser positiu")]
            public double PBEE_GasNat { get; set; }

            [Required(ErrorMessage = "El valor és obligatori")]
            [Range(0, double.MaxValue, ErrorMessage = "El valor ha de ser positiu")]
            public double PBEE_FuelOil { get; set; }

            [Required(ErrorMessage = "El valor és obligatori")]
            [Range(0, double.MaxValue, ErrorMessage = "El valor ha de ser positiu")]
            public double PBEE_CiclComb { get; set; }

            [Required(ErrorMessage = "El valor és obligatori")]
            [Range(0, double.MaxValue, ErrorMessage = "El valor ha de ser positiu")]
            public double PBEE_Nuclear { get; set; }

            [Required(ErrorMessage = "El valor és obligatori")]
            [Range(0, double.MaxValue, ErrorMessage = "El valor ha de ser positiu")]
            public double CDEEBC_ProdBruta { get; set; }

            [Required(ErrorMessage = "El valor és obligatori")]
            [Range(0, double.MaxValue, ErrorMessage = "El valor ha de ser positiu")]
            public double CDEEBC_ConsumAux { get; set; }

            [Required(ErrorMessage = "El valor és obligatori")]
            [Range(0, double.MaxValue, ErrorMessage = "El valor ha de ser positiu")]
            public double CDEEBC_ProdNeta { get; set; }

            [Required(ErrorMessage = "El valor és obligatori")]
            [Range(0, double.MaxValue, ErrorMessage = "El valor ha de ser positiu")]
            public double CDEEBC_ProdDisp { get; set; }

            [Required(ErrorMessage = "El valor és obligatori")]
            [Range(0, double.MaxValue, ErrorMessage = "El valor ha de ser positiu")]
            public double CDEEBC_DemandaElectr { get; set; }

            [Required(ErrorMessage = "El valor és obligatori")]
            [Range(0, double.MaxValue, ErrorMessage = "El valor ha de ser positiu")]
            public double CCAC_GasolinaAuto { get; set; }
        }
        /// <summary>
        /// Saves the energetic indicator to the file
        /// </summary>
        /// <returns></returns>
        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var indicator = new EnergeticIndicator
            {
                Data = EnergeticIndicatorInput.Data,
                PBEE_Hidroelectr = EnergeticIndicatorInput.PBEE_Hidroelectr,
                PBEE_Carbo = EnergeticIndicatorInput.PBEE_Carbo,
                PBEE_GasNat = EnergeticIndicatorInput.PBEE_GasNat,
                PBEE_FuelOil = EnergeticIndicatorInput.PBEE_FuelOil,
                PBEE_CiclComb = EnergeticIndicatorInput.PBEE_CiclComb,
                PBEE_Nuclear = EnergeticIndicatorInput.PBEE_Nuclear,
                CDEEBC_ProdBruta = EnergeticIndicatorInput.CDEEBC_ProdBruta,
                CDEEBC_ConsumAux = EnergeticIndicatorInput.CDEEBC_ConsumAux,
                CDEEBC_ProdNeta = EnergeticIndicatorInput.CDEEBC_ProdNeta,
                CDEEBC_ProdDisp = EnergeticIndicatorInput.CDEEBC_ProdDisp,
                CDEEBC_DemandaElectr = EnergeticIndicatorInput.CDEEBC_DemandaElectr,
                CCAC_GasolinaAuto = EnergeticIndicatorInput.CCAC_GasolinaAuto,
                
                // Set defaults for other properties not in the form
                CDEEBC_ConsumBomb = 0,
                CDEEBC_TotVendesXarxaCentral = 0,
                CDEEBC_SaldoIntercanviElectr = 0,
                CDEEBC_TotalEBCMercatRegulat = "",
                CDEEBC_TotalEBCMercatLliure = "",
                FEE_Industria = 0,
                FEE_Terciari = 0,
                FEE_Domestic = 0,
                FEE_Primari = 0,
                FEE_Energetic = 0,
                FEEI_ConsObrPub = 0,
                FEEI_SiderFoneria = 0,
                FEEI_Metalurgia = 0,
                FEEI_IndusVidre = 0,
                FEEI_CimentsCalGuix = 0,
                FEEI_AltresMatConstr = 0,
                FEEI_QuimPetroquim = 0,
                FEEI_ConstrMedTrans = 0,
                FEEI_RestaTransforMetal = 0,
                FEEI_AlimBegudaTabac = 0,
                FEEI_TextilConfecCuirCalcat = 0,
                FEEI_PastaPaperCartro = 0,
                FEEI_AltresIndus = 0,
                DGGN_PuntFrontEnagas = 0,
                DGGN_DistrAlimGNL = 0,
                DGGN_ConsumGNCentrTerm = 0,
                CCAC_GasoilA = 0
            };

            _manager.SaveIndicator(indicator);

            return RedirectToPage("./EnergeticIndicators");
        }
    }
}
