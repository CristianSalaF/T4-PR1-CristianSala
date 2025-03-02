namespace T4_PR1_CristianSala.Model;

public class HydroEnergy : BaseSimulation
{
    public override string ParameterName => "Cabal d'aigua (m³/s)";
    private double _waterFlow;
    public override double ParameterValue 
    { 
        get => _waterFlow; 
        set => _waterFlow = value; 
    }
    
    public HydroEnergy()
    {
        Type = "Sistema Hidroelèctric";
    }
    
    public void CalculateEnergy()
    {
        EnergyGenerated = ParameterValue * 9.8 * Ratio;
    }
}