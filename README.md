# AppBookingTour API

Hệ thống API cho ứng dụng đặt tour du lịch được xây dựng theo Clean Architecture với .NET 8.

## Cấu trúc dự án

```
AppBookingTour/
├── AppBookingTour.Api/           # API Layer (Controllers, Middleware)
├── AppBookingTour.Application/   # Application Layer (Use Cases, DTOs)
├── AppBookingTour.Domain/        # Domain Layer (Entities, Enums)
├── AppBookingTour.Infrastructure/ # Infrastructure Layer (Data, Services)
└── AppBookingTour.Share/         # Shared Layer (Common utilities)
```

## Công nghệ sử dụng

- **Framework**: .NET 8
- **Database**: SQL Server
- **ORM**: Entity Framework Core
- **Authentication**: JWT Bearer Token
- **Authorization**: ASP.NET Core Identity + Role-based
- **Architecture**: Clean Architecture, CQRS với MediatR
- **Validation**: FluentValidation
- **Logging**: Serilog
- **Documentation**: Swagger/OpenAPI

## Cài đặt và chạy

### 1. Clone repository

```bash
git clone <repository-url>
cd AppBookingTour
```

### 2. Cấu hình appsettings.json

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=AppBookingTourDb;Trusted_Connection=true"
  },
  "JWT": {
    "SecretKey": "YourVerySecureSecretKeyHereThatIsAtLeast32CharactersLong12345",
    "Issuer": "AppBookingTour",
    "Audience": "AppBookingTour-Users",
    "TokenExpirationMinutes": 60,
    "RefreshTokenExpirationDays": 30
  }
}
```

## Hệ thống phân quyền

### Roles trong hệ thống

- **Admin**: Quản trị viên (quyền cao nhất)
- **Staff**: Nhân viên (quản lý tours, bookings)
- **Customer**: Khách hàng (đặt tour, xem thông tin)

### Cách sử dụng Authorization trong Controller

#### 1. Public API (không cần đăng nhập)

```csharp
[HttpGet]
public async Task<IActionResult> GetTours()
{
    // Ai cũng có thể truy cập
}
```

#### 2. Authenticated User (cần đăng nhập)

```csharp
[Authorize]
[HttpGet("my-bookings")]
public async Task<IActionResult> GetMyBookings()
{
    // Chỉ user đã đăng nhập mới truy cập được
    var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
}
```

#### 3. Role-based Authorization

**Chỉ Admin:**

```csharp
[Authorize(Roles = "Admin")]
[HttpDelete("{id}")]
public async Task<IActionResult> DeleteTour(int id)
{
    // Chỉ Admin mới có thể xóa tour
}
```

**Admin hoặc Staff:**

```csharp
[Authorize(Roles = "Admin,Staff")]
[HttpPost]
public async Task<IActionResult> CreateTour()
{
    // Admin hoặc Staff có thể tạo tour
}
```

**Tất cả roles:**

```csharp
[Authorize(Roles = "Admin,Staff,Customer")]
[HttpPost("bookings")]
public async Task<IActionResult> CreateBooking()
{
    // Mọi user đã đăng nhập đều có thể đặt tour
}
```
