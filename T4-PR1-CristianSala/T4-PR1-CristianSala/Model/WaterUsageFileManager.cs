using CsvHelper;
using System.Diagnostics;
using System.Globalization;
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

        /// <summary>
        /// Checks if the file exists, if not, creates it with the header line
        /// </summary>
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

        /// <summary>
        /// Loads all water consumption data from the file
        /// </summary>
        /// <returns></returns>
        public List<WaterUsage> LoadUsages()
        {
            var usages = new List<WaterUsage>();
            
            try
            {
                if (File.Exists(WaterUsageFilePath))
                {
                    using (var reader = new StreamReader(WaterUsageFilePath))
                    using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                    {
                        // Skip the header row
                        csv.Read();
                        csv.ReadHeader();
                        
                        // Configure CSV parsing
                        csv.Context.TypeConverterOptionsCache.GetOptions<double>().CultureInfo = new CultureInfo("ca-ES");
                        
                        // Read all records
                        while (csv.Read())
                        {
                            try
                            {
                                var consum = new WaterUsage
                                {
                                    Any = csv.GetField<int>(0),
                                    CodiComarca = csv.GetField<int>(1),
                                    Comarca = csv.GetField<string>(2),
                                    Poblacio = csv.GetField<int>(3),
                                    DomesticXarxa = csv.GetField<int>(4),
                                    ActivitatsEconomiquesIFontsPropies = csv.GetField<int>(5),
                                    Total = csv.GetField<int>(6),
                                    ConsumDomesticPerCapita = csv.GetField<double>(7)
                                };
                                
                                usages.Add(consum);
                            }
                            catch
                            {
                                // Skip invalid rows
                                continue;
                            }
                        }
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

        /// <summary>
        /// Saves a water consumption record to the file
        /// </summary>
        /// <param name="usage"></param>
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

        /// <summary>
        /// Gets the top 10 municipis with the highest water consumption in the last year
        /// </summary>
        /// <returns></returns>
        public List<WaterUsage> GetTop10MunicipisWithHighestConsum()
        {
            var usages = LoadUsages();

            if (!usages.Any()) 
            {
                Debug.WriteLine("No data was loaded from CSV or XML.");
                return new List<WaterUsage>(); // Prevents crash
            }

            int lastYear = usages.Max(c => c.Any);
            
            return usages
                .Where(c => c.Any == lastYear)
                .OrderByDescending(c => c.Total)
                .Take(10)
                .ToList();
        }

        /// <summary>
        /// Gets the average water consumption by comarca
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Gets the municipis with suspicious water consumption values
        /// </summary>
        /// <returns></returns>
        public List<WaterUsage> GetSuspiciousUsageValues()
        {
            var usages = LoadUsages();
            
            return usages
                .Where(c => c.Total >= 1000000) // 6 digits or more
                .ToList();
        }

        /// <summary>
        /// Gets the comarques with increasing water consumption in the last 5 years
        /// </summary>
        /// <returns></returns>
        public List<dynamic> GetMunicipisWithIncreasingUsageLast5Years()
        {
            var usages = LoadUsages();

            if (!usages.Any()) 
            {
                Debug.WriteLine("No data was loaded from CSV or XML.");
                return new List<dynamic>(); // Prevents crash
            }

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
