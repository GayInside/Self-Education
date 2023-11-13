using LanguageFeatures.Models;
using Microsoft.AspNetCore.Mvc;

namespace LanguageFeatures.Controllers
{
    public class HomeController : Controller
    {
        public ViewResult Index()
        {
            Dictionary<string, Product> products = new Dictionary<string, Product>
            {
                { "Kayak", new Product {Name = "Kayak", Price = 275m} },
                {"Lifejacket", new Product{Name = "Lifejacket", Price = 48.95m } }
            };
            return View("Index", products.Keys);
        }
    }
}
