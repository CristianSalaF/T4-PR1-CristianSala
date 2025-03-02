namespace T4_PR1_CristianSala.Model
{
    public class EnergySystemInit
    {
        /// <summary>
        /// Defaults to solar if it can not find a matching value
        /// </summary>
        /// <param name="type">Checks the value in lowercase and returns a new instance of the appropiate type or default</param>
        /// <returns></returns>
        public static BaseSimulation CreateEnergySystem(string type)
        {
            return type.ToLower() switch
            {
                "solar" => new SolarEnergy(),
                "wind" => new WindEnergy(),
                "hydro" => new HydroEnergy(),
                _ => new SolarEnergy()
            };
        }

    }
}
