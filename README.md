# MicroserviceArticles

Bu repo, **iki ayrı ASP.NET Core Web API** servisi ve bu servislerin bağlı olduğu iki ayrı MongoDB instance’ı ile kurulmuş basit bir mikroservis örneğidir.

## Genel Mimari

Çözüm (`MicroserviceArticles.sln`) şu parçalardan oluşur:

- `ArticleAPI`: Makale verisini yönetir.
- `ReviewAPI`: Değerlendirme/yorum verisini yönetir.
- `docker-compose`: Servisleri ve veritabanlarını birlikte ayağa kaldırır.

Her API kendi veritabanına bağlanır:

- `ArticleAPI` → `ArticleDb / Articles`
- `ReviewAPI` → `ReviewDb / Reviews`

Bu yapı, mikroservis yaklaşımındaki "her servisin kendi verisi" prensibini örnekler.

## Klasör Yapısı ve Katmanlar

Her serviste benzer bir düzen var:

- `Program.cs`
  - Dependency Injection kaydı yapılır.
  - Mongo ayarları `IOptions<...>` ile bağlanır.
  - Controller ve Swagger devreye alınır.
- `Controllers/`
  - HTTP endpoint’leri barındırır (`GET/POST/PUT/DELETE`).
  - İş kurallarını doğrudan yazmak yerine service katmanına delegasyon yapar.
- `Services/`
  - MongoDB ile konuşan veri erişim katmanıdır.
  - CRUD operasyonları burada toplanır.
- `Entities/`
  - Mongo doküman modelini temsil eden sınıflar.
  - `BsonId` ve `ObjectId` eşleştirmeleri burada yapılır.
- `Settings/`
  - Veritabanı bağlantı ayarlarının (`ConnectionString`, `DatabaseName`, `CollectionName`) tutulduğu POCO sınıflar.

## Lokal Çalıştırma

### Docker Compose ile (önerilen)

```bash
docker compose up --build
```

Varsayılan portlar:

- `ArticleAPI`: `http://localhost:8000`
- `ReviewAPI`: `http://localhost:8001`
- `articledb`: `localhost:27017`
- `reviewdb`: `localhost:27018`

Swagger arayüzleri:

- `http://localhost:8000/swagger`
- `http://localhost:8001/swagger`

### Projeleri ayrı çalıştırma

- `ArticleAPI` profili: `http://localhost:5031`
- `ReviewAPI` profili: `http://localhost:5260`

Development ayarları ilgili servisin `appsettings.Development.json` dosyasında tanımlıdır.

## Yeni Katılanlar İçin Önemli Noktalar

1. **Servisler bağımsızdır**
   - Kod tekrarına benzerlik olsa da iki API ayrı deploy edilir ve ayrı veritabanı kullanır.
2. **Controller ince, Service kalın**
   - Endpoint’lerde asıl veri erişimi service katmanında tutulur.
3. **ID formatı Mongo ObjectId’ye bağlıdır**
   - Route’larda `id:length(24)` kontrolü vardır; geçersiz uzunlukta ID istekleri route’a düşmez.
4. **Şema validasyonu sınırlı**
   - Şu an DTO/FluentValidation katmanı yok; entity nesneleri doğrudan request body’den alınıyor.
5. **Servisler arasında gerçek entegrasyon henüz zayıf**
   - `Review` içinde hem `ArticleId` hem `Article` metni bulunuyor; veri tutarlılığı için ek tasarım kararları gerekebilir.

## Sonraki Öğrenme / Geliştirme Adımları

1. **DTO + doğrulama katmanı ekleyin**
   - Request/response modellerini Entity’den ayırın.
2. **Servisler arası iletişimi netleştirin**
   - Senkron HTTP çağrısı mı, event-driven akış mı kullanılacağına karar verin.
3. **Hata yönetimi ve gözlemlenebilirlik**
   - Global exception middleware, structured logging, health-check endpointleri ekleyin.
4. **Test stratejisi**
   - Controller integration testleri ve service unit testleri ekleyin.
5. **Güvenlik**
   - AuthN/AuthZ, rate limiting ve API versiyonlama planlayın.
6. **Konfigürasyon yönetimi**
   - Ortam bazlı secret yönetimi ve config ayrıştırmasını düzenleyin.

## Hızlı Okuma Sırası (Onboarding)

1. `docker-compose.yml` ve `docker-compose.override.yml`
2. `ArticleAPI/Program.cs` + `ReviewAPI/Program.cs`
3. Her iki serviste `Controllers` → `Services` → `Entities`
4. `appsettings.Development.json` dosyaları

Bu sırayla okuyunca, sistemin çalışma akışını en kısa sürede zihninizde oturtabilirsiniz.
