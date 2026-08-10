using Microsoft.EntityFrameworkCore;
using PharmaSupply.Data;

namespace PharmaSupply.Services;

public interface IPharmacyService
{
    Task<IReadOnlyList<Pharmacy>> GetAllAsync();
    Task<Pharmacy?> GetAsync(int id);
    Task SaveAsync(Pharmacy pharmacy);
}
public sealed class PharmacyService(AppDbContext db) : IPharmacyService
{
    public async Task<IReadOnlyList<Pharmacy>> GetAllAsync() => await db.Pharmacies.AsNoTracking().OrderBy(x => x.Name).ToListAsync();
    public Task<Pharmacy?> GetAsync(int id) => db.Pharmacies.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
    public async Task SaveAsync(Pharmacy pharmacy)
    {
        if (pharmacy.Id == 0) db.Add(pharmacy); else db.Update(pharmacy);
        await db.SaveChangesAsync();
    }
}

public interface IOrderService
{
    Task<IReadOnlyList<Order>> GetAllAsync();
    Task UpdateStatusAsync(int id, OrderStatus status);
}
public sealed class OrderService(AppDbContext db) : IOrderService
{
    public async Task<IReadOnlyList<Order>> GetAllAsync() => await db.Orders.AsNoTracking().Include(x => x.Pharmacy)
        .Include(x => x.Items).ThenInclude(x => x.Product).OrderByDescending(x => x.CreatedAt).ToListAsync();
    public async Task UpdateStatusAsync(int id, OrderStatus status)
    {
        var order = await db.Orders.FindAsync(id);
        if (order is null) return;
        order.Status = status; await db.SaveChangesAsync();
    }
}
