using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace T4_PR1_CristianSala.Pages
{
    public class ViewSimulationsModel : PageModel
    {
        /*for (int i = 0; i < indexSimulacio; i++)
        {
            var sistema = simulacions[i];
            Console.WriteLine($"{sistema.Nom}\t{sistema.EnergiaGenerada:F2}");
        }*/

        /*public string FileErrorMessage;
        public List<Product> MyProducts { get; set; } = new List<Product>();
        public void OnGet()
        {
            string filePath = Path.GetFullPath(@"ModelData\products.txt");
            Debug.WriteLine(Path.GetFullPath(filePath));

            if (FileWorking.File.Exists(filePath))
            {
                string[] lines = FileWorking.File.ReadAllLines(filePath);

                foreach (string line in lines) 
                {
                    string[] parts = line.Split('|');

                    if(parts.Length == 4) 
                    {
                        Product product = new Product();
                        product.Id = int.Parse(parts[0]);
                        product.Name = parts[1];
                        product.Quantity = int.Parse(parts[2]);
                        product.Price = decimal.Parse(parts[3],CultureInfo.InvariantCulture);
                        MyProducts.Add(product);
                    }
                    else FileErrorMessage = "Error de càrrega dels atributs d'un producte";
                }
            }
            else FileErrorMessage = "Error de càrrega de dades.";
        }*/
    }
}
