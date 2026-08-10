using Microsoft.EntityFrameworkCore;
using PharmaSupply.Data;

namespace PharmaSupply.Services;

public interface ICartService
{
    Task<IReadOnlyList<CartItem>> GetAsync(int pharmacyId);
    Task AddAsync(int pharmacyId, int productId, int quantity);
    Task UpdateAsync(int pharmacyId, int productId, int quantity);
    Task ClearAsync(int pharmacyId);
}

public sealed class CartService(AppDbContext db) : ICartService
{
    public async Task<IReadOnlyList<CartItem>> GetAsync(int pharmacyId) => await db.CartItems.AsNoTracking()
        .Include(x => x.Product).Where(x => x.PharmacyId == pharmacyId).OrderBy(x => x.Id).ToListAsync();

    public async Task AddAsync(int pharmacyId, int productId, int quantity)
    {
        var product = await db.Products.FindAsync(productId);
        if (product is null || product.Stock == 0) return;
        var item = await db.CartItems.SingleOrDefaultAsync(x => x.PharmacyId == pharmacyId && x.ProductId == productId);
        if (item is null)
            db.CartItems.Add(new CartItem { PharmacyId = pharmacyId, ProductId = productId, Quantity = Math.Min(quantity, product.Stock) });
        else
            item.Quantity = Math.Min(item.Quantity + quantity, product.Stock);
        await db.SaveChangesAsync();
    }

    public async Task UpdateAsync(int pharmacyId, int productId, int quantity)
    {
        var item = await db.CartItems.Include(x => x.Product)
            .SingleOrDefaultAsync(x => x.PharmacyId == pharmacyId && x.ProductId == productId);
        if (item is null) return;
        if (quantity <= 0) db.CartItems.Remove(item);
        else item.Quantity = Math.Min(quantity, item.Product!.Stock);
        await db.SaveChangesAsync();
    }

    public async Task ClearAsync(int pharmacyId) =>
        await db.CartItems.Where(x => x.PharmacyId == pharmacyId).ExecuteDeleteAsync();
}
