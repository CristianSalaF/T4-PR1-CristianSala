using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using T4_PR1_CristianSala.Model;

namespace T4_PR1_CristianSala.Pages
{
    public class AddSimulationModel : PageModel
    {
        private readonly SimulationFileManager _fileManager;

        public AddSimulationModel()
        {
            _fileManager = new SimulationFileManager();
            SimulationInput = new SimulationInputModel();
        }

        [BindProperty]
        public SimulationInputModel SimulationInput { get; set; }

        public class SimulationInputModel
        {
            [Required(ErrorMessage = "El tipus és obligatori")]
            public string Type { get; set; } = "solar";

            [Required(ErrorMessage = "El rati és obligatori")]
            [Range(0.01, 3, ErrorMessage = "El rati ha d'estar entre 0.01 i 3")]
            public double Ratio { get; set; } = 1;

            [Range(0, double.MaxValue, ErrorMessage = "Les hores de sol han de ser un valor positiu")]
            public double SolarHours { get; set; }

            [Range(0, double.MaxValue, ErrorMessage = "La velocitat del vent ha de ser un valor positiu")]
            public double WindSpeed { get; set; }

            [Range(0, double.MaxValue, ErrorMessage = "El cabal d'aigua ha de ser un valor positiu")]
            public double WaterFlow { get; set; }

            [Required(ErrorMessage = "El cost per kWh és obligatori")]
            [Range(0, double.MaxValue, ErrorMessage = "El cost per kWh ha de ser un valor positiu")]
            public double CostPerKWh { get; set; }

            [Required(ErrorMessage = "El preu per kWh és obligatori")]
            [Range(0, double.MaxValue, ErrorMessage = "El preu per kWh ha de ser un valor positiu")]
            public double PricePerKWh { get; set; }
        }

        public IActionResult OnPost()
        {
            
            ModelState.Clear();
            if (SimulationInput is { Type: "solar", SolarHours: <= 0 })
            {
                ModelState.AddModelError("SimulationInput.SolarHours", "Les hores de sol han de ser un valor positiu");
            }
            else if (SimulationInput is { Type: "wind", WindSpeed: <= 0 })
            {
                ModelState.AddModelError("SimulationInput.WindSpeed", "La velocitat del vent ha de ser un valor positiu");
            }
            else if (SimulationInput is { Type: "hydro", WaterFlow: <= 0 })
            {
                ModelState.AddModelError("SimulationInput.WaterFlow", "El cabal d'aigua ha de ser un valor positiu");
            }

            if (SimulationInput.Ratio is <= 0 or > 3)
            {
                ModelState.AddModelError("SimulationInput.Ratio", "El rati ha d'estar entre 0.01 i 3");
            }

            if (SimulationInput.CostPerKWh < 0)
            {
                ModelState.AddModelError("SimulationInput.CostPerKWh", "El cost per kWh ha de ser un valor positiu");
            }

            if (SimulationInput.PricePerKWh < 0)
            {
                ModelState.AddModelError("SimulationInput.PricePerKWh", "El preu per kWh ha de ser un valor positiu");
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            BaseSimulation simulation = EnergySystemInit.CreateEnergySystem(SimulationInput.Type);
            simulation.Ratio = SimulationInput.Ratio;
            simulation.CostPerKWh = SimulationInput.CostPerKWh;
            simulation.PricePerKWh = SimulationInput.PricePerKWh;

            switch (SimulationInput.Type.ToLower())
            {
                case "solar":
                    var solar = (SolarEnergy)simulation;
                    solar.ParameterValue = SimulationInput.SolarHours;
                    solar.CalculateEnergy();
                    break;
                case "wind":
                    var wind = (WindEnergy)simulation;
                    wind.ParameterValue = SimulationInput.WindSpeed;
                    wind.CalculateEnergy();
                    break;
                case "hydro":
                    var hydro = (HydroEnergy)simulation;
                    hydro.ParameterValue = SimulationInput.WaterFlow;
                    hydro.CalculateEnergy();
                    break;
            }

            _fileManager.SaveSimulation(simulation);

            return RedirectToPage("./Simulations");
        }
    }
}
