using PharmaSupply.Models;

namespace PharmaSupply.Services.Checkout;

public abstract class CheckoutValidationHandler
{
    private CheckoutValidationHandler? _next;
    public CheckoutValidationHandler SetNext(CheckoutValidationHandler next) { _next = next; return next; }
    public virtual string? Validate(CheckoutRequest request) => _next?.Validate(request);
}

public sealed class LicenseValidationHandler : CheckoutValidationHandler
{
    public override string? Validate(CheckoutRequest request) => !request.Pharmacy.IsLicenseActive
        ? "Eczane lisansı aktif değil." : base.Validate(request);
}
public sealed class RedPrescriptionQuotaHandler : CheckoutValidationHandler
{
    public override string? Validate(CheckoutRequest request) =>
        request.Pharmacy.UsedRedPrescriptionQuota + request.RedPrescriptionQuantity > request.Pharmacy.MonthlyRedPrescriptionQuota
            ? "Kırmızı reçeteli ürün kotası aşılıyor." : base.Validate(request);
}
public sealed class BalanceValidationHandler : CheckoutValidationHandler
{
    public override string? Validate(CheckoutRequest request) => request.Total > request.Pharmacy.Balance + request.Pharmacy.CreditLimit
        ? "Eczane bakiyesi ve kredi limiti yetersiz." : base.Validate(request);
}
