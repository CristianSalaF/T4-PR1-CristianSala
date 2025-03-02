namespace T4_PR1_CristianSala.Model
{
    public class SolarEnergy : BaseSimulation
    {
        public override string ParameterName => "Hores de sol";
        private double _sunHours;
        public override double ParameterValue 
        { 
            get => _sunHours; 
            set => _sunHours = value; 
        }
    
        public SolarEnergy()
        {
            Type = "Sistema Solar";
        }
    
        public void CalculateEnergy()
        {
            EnergyGenerated = ParameterValue * Ratio;
        }
    }
}
