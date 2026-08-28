using PharmaSupply.Data;

namespace PharmaSupply.Services.Pricing;

public sealed record PriceBreakdown(decimal Subtotal, decimal Discount, decimal Tax, decimal Total);
public interface IPricingStrategy
{
    PriceBreakdown Calculate(decimal unitPrice, int quantity);
}

public sealed class SgkMedicinePricingStrategy : IPricingStrategy
{
    public PriceBreakdown Calculate(decimal unitPrice, int quantity)
    {
        var subtotal = unitPrice * quantity;
        var discount = subtotal * .12m;
        var tax = (subtotal - discount) * .10m;
        return new(subtotal, discount, tax, subtotal - discount + tax);
    }
}

public sealed class SupplementPricingStrategy : IPricingStrategy
{
    public PriceBreakdown Calculate(decimal unitPrice, int quantity)
    {
        var subtotal = unitPrice * quantity;
        var discount = quantity >= 10 ? subtotal * .05m : 0;
        var tax = (subtotal - discount) * .20m;
        return new(subtotal, discount, tax, subtotal - discount + tax);
    }
}

public sealed class PricingStrategyFactory
{
    private readonly IPricingStrategy _sgk = new SgkMedicinePricingStrategy();
    private readonly IPricingStrategy _supplement = new SupplementPricingStrategy();
    public IPricingStrategy Resolve(ProductKind kind) => kind == ProductKind.SgkMedicine ? _sgk : _supplement;
}
