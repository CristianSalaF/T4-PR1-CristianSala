using System.Xml.Linq;

namespace T4_PR1_CristianSala.Model
{
    public class WaterUsageFileManager
    {
        private const string WaterUsageFilePath = "ModelData/consum_aigua_cat_per_comarques.csv";
        private const string WaterUsageXmlPath = "ModelData/consum_aigua_cat_per_comarques.xml";

        public WaterUsageFileManager()
        {
            FileExistsOrCreateXml();
        }

        private void FileExistsOrCreateXml()
        {
            string? directory = Path.GetDirectoryName(WaterUsageXmlPath);
            if (!Directory.Exists(directory))
            {
                if (directory != null) Directory.CreateDirectory(directory);
            }
            
            if (!File.Exists(WaterUsageXmlPath))
            {
                // Create XML structure if file doesn't exist
                XDocument doc = new XDocument(
                    new XDeclaration("1.0", "utf-8", "yes"),
                    new XElement("Consums")
                );
                doc.Save(WaterUsageXmlPath);
            }
        }

        public List<WaterUsage> LoadUsages()
        {
            var usages = new List<WaterUsage>();
            
            try
            {
                if (File.Exists(WaterUsageFilePath))
                {
                    // Skip header line
                    var lines = File.ReadAllLines(WaterUsageFilePath).Skip(1);
                    
                    foreach (string line in lines)
                    {
                        string[] parts = line.Split(',');
                        if (parts.Length < 8) continue;
                        
                        var consum = new WaterUsage
                        {
                            Any = int.Parse(parts[0]),
                            CodiComarca = int.Parse(parts[1]),
                            Comarca = parts[2],
                            Poblacio = int.Parse(parts[3]),
                            DomesticXarxa = int.Parse(parts[4]),
                            ActivitatsEconomiquesIFontsPropies = int.Parse(parts[5]),
                            Total = int.Parse(parts[6]),
                            ConsumDomesticPerCapita = double.Parse(parts[7].Replace('.', ','))
                        };
                        
                        usages.Add(consum);
                    }
                }

                if (File.Exists(WaterUsageXmlPath))
                {
                    XDocument doc = XDocument.Load(WaterUsageXmlPath);
                    foreach (var element in doc.Root.Elements("Consum"))
                    {
                        var consum = new WaterUsage
                        {
                            Any = int.Parse(element.Element("Any").Value),
                            CodiComarca = int.Parse(element.Element("CodiComarca").Value),
                            Comarca = element.Element("Comarca").Value,
                            Poblacio = int.Parse(element.Element("Poblacio").Value),
                            DomesticXarxa = int.Parse(element.Element("DomesticXarxa").Value),
                            ActivitatsEconomiquesIFontsPropies = int.Parse(element.Element("ActivitatsEconomiques").Value),
                            Total = int.Parse(element.Element("Total").Value),
                            ConsumDomesticPerCapita = double.Parse(element.Element("ConsumPerCapita").Value.Replace('.', ','))
                        };
                        
                        usages.Add(consum);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading water consumption data: {ex.Message}");
            }
            
            return usages;
        }

        public void SaveUsage(WaterUsage usage)
        {
            try
            {
                XDocument doc;
                if (File.Exists(WaterUsageXmlPath))
                {
                    doc = XDocument.Load(WaterUsageXmlPath);
                }
                else
                {
                    doc = new XDocument(
                        new XDeclaration("1.0", "utf-8", "yes"),
                        new XElement("Consums")
                    );
                }

                XElement newUsage = new XElement("Consum",
                    new XElement("Any", usage.Any),
                    new XElement("CodiComarca", usage.CodiComarca),
                    new XElement("Comarca", usage.Comarca),
                    new XElement("Poblacio", usage.Poblacio),
                    new XElement("DomesticXarxa", usage.DomesticXarxa),
                    new XElement("ActivitatsEconomiques", usage.ActivitatsEconomiquesIFontsPropies),
                    new XElement("Total", usage.Total),
                    new XElement("ConsumPerCapita", usage.ConsumDomesticPerCapita.ToString().Replace(',', '.'))
                );

                doc.Root.Add(newUsage);
                doc.Save(WaterUsageXmlPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving water consumption: {ex.Message}");
            }
        }

        public List<WaterUsage> GetTop10MunicipisWithHighestConsum()
        {
            var usages = LoadUsages();
            int lastYear = usages.Max(c => c.Any);
            
            return usages
                .Where(c => c.Any == lastYear)
                .OrderByDescending(c => c.Total)
                .Take(10)
                .ToList();
        }

        public List<dynamic> GetAverageUsageByComarca()
        {
            var usages = LoadUsages();
            
            return usages
                .GroupBy(c => c.Comarca)
                .Select(g => new 
                {
                    Comarca = g.Key,
                    AverageConsum = g.Average(c => c.Total)
                })
                .OrderByDescending(c => c.AverageConsum)
                .ToList<dynamic>();
        }

        public List<WaterUsage> GetSuspiciousUsageValues()
        {
            var usages = LoadUsages();
            
            return usages
                .Where(c => c.Total >= 1000000) // 6 digits or more
                .ToList();
        }

        public List<dynamic> GetMunicipisWithIncreasingUsageLast5Years()
        {
            var usages = LoadUsages();
            int lastYear = usages.Max(c => c.Any);
            int firstYear = lastYear - 4; // Last 5 years
            
            var result = new List<dynamic>();
            
            var municipisByComarca = usages
                .Where(c => c.Any >= firstYear && c.Any <= lastYear)
                .GroupBy(c => c.Comarca);
                
            foreach (var comarca in municipisByComarca)
            {
                var yearlyData = comarca
                    .OrderBy(c => c.Any)
                    .ToList();
                    
                // Check if there is 5 years of data
                if (yearlyData.Count == 5)
                {
                    bool isIncreasing = true;
                    for (int i = 1; i < yearlyData.Count; i++)
                    {
                        if (yearlyData[i].Total <= yearlyData[i-1].Total)
                        {
                            isIncreasing = false;
                            break;
                        }
                    }
                    
                    if (isIncreasing)
                    {
                        result.Add(new 
                        {
                            Comarca = comarca.Key,
                            ConsumsPerYear = yearlyData.Select(c => new { c.Any, c.Total }).ToList()
                        });
                    }
                }
            }
            
            return result;
        }
    }
}
