using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PharmaSupply.Data;
using PharmaSupply.Models;
using PharmaSupply.Services;

namespace PharmaSupply.Controllers;

[Route("admin/[action]")]
public sealed class AdminController(
    AppDbContext db,
    IProductService products,
    IPharmacyService pharmacies,
    IOrderService orders) : Controller
{
    public async Task<IActionResult> Index()
    {
        var start = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
        var latestOrders = await db.Orders
            .AsNoTracking()
            .Include(x => x.Pharmacy)
            .OrderByDescending(x => x.CreatedAt)
            .Take(6)
            .ToListAsync();
        var notifications = await db.AdminNotifications
            .AsNoTracking()
            .OrderByDescending(x => x.CreatedAt)
            .Take(6)
            .ToListAsync();

        var viewModel = new DashboardViewModel(
            await db.Products.CountAsync(),
            await db.Pharmacies.CountAsync(),
            await db.Orders.CountAsync(x =>
                x.Status == OrderStatus.Preparing || x.Status == OrderStatus.Shipped),
            await db.Orders
                .Where(x => x.CreatedAt >= start)
                .SumAsync(x => (decimal?)x.Total) ?? 0,
            latestOrders,
            notifications);

        return View(viewModel);
    }

    public async Task<IActionResult> Products()
    {
        return View(await products.GetAllAsync());
    }

    [HttpGet]
    public async Task<IActionResult> ProductForm(int? id)
    {
        ViewBag.Categories = await products.GetCategoriesAsync();

        if (id is null)
        {
            return View(new Product { ExpirationDate = DateTime.Today.AddYears(1) });
        }

        var product = await products.GetByIdAsync(id.Value);
        return product is null ? NotFound() : View(product);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> ProductForm(Product product)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Categories = await products.GetCategoriesAsync();
            return View(product);
        }

        if (product.Id == 0)
        {
            await products.AddAsync(product);
        }
        else
        {
            await products.UpdateAsync(product);
        }

        return RedirectToAction(nameof(Products));
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        await products.DeleteAsync(id);
        return RedirectToAction(nameof(Products));
    }

    public async Task<IActionResult> Pharmacies()
    {
        return View(await pharmacies.GetAllAsync());
    }

    [HttpGet]
    public async Task<IActionResult> PharmacyForm(int? id)
    {
        if (id is null)
        {
            return View(new Pharmacy());
        }

        var pharmacy = await pharmacies.GetAsync(id.Value);
        return pharmacy is null ? NotFound() : View(pharmacy);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> PharmacyForm(Pharmacy pharmacy)
    {
        if (!ModelState.IsValid)
        {
            return View(pharmacy);
        }

        await pharmacies.SaveAsync(pharmacy);
        return RedirectToAction(nameof(Pharmacies));
    }

    public async Task<IActionResult> Orders()
    {
        return View(await orders.GetAllAsync());
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateOrder(int id, OrderStatus status)
    {
        await orders.UpdateStatusAsync(id, status);
        return RedirectToAction(nameof(Orders));
    }
}
