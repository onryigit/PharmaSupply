# PharmaSupply

![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?logo=dotnet&logoColor=white)
![ASP.NET Core MVC](https://img.shields.io/badge/ASP.NET_Core-MVC-5C2D91)
![Entity Framework Core](https://img.shields.io/badge/Entity_Framework-Core-6DB33F)
![SQL Server](https://img.shields.io/badge/Database-SQL_Server-CC2927?logo=microsoftsqlserver&logoColor=white)
![Design Patterns](https://img.shields.io/badge/Design_Patterns-5-0A66C2)

**PharmaSupply**, eczanelerin medikal ürünleri inceleyebildiği, filtreleyebildiği, sepet oluşturabildiği ve güvenli sipariş verebildiği B2B ecza tedarik platformudur.

Proje yalnızca bir e-ticaret arayüzü olarak değil; gerçek iş ihtiyaçlarına karşılık gelen tasarım desenlerini, transaction yönetimini, dinamik veri akışını ve katmanlı uygulama yaklaşımını gösterecek şekilde geliştirilmiştir.

## Projenin Amacı

PharmaSupply aşağıdaki operasyonları tek bir uygulamada birleştirir:

- Ürün, kategori, stok ve reçete bilgilerinin dinamik olarak listelenmesi
- İlaç veya etken madde üzerinden arama yapılması
- Kategori, reçete tipi ve stok durumuna göre filtreleme
- Veritabanında kalıcı sepet yönetimi
- Ürün türüne göre indirim ve vergi hesaplama
- Eczane lisansı, kırmızı reçete kotası ve bakiye doğrulama
- Siparişin transaction içerisinde oluşturulması
- Sipariş sonrası stok, bakiye ve kota bilgilerinin güncellenmesi
- Kritik stok ve yaklaşan miat durumlarında yönetici bildirimi oluşturulması
- Ürün, eczane ve sipariş operasyonlarının Admin Panel üzerinden yönetilmesi

## Uygulama Ekranları

### Kullanıcı tarafı

- **Ana Sayfa:** Kategoriler, çok satan ürünler ve miadı yaklaşan ürünler
- **Ürün Kataloğu:** Sidebar filtreleri, arama ve fiyat sıralaması
- **Ürün Detayı:** Ürün bilgileri, stok, reçete tipi ve sepete ekleme
- **Sepet:** Adet güncelleme, eczane bilgileri ve sipariş tamamlama

### Yönetim tarafı

- **Dashboard:** Ürün, eczane, açık sipariş ve aylık ciro istatistikleri
- **Ürün Yönetimi:** Ürün ekleme, düzenleme, listeleme ve silme
- **Eczane Yönetimi:** Lisans, bakiye, kredi limiti ve reçete kotası takibi
- **Sipariş Yönetimi:** Satın alınan ürünler, adetler, eczane, toplam tutar ve sipariş durumu
- **Akıllı Uyarılar:** Kritik stok ve yaklaşan son kullanma tarihi bildirimleri

## Kullanılan Tasarım Desenleri

Projede toplam beş tasarım deseni, doğrudan bir iş ihtiyacını karşılayacak şekilde uygulanmıştır.

| Tasarım deseni | Kullanım alanı | Sağladığı fayda |
|---|---|---|
| **Chain of Responsibility** | Lisans, kırmızı reçete kotası ve bakiye kontrolleri | Doğrulamaların birbirinden bağımsız ve sıralı çalışmasını sağlar |
| **Unit of Work** | Sipariş, stok, bakiye ve kota güncellemeleri | İlişkili veritabanı işlemlerini tek transaction altında toplar |
| **Strategy** | SGK ilacı ve takviye ürünü fiyatlandırması | Farklı fiyat hesaplama kurallarının kolayca değiştirilebilmesini sağlar |
| **Decorator** | Ürün servisinin önbelleğe alınması | Temel servisi değiştirmeden cache davranışı ekler |
| **Observer** | Stok ve miat değişikliklerinin izlenmesi | Değişiklikleri observer’lara bildirerek yönetici uyarısı ve log üretir |

`PricingStrategyFactory`, ilgili fiyatlandırma stratejisinin seçilmesinden sorumlu yardımcı bileşendir ve Strategy deseninin kullanımını destekler.

## Genel Mimari

```mermaid
flowchart TB
    User["Eczane Kullanıcısı"]
    Admin["Yönetici"]

    subgraph Presentation["Sunum Katmanı"]
        Views["Razor Views"]
        Controllers["MVC Controllers"]
    end

    subgraph Application["Uygulama Katmanı"]
        ProductService["Product Service"]
        CartService["Cart Service"]
        CheckoutService["Checkout Service"]
        ManagementServices["Pharmacy & Order Services"]
    end

    subgraph Patterns["Tasarım Desenleri"]
        Decorator["Decorator\nCached Product Service"]
        Strategy["Strategy\nPricing Strategies"]
        Chain["Chain of Responsibility\nCheckout Validations"]
        UoW["Unit of Work\nTransaction Management"]
        Observer["Observer\nStock Notifications"]
    end

    subgraph Data["Veri Katmanı"]
        DbContext["Entity Framework Core\nAppDbContext"]
        Database[("Microsoft SQL Server")]
    end

    User --> Views
    Admin --> Views
    Views --> Controllers
    Controllers --> ProductService
    Controllers --> CartService
    Controllers --> CheckoutService
    Controllers --> ManagementServices

    ProductService --> Decorator
    CheckoutService --> Strategy
    CheckoutService --> Chain
    CheckoutService --> UoW
    CheckoutService --> Observer

    Decorator --> DbContext
    CartService --> DbContext
    ManagementServices --> DbContext
    UoW --> DbContext
    Observer --> DbContext
    DbContext --> Database
```

## Sipariş Oluşturma Akışı

```mermaid
sequenceDiagram
    actor Pharmacy as Eczane
    participant Cart as CartController
    participant Checkout as CheckoutService
    participant Pricing as PricingStrategyFactory
    participant Validation as Validation Chain
    participant UoW as UnitOfWork
    participant Stock as StockSubject
    participant DB as SQL Server

    Pharmacy->>Cart: Siparişi tamamla
    Cart->>Checkout: Eczane ve sepet bilgileri
    Checkout->>DB: Eczane ve ürünleri getir

    loop Her sipariş kalemi
        Checkout->>Pricing: Ürün türüne uygun stratejiyi seç
        Pricing-->>Checkout: İndirim, vergi ve toplam
    end

    Checkout->>Validation: Lisans kontrolü
    Validation->>Validation: Kırmızı reçete kotası
    Validation->>Validation: Bakiye ve kredi limiti
    Validation-->>Checkout: Doğrulama sonucu

    Checkout->>UoW: Transaction başlat
    UoW->>DB: Siparişi ve kalemleri kaydet
    UoW->>DB: Stok, bakiye ve kotayı güncelle
    Checkout->>Stock: Güncellenen ürünleri bildir
    Stock->>DB: Kritik stok ve miat uyarılarını kaydet
    UoW->>DB: Transaction commit
    Checkout-->>Cart: Başarılı sipariş sonucu
    Cart-->>Pharmacy: Sipariş onayı
```

## Teknoloji Yığını

- **.NET 10**
- **ASP.NET Core MVC**
- **Entity Framework Core 10**
- **Microsoft SQL Server**
- **Razor Views**
- **Dependency Injection**
- **Memory Cache**
- **Bootstrap, özel CSS ve JavaScript**
- **EF Core Migrations**

## Proje Yapısı

```text
PharmaSupply/
├── Controllers/          MVC request ve response akışları
├── Data/                 DbContext, entity modelleri ve Unit of Work
├── Models/               ViewModel ve checkout modelleri
├── Services/
│   ├── Caching/          Decorator implementasyonu
│   ├── Checkout/         Checkout servisi ve doğrulama zinciri
│   ├── Notifications/    Observer implementasyonu
│   └── Pricing/          Strategy ve fiyatlandırma seçimi
├── Views/                Kullanıcı ve Admin Panel Razor ekranları
├── Migrations/           Veritabanı şeması ve seed geçmişi
└── wwwroot/              CSS, JavaScript ve ürün görselleri
```

## Teknik Kazanımlar

Bu projede özellikle aşağıdaki mühendislik konularına odaklanılmıştır:

- Interface tabanlı ve Dependency Injection uyumlu servis tasarımı
- İş kurallarının controller katmanından ayrılması
- Birden fazla veri değişikliğinin transaction güvenliğiyle yürütülmesi
- Genişletilebilir fiyatlandırma ve doğrulama mekanizmaları
- Cache invalidation yönetimi
- Veritabanında kalıcı sepet yapısı
- İlişkili entity’lerin doğru şekilde sorgulanması
- Dinamik ve yönetilebilir Admin Panel verileri
- Okunabilir, sorumlulukları ayrılmış ve sürdürülebilir kod yapısı

<img width="1904" height="915" alt="x9" src="https://github.com/user-attachments/assets/3a0b1787-de5d-446a-bf9a-bb0b98344cca" />
<img width="1902" height="913" alt="x8" src="https://github.com/user-attachments/assets/ffa3c984-72f1-4430-afa6-59a33eaafe5d" />
<img width="1905" height="916" alt="x7" src="https://github.com/user-attachments/assets/a6c71ada-ce2c-4020-b44d-5c7285815ea3" />
<img width="1902" height="916" alt="x6" src="https://github.com/user-attachments/assets/77b83a8e-cce2-4003-883a-8ec908c35a41" />
<img width="1904" height="948" alt="x5" src="https://github.com/user-attachments/assets/96b2280a-3939-4fac-b790-93643e66ca68" />
<img width="1908" height="948" alt="x4" src="https://github.com/user-attachments/assets/d0d656ad-555f-4094-a429-4b866792280f" />
<img width="1895" height="943" alt="x3" src="https://github.com/user-attachments/assets/ef58b5d9-c5fc-482a-a966-5307f59c156c" />
<img width="1907" height="940" alt="x2" src="https://github.com/user-attachments/assets/80f49f53-29ec-4c1d-9e34-ab7abf79dd9e" />
<img width="1902" height="948" alt="x1" src="https://github.com/user-attachments/assets/15416009-3a71-4617-8c05-b8610be0cba5" />
<img width="1905" height="952" alt="x11" src="https://github.com/user-attachments/assets/fc1f12de-75ff-47f3-bc22-3a6afb5b39df" />
<img width="1920" height="916" alt="x1111" src="https://github.com/user-attachments/assets/47dd2aa9-2e9c-4c87-8739-cb8405ccb538" />

