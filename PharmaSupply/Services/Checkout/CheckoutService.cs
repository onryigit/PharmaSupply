using Microsoft.EntityFrameworkCore;
using PharmaSupply.Data;
using PharmaSupply.Models;
using PharmaSupply.Services.Notifications;
using PharmaSupply.Services.Pricing;

namespace PharmaSupply.Services.Checkout;

public interface ICheckoutService { Task<CheckoutResult> CheckoutAsync(int pharmacyId, IReadOnlyDictionary<int, int> cart); }

public sealed class CheckoutService(
    IUnitOfWork unitOfWork,
    PricingStrategyFactory pricingFactory,
    LicenseValidationHandler license,
    RedPrescriptionQuotaHandler quota,
    BalanceValidationHandler balance,
    IStockObserver stockObserver) : ICheckoutService
{
    public async Task<CheckoutResult> CheckoutAsync(int pharmacyId, IReadOnlyDictionary<int, int> cart)
    {
        if (cart.Count == 0) return new(false, "Sepetiniz boş.");
        var db = unitOfWork.Context;
        var pharmacy = await db.Pharmacies.FindAsync(pharmacyId);
        var products = await db.Products.Where(x => cart.Keys.Contains(x.Id)).ToListAsync();
        if (pharmacy is null || products.Count != cart.Count) return new(false, "Eczane veya ürün bilgisi bulunamadı.");
        if (products.Any(x => cart[x.Id] <= 0 || x.Stock < cart[x.Id])) return new(false, "Bir veya daha fazla ürün için stok yetersiz.");

        var request = new CheckoutRequest
        {
            Pharmacy = pharmacy,
            Lines = products.Select(x => (x, cart[x.Id])).ToList()
        };
        var prices = request.Lines.Select(x => (Line: x, Price: pricingFactory.Resolve(x.Product.Kind)
            .Calculate(x.Product.UnitPrice, x.Quantity))).ToList();
        request.Total = prices.Sum(x => x.Price.Total);

        license.SetNext(quota).SetNext(balance);
        var validationError = license.Validate(request);
        if (validationError is not null) return new(false, validationError);

        var order = new Order
        {
            PharmacyId = pharmacy.Id,
            Subtotal = prices.Sum(x => x.Price.Subtotal), Discount = prices.Sum(x => x.Price.Discount),
            Tax = prices.Sum(x => x.Price.Tax), Total = request.Total,
            Items = prices.Select(x => new OrderItem
            {
                ProductId = x.Line.Product.Id, Quantity = x.Line.Quantity, UnitPrice = x.Line.Product.UnitPrice,
                Discount = x.Price.Discount, Tax = x.Price.Tax
            }).ToList()
        };

        await unitOfWork.ExecuteInTransactionAsync(() =>
        {
            db.Orders.Add(order);
            foreach (var line in request.Lines)
            {
                line.Product.Stock -= line.Quantity;
                stockObserver.Inspect(line.Product);
            }
            pharmacy.Balance -= request.Total;
            pharmacy.UsedRedPrescriptionQuota += request.RedPrescriptionQuantity;
            return Task.CompletedTask;
        });
        return new(true, "Siparişiniz güvenle oluşturuldu.", order.Id);
    }
}
