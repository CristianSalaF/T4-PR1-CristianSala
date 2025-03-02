namespace T4_PR1_CristianSala.Model;

public class WindEnergy : BaseSimulation
{
    public override string ParameterName => "Velocitat del vent (m/s)";
    private double _windSpeed;
    public override double ParameterValue 
    { 
        get => _windSpeed; 
        set => _windSpeed = value; 
    }
    
    public WindEnergy()
    {
        Type = "Sistema Eòlic";
    }
    
    public void CalculateEnergy()
    {
        EnergyGenerated = Math.Pow(ParameterValue, 3) * Ratio;
    }
}