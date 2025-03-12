namespace T4_PR1_CristianSala.Model
{
    public class SimulationFileManager
    {
        private const string SimulationsFilePath = "ModelData/simulacions_energia.csv";

        public SimulationFileManager()
    {
        FileExistsOrDefault();
    }

        /// <summary>
        /// Checks if the file exists, if not, creates it with the header line
        /// </summary>
        private void FileExistsOrDefault()
    {
        string? directory = Path.GetDirectoryName(SimulationsFilePath);
        if (!Directory.Exists(directory))
        {
            if (directory != null) Directory.CreateDirectory(directory);
        }
        
        if (!File.Exists(SimulationsFilePath))
        {
            using (var writer = new StreamWriter(SimulationsFilePath))
            {
                writer.WriteLine("Data,Tipus,Parametre,Valor,Rati,EnergiaGenerada,CostPerKWh,PreuPerKWh,CostTotal,PreuTotal");
            }
        }
    }

        /// <summary>
        /// Loads all simulations from the file
        /// </summary>
        /// <returns></returns>
        public List<BaseSimulation> LoadSimulations()
    {
        var simulations = new List<BaseSimulation>();
        
        try
        {
            if (File.Exists(SimulationsFilePath))
            {
                //this should skip the header line
                var lines = File.ReadAllLines(SimulationsFilePath).Skip(1);
                
                foreach (string line in lines)
                {
                    string[] parts = line.Split(',');
                    if (parts.Length < 10) continue;
                    
                    BaseSimulation model;
                    
                    //column for the Type
                    switch (parts[1])
                    {
                        case "Sistema Solar":
                            model = new SolarEnergy();
                            break;
                        case "Sistema Eòlic":
                            model = new WindEnergy();
                            break;
                        case "Sistema Hidroelèctric":
                            model = new HydroEnergy();
                            break;
                        default:
                            //skips unknown types
                            continue;
                    }
                    
                    model.SimulationDate = DateTime.Parse(parts[0]);
                    model.Type = parts[1];
                    model.ParameterValue = double.Parse(parts[3]);
                    model.Ratio = double.Parse(parts[4]);
                    model.EnergyGenerated = double.Parse(parts[5]);
                    model.CostPerKWh = double.Parse(parts[6]);
                    model.PricePerKWh = double.Parse(parts[7]);
                    
                    simulations.Add(model);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading simulations: {ex.Message}");
        }
        
        return simulations;
    }

        /// <summary>
        /// Saves a simulation to the file
        /// </summary>
        /// <param name="simulation"></param>
        public void SaveSimulation(BaseSimulation simulation)
    {
        try
        {
            string line = $"{simulation.SimulationDate:yyyy-MM-dd HH:mm:ss}," +
                      $"{simulation.Type}," +
                      $"{simulation.ParameterName}," +
                      $"{simulation.ParameterValue}," +
                      $"{simulation.Ratio}," +
                      $"{simulation.EnergyGenerated}," +
                      $"{simulation.CostPerKWh}," +
                      $"{simulation.PricePerKWh}," +
                      $"{simulation.TotalCost}," +
                      $"{simulation.TotalPrice}";
            
            File.AppendAllLines(SimulationsFilePath, [line]);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving simulation: {ex.Message}");
        }
    }
    }
}
