using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using T4_PR1_CristianSala.Model;

namespace T4_PR1_CristianSala.Pages
{
    public class SimulationsModel : PageModel
    {
        private readonly SimulationFileManager _fileManager;

        public SimulationsModel()
        {
            _fileManager = new SimulationFileManager();
            Simulations = new List<BaseSimulation>();
        }

        public List<BaseSimulation> Simulations { get; set; }

        public void OnGet()
        {
            Simulations = _fileManager.LoadSimulations();
        }
    }
}
