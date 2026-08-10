using Microsoft.Extensions.Caching.Memory;
using PharmaSupply.Data;
using PharmaSupply.Models;

namespace PharmaSupply.Services.Caching;

// Decorator: reads are cached without changing the underlying product service.
public sealed class CachedProductService(IProductService inner, IMemoryCache cache) : IProductService
{
    private const string BestSellersKey = "products:best-sellers";
    public Task<IReadOnlyList<Product>> GetBestSellersAsync() => cache.GetOrCreateAsync(BestSellersKey, entry =>
    {
        entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);
        return inner.GetBestSellersAsync();
    })!;
    public Task<IReadOnlyList<Product>> GetAllAsync(ProductFilter? filter = null) => inner.GetAllAsync(filter);
    public Task<Product?> GetByIdAsync(int id) => inner.GetByIdAsync(id);
    public Task<IReadOnlyList<Category>> GetCategoriesAsync() =>
        cache.GetOrCreateAsync<IReadOnlyList<Category>>("categories", _ => inner.GetCategoriesAsync())!;
    public Task<IReadOnlyList<string>> GetIngredientsAsync() =>
        cache.GetOrCreateAsync<IReadOnlyList<string>>("ingredients", _ => inner.GetIngredientsAsync())!;
    public async Task AddAsync(Product product) { await inner.AddAsync(product); Clear(); }
    public async Task UpdateAsync(Product product) { await inner.UpdateAsync(product); Clear(); }
    public async Task DeleteAsync(int id) { await inner.DeleteAsync(id); Clear(); }
    private void Clear()
    {
        cache.Remove(BestSellersKey); cache.Remove("categories"); cache.Remove("ingredients");
    }
}
