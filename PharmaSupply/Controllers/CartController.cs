using Microsoft.AspNetCore.Mvc;
using PharmaSupply.Models;
using PharmaSupply.Services;
using PharmaSupply.Services.Checkout;

namespace PharmaSupply.Controllers;

public sealed class CartController(IPharmacyService pharmacies, ICartService cartService, ICheckoutService checkout) : Controller
{
    private const int CurrentPharmacyId = 1;
    public async Task<IActionResult> Index()
    {
        var pharmacy = await pharmacies.GetAsync(CurrentPharmacyId);
        if (pharmacy is null) return NotFound("Varsayılan eczane bulunamadı.");
        var cart = await cartService.GetAsync(CurrentPharmacyId);
        var lines = cart.Where(x => x.Product is not null).Select(x => new CartLine(x.ProductId, x.Product!.Name,
            x.Product.ImageUrl, x.Product.UnitPrice, x.Quantity, x.Product.Stock)).ToList();
        return View(new CartViewModel(lines, pharmacy));
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Add(int productId, int quantity = 1)
    {
        await cartService.AddAsync(CurrentPharmacyId, productId, Math.Max(1, quantity));
        TempData["Success"] = "Ürün sepete eklendi.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(int productId, int quantity)
    {
        await cartService.UpdateAsync(CurrentPharmacyId, productId, quantity);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Checkout()
    {
        var cart = await cartService.GetAsync(CurrentPharmacyId);
        var result = await checkout.CheckoutAsync(CurrentPharmacyId, cart.ToDictionary(x => x.ProductId, x => x.Quantity));
        TempData[result.Success ? "Success" : "Error"] = result.Message;
        if (result.Success) await cartService.ClearAsync(CurrentPharmacyId);
        return RedirectToAction(nameof(Index));
    }
}
