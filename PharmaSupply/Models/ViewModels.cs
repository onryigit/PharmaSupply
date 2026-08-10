using PharmaSupply.Data;

namespace PharmaSupply.Models;

public sealed record ProductFilter(string? Ingredient, PrescriptionType? Prescription, bool? InStock, int? CategoryId,
    string? Search = null, string? Sort = null);
public sealed record CartLine(int ProductId, string Name, string ImageUrl, decimal UnitPrice, int Quantity, int Stock)
{
    public decimal Total => UnitPrice * Quantity;
}
public sealed record CartViewModel(IReadOnlyList<CartLine> Lines, Pharmacy Pharmacy)
{
    public decimal Total => Lines.Sum(x => x.Total);
}
public sealed record HomeViewModel(IReadOnlyList<Category> Categories, IReadOnlyList<Product> BestSellers,
    IReadOnlyList<Product> ExpiringSoon);
public sealed record ShopViewModel(IReadOnlyList<Product> Products, IReadOnlyList<string> Ingredients,
    ProductFilter Filter);
public sealed record DashboardViewModel(int ProductCount, int PharmacyCount, int OpenOrderCount,
    decimal MonthlyRevenue, IReadOnlyList<Order> LatestOrders, IReadOnlyList<AdminNotification> Notifications);

public sealed class CheckoutRequest
{
    public required Pharmacy Pharmacy { get; init; }
    public required IReadOnlyList<(Product Product, int Quantity)> Lines { get; init; }
    public decimal Total { get; set; }
    public int RedPrescriptionQuantity => Lines.Where(x => x.Product.PrescriptionType == PrescriptionType.Red)
        .Sum(x => x.Quantity);
}

public sealed record CheckoutResult(bool Success, string Message, int? OrderId = null);
