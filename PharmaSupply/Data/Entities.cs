using System.ComponentModel.DataAnnotations;

namespace PharmaSupply.Data;

public enum PrescriptionType { Normal, Red, Green }
public enum ProductKind { SgkMedicine, Supplement }
public enum OrderStatus { Preparing, Shipped, Delivered, Cancelled }

public sealed class Category
{
    public int Id { get; set; }
    [Required, MaxLength(80)] public string Name { get; set; } = "";
    [MaxLength(40)] public string Icon { get; set; } = "medical_services";
    public ICollection<Product> Products { get; set; } = [];
}

public sealed class Product
{
    public int Id { get; set; }
    [Required, MaxLength(140)] public string Name { get; set; } = "";
    [Required, MaxLength(100)] public string ActiveIngredient { get; set; } = "";
    [MaxLength(2000)] public string Description { get; set; } = "";
    [MaxLength(500)] public string ImageUrl { get; set; } = "";
    [Range(0.01, 1_000_000)] public decimal UnitPrice { get; set; }
    [Range(0, 1_000_000)] public int Stock { get; set; }
    public DateTime ExpirationDate { get; set; }
    public PrescriptionType PrescriptionType { get; set; }
    public ProductKind Kind { get; set; }
    public bool IsBestSeller { get; set; }
    public int CategoryId { get; set; }
    public Category? Category { get; set; }
}

public sealed class Pharmacy
{
    public int Id { get; set; }
    [Required, MaxLength(140)] public string Name { get; set; } = "";
    [Required, MaxLength(30)] public string LicenseNumber { get; set; } = "";
    public bool IsLicenseActive { get; set; } = true;
    public decimal Balance { get; set; }
    public decimal CreditLimit { get; set; }
    public int MonthlyRedPrescriptionQuota { get; set; } = 100;
    public int UsedRedPrescriptionQuota { get; set; }
    public ICollection<Order> Orders { get; set; } = [];
}

public sealed class Order
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public decimal Subtotal { get; set; }
    public decimal Discount { get; set; }
    public decimal Tax { get; set; }
    public decimal Total { get; set; }
    public OrderStatus Status { get; set; } = OrderStatus.Preparing;
    public int PharmacyId { get; set; }
    public Pharmacy? Pharmacy { get; set; }
    public ICollection<OrderItem> Items { get; set; } = [];
}

public sealed class OrderItem
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public Order? Order { get; set; }
    public int ProductId { get; set; }
    public Product? Product { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal Tax { get; set; }
    public decimal Discount { get; set; }
}

public sealed class CartItem
{
    public int Id { get; set; }
    public int PharmacyId { get; set; }
    public Pharmacy? Pharmacy { get; set; }
    public int ProductId { get; set; }
    public Product? Product { get; set; }
    [Range(1, 1_000_000)] public int Quantity { get; set; }
}

public sealed class AdminNotification
{
    public int Id { get; set; }
    [MaxLength(500)] public string Message { get; set; } = "";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsRead { get; set; }
}
