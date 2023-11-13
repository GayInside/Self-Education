namespace LanguageFeatures.Models
{
    public class Product
    {
        public string Name { get; set; }
        public decimal? Price { get; set; }
        public string Category { get; set; } = "Watersports";
        public Product Related { get; set; }
        public bool InStock { get; }

        public Product(bool stock = true)
        {
            InStock = stock;
        }
        public static Product[] GetProducts()
        {
            Product kayak = new Product { Name = "kayak", Price = 275m, Category = "Water Craft" };

            Product lifejacket = new Product(false) { Name = "Lifejacket", Price = 48.95m };

            kayak.Related = lifejacket;
            return new Product[] { kayak, lifejacket, null };
        }
    }
}
