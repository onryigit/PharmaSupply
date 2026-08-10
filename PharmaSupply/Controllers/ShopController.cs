using Microsoft.AspNetCore.Mvc;
using PharmaSupply.Data;
using PharmaSupply.Models;
using PharmaSupply.Services;

namespace PharmaSupply.Controllers;

public sealed class ShopController(IProductService products) : Controller
{
    public async Task<IActionResult> Index(string? ingredient, PrescriptionType? prescription, bool? inStock,
        int? categoryId, string? q, string? sort)
    {
        var filter = new ProductFilter(ingredient, prescription, inStock, categoryId, q, sort);
        return View(new ShopViewModel(await products.GetAllAsync(filter), await products.GetIngredientsAsync(), filter));
    }
    public async Task<IActionResult> Detail(int id)
    {
        var product = await products.GetByIdAsync(id);
        return product is null ? NotFound() : View(product);
    }
}
