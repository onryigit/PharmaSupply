using Microsoft.EntityFrameworkCore;
using PharmaSupply.Data;
using PharmaSupply.Models;

namespace PharmaSupply.Services;

public interface IProductService
{
    Task<IReadOnlyList<Product>> GetAllAsync(ProductFilter? filter = null);
    Task<IReadOnlyList<Product>> GetBestSellersAsync();
    Task<Product?> GetByIdAsync(int id);
    Task<IReadOnlyList<Category>> GetCategoriesAsync();
    Task<IReadOnlyList<string>> GetIngredientsAsync();
    Task AddAsync(Product product);
    Task UpdateAsync(Product product);
    Task DeleteAsync(int id);
}

public sealed class ProductService(AppDbContext db) : IProductService
{
    public async Task<IReadOnlyList<Product>> GetAllAsync(ProductFilter? filter = null)
    {
        var query = db.Products.AsNoTracking().Include(x => x.Category).AsQueryable();
        if (!string.IsNullOrWhiteSpace(filter?.Ingredient)) query = query.Where(x => x.ActiveIngredient == filter.Ingredient);
        if (filter?.Prescription is not null) query = query.Where(x => x.PrescriptionType == filter.Prescription);
        if (filter?.InStock == true) query = query.Where(x => x.Stock > 0);
        if (filter?.CategoryId is not null) query = query.Where(x => x.CategoryId == filter.CategoryId);
        if (!string.IsNullOrWhiteSpace(filter?.Search))
        {
            var search = filter.Search.Trim();
            query = query.Where(x => x.Name.Contains(search) || x.ActiveIngredient.Contains(search));
        }
        query = filter?.Sort == "priceAsc" ? query.OrderBy(x => x.UnitPrice).ThenBy(x => x.Name) : query.OrderBy(x => x.Name);
        return await query.ToListAsync();
    }

    public async Task<IReadOnlyList<Product>> GetBestSellersAsync() => await db.Products.AsNoTracking()
        .Include(x => x.Category).Where(x => x.IsBestSeller).Take(8).ToListAsync();
    public Task<Product?> GetByIdAsync(int id) => db.Products.AsNoTracking().Include(x => x.Category)
        .FirstOrDefaultAsync(x => x.Id == id);
    public async Task<IReadOnlyList<Category>> GetCategoriesAsync() => await db.Categories.AsNoTracking().ToListAsync();
    public async Task<IReadOnlyList<string>> GetIngredientsAsync() => await db.Products.AsNoTracking()
        .Select(x => x.ActiveIngredient).Distinct().OrderBy(x => x).ToListAsync();
    public async Task AddAsync(Product product) { db.Add(product); await db.SaveChangesAsync(); }
    public async Task UpdateAsync(Product product) { db.Update(product); await db.SaveChangesAsync(); }
    public async Task DeleteAsync(int id)
    {
        var product = await db.Products.FindAsync(id);
        if (product is null) return;
        db.Remove(product); await db.SaveChangesAsync();
    }
}
