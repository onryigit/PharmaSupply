using PharmaSupply.Data;

namespace PharmaSupply.Services.Notifications;

public interface IStockObserver { void Inspect(Product product); }
public sealed class AdminStockObserver(AppDbContext db, ILogger<AdminStockObserver> logger) : IStockObserver
{
    public void Inspect(Product product)
    {
        var warnings = new List<string>();
        if (product.Stock < 50) warnings.Add($"{product.Name} kritik stokta ({product.Stock} kutu).");
        if (product.ExpirationDate <= DateTime.Today.AddMonths(3)) warnings.Add($"{product.Name} ürününün miadı 3 ay içinde doluyor.");
        foreach (var warning in warnings)
        {
            db.AdminNotifications.Add(new AdminNotification { Message = warning });
            logger.LogWarning("PharmaSupply alert: {Warning}", warning);
        }
    }
}
