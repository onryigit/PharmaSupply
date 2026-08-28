using Microsoft.AspNetCore.Mvc;
using PharmaSupply.Models;
using PharmaSupply.Services;
using PharmaSupply.Data;
using System.Diagnostics;

namespace PharmaSupply.Controllers
{
    public class HomeController(IProductService products) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var allProducts = await products.GetAllAsync();
            var categories = await products.GetCategoriesAsync();
            var bestSellers = await products.GetBestSellersAsync();
            var expiringSoon = allProducts
                .Where(x => x.ExpirationDate <= DateTime.Today.AddMonths(3))
                .OrderBy(x => x.ExpirationDate)
                .Take(4)
                .ToList();

            var viewModel = new HomeViewModel(
                categories,
                bestSellers,
                expiringSoon,
                allProducts.Count);

            return View(viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
