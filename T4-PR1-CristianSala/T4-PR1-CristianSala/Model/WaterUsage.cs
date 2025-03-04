namespace T4_PR1_CristianSala.Model
{
    public class WaterUsage
    { public int Any { get; set; }
        public int CodiComarca { get; set; }
        public string Comarca { get; set; } = string.Empty;
        public int Poblacio { get; set; }
        public int DomesticXarxa { get; set; }
        public int ActivitatsEconomiquesIFontsPropies { get; set; }
        public int Total { get; set; }
        public double ConsumDomesticPerCapita { get; set; }
    }
}
