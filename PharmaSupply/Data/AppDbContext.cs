using Microsoft.EntityFrameworkCore;

namespace PharmaSupply.Data;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Pharmacy> Pharmacies => Set<Pharmacy>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
    public DbSet<CartItem> CartItems => Set<CartItem>();
    public DbSet<AdminNotification> AdminNotifications => Set<AdminNotification>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>().Property(x => x.UnitPrice).HasPrecision(18, 2);
        modelBuilder.Entity<Pharmacy>().Property(x => x.Balance).HasPrecision(18, 2);
        modelBuilder.Entity<Pharmacy>().Property(x => x.CreditLimit).HasPrecision(18, 2);
        modelBuilder.Entity<CartItem>().HasIndex(x => new { x.PharmacyId, x.ProductId }).IsUnique();
        foreach (var property in modelBuilder.Model.GetEntityTypes()
                     .SelectMany(x => x.GetProperties()).Where(x => x.ClrType == typeof(decimal)))
        {
            property.SetPrecision(18);
            property.SetScale(2);
        }

        modelBuilder.Entity<Category>().HasData(
            new Category { Id = 1, Name = "Ağrı Kesiciler", Icon = "medication" },
            new Category { Id = 2, Name = "Vitamin & Takviye", Icon = "nutrition" },
            new Category { Id = 3, Name = "Dermokozmetik", Icon = "dermatology" },
            new Category { Id = 4, Name = "Medikal Ürünler", Icon = "health_and_safety" });
        modelBuilder.Entity<Pharmacy>().HasData(new Pharmacy
        {
            Id = 1, Name = "Şifa Eczanesi", LicenseNumber = "ECZ-34001", IsLicenseActive = true,
            Balance = 12500, CreditLimit = 75000, MonthlyRedPrescriptionQuota = 100
        });
        modelBuilder.Entity<Product>().HasData(
            ProductSeed(1, "Paraset 500 mg", "Parasetamol", 1, 68.50m, 240, PrescriptionType.Normal, ProductKind.SgkMedicine, true, 45),
            ProductSeed(2, "İbucold Plus", "İbuprofen", 1, 124.90m, 42, PrescriptionType.Normal, ProductKind.SgkMedicine, true, 70),
            ProductSeed(3, "Omega 3 Forte", "Omega-3", 2, 289.00m, 85, PrescriptionType.Normal, ProductKind.Supplement, true, 250),
            ProductSeed(4, "Dermacare Krem", "Dekspantenol", 3, 176.50m, 33, PrescriptionType.Normal, ProductKind.SgkMedicine, false, 320),
            ProductSeed(5, "Diazepam 5 mg", "Diazepam", 1, 92.25m, 80, PrescriptionType.Green, ProductKind.SgkMedicine, false, 500),
            ProductSeed(6, "Morfin Ampul", "Morfin", 4, 440.00m, 65, PrescriptionType.Red, ProductKind.SgkMedicine, false, 720),
            ProductSeed(7, "Naproksen Forte 550 mg", "Naproksen Sodyum", 1, 148.75m, 118, PrescriptionType.Normal, ProductKind.SgkMedicine, true, 610),
            ProductSeed(8, "Migra Relief 250 mg", "Asetilsalisilik Asit", 1, 86.40m, 76, PrescriptionType.Normal, ProductKind.SgkMedicine, false, 455),
            ProductSeed(9, "Ketoprofen Jel", "Ketoprofen", 1, 132.20m, 39, PrescriptionType.Normal, ProductKind.SgkMedicine, false, 290),
            ProductSeed(10, "Vitamin D3 1000 IU", "Kolekalsiferol", 2, 214.90m, 164, PrescriptionType.Normal, ProductKind.Supplement, true, 680),
            ProductSeed(11, "Magnezyum Complex", "Magnezyum Sitrat", 2, 319.50m, 91, PrescriptionType.Normal, ProductKind.Supplement, false, 540),
            ProductSeed(12, "Probiyotik Balance", "Lactobacillus Acidophilus", 2, 384.00m, 58, PrescriptionType.Normal, ProductKind.Supplement, true, 390),
            ProductSeed(13, "Urea Repair Losyon", "%10 Üre", 3, 268.75m, 72, PrescriptionType.Normal, ProductKind.Supplement, false, 470),
            ProductSeed(14, "Sun Protect SPF 50+", "Organik UV Filtreleri", 3, 449.90m, 105, PrescriptionType.Normal, ProductKind.Supplement, true, 520),
            ProductSeed(15, "Niacinamide Serum", "Niasinamid", 3, 398.50m, 46, PrescriptionType.Normal, ProductKind.Supplement, false, 360),
            ProductSeed(16, "Dijital Ateş Ölçer", "Medikal Cihaz", 4, 245.00m, 130, PrescriptionType.Normal, ProductKind.Supplement, true, 900),
            ProductSeed(17, "Glikoz Test Stribi 50'li", "Enzimatik Test Stribi", 4, 525.40m, 88, PrescriptionType.Normal, ProductKind.Supplement, false, 430),
            ProductSeed(18, "Steril Gaz Kompres 100'lü", "Steril Pamuklu Gaz", 4, 184.90m, 210, PrescriptionType.Normal, ProductKind.Supplement, false, 760));
    }

    private static Product ProductSeed(int id, string name, string ingredient, int categoryId, decimal price,
        int stock, PrescriptionType prescription, ProductKind kind, bool bestSeller, int expiresInDays) => new()
    {
        Id = id, Name = name, ActiveIngredient = ingredient, CategoryId = categoryId, UnitPrice = price,
        Stock = stock, PrescriptionType = prescription, Kind = kind, IsBestSeller = bestSeller,
        ExpirationDate = new DateTime(2026, 8, 10).AddDays(expiresInDays),
        Description = $"{name}, eczane ve sağlık kuruluşlarının profesyonel kullanımına sunulan güvenilir üründür.",
        ImageUrl = categoryId switch
        {
            1 => "/images/products/analgesics.png",
            2 => "/images/products/supplements.png",
            3 => "/images/products/dermocosmetics.png",
            _ => "/images/products/medical-supplies.png"
        }
    };
}
