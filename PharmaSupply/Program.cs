using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using PharmaSupply.Data;
using PharmaSupply.Services;
using PharmaSupply.Services.Caching;
using PharmaSupply.Services.Checkout;
using PharmaSupply.Services.Notifications;
using PharmaSupply.Services.Pricing;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddMemoryCache();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<IProductService>(sp => new CachedProductService(
    sp.GetRequiredService<ProductService>(), sp.GetRequiredService<IMemoryCache>()));
builder.Services.AddScoped<IPharmacyService, PharmacyService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<ICheckoutService, CheckoutService>();
builder.Services.AddScoped<IStockObserver, AdminStockObserver>();
builder.Services.AddScoped<IStockSubject, StockSubject>();
builder.Services.AddScoped<PricingStrategyFactory>();
builder.Services.AddScoped<LicenseValidationHandler>();
builder.Services.AddScoped<RedPrescriptionQuotaHandler>();
builder.Services.AddScoped<BalanceValidationHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
