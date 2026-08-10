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
            var all = await products.GetAllAsync();
            return View(new HomeViewModel(await products.GetCategoriesAsync(), await products.GetBestSellersAsync(),
                all.Where(x => x.ExpirationDate <= DateTime.Today.AddMonths(3)).OrderBy(x => x.ExpirationDate).Take(4).ToList()));
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
