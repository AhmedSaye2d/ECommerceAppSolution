# 🛒 Enterprise E-Commerce Backend System

[![.NET 8](https://img.shields.io/badge/.NET-8.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)
[![C# 12](https://img.shields.io/badge/C%23-12.0-239120?style=for-the-badge&logo=csharp&logoColor=white)](https://docs.microsoft.com/en-us/dotnet/csharp/)
[![Clean Architecture](https://img.shields.io/badge/Architecture-Clean%20Architecture-blue?style=for-the-badge)](https://blog.cleancoder.com/)
[![EF Core 8](https://img.shields.io/badge/EF%20Core-8.0-purple?style=for-the-badge)](https://docs.microsoft.com/en-us/ef/core/)
[![SQL Server](https://img.shields.io/badge/SQL%20Server-2022-CC292B?style=for-the-badge&logo=microsoftsqlserver&logoColor=white)](https://www.microsoft.com/sql-server)
[![JWT Auth](https://img.shields.io/badge/Auth-JWT%20%2B%20Refresh%20Tokens-orange?style=for-the-badge&logo=jsonwebtokens&logoColor=white)](https://jwt.io/)
[![Stripe API](https://img.shields.io/badge/Payments-Stripe%20Checkout-6772E5?style=for-the-badge&logo=stripe&logoColor=white)](https://stripe.com/)
[![Serilog](https://img.shields.io/badge/Logging-Serilog-007ACC?style=for-the-badge)](https://serilog.net/)

A production-grade, highly scalable E-Commerce RESTful Backend API built with **C# 12** and **ASP.NET Core 8**, adhering strictly to **Clean Architecture** principles and the **Repository Pattern**.

The system provides an enterprise-ready backbone for e-commerce operations, featuring dynamic product discovery, cart checkout with **Stripe Hosted Checkout**, order lifecycle state transitions, JWT authentication with single-use **Refresh Token Rotation**, centralized SQL Server error interception, and structured logging.

---

## 🛠️ Technology Stack & Frameworks

- **Runtime & Language:** .NET 8 (LTS) & C# 12
- **Data Access & ORM:** Entity Framework Core 8, LINQ, SQL Server, EntityFramework.Exceptions.SqlServer
- **Security & Identity:** ASP.NET Core Identity, JWT Bearer Tokens, Cryptographically Secure Refresh Tokens
- **External Integrations:** Stripe API (Stripe.net SDK)
- **Object Mapping:** AutoMapper
- **Validation:** FluentValidation
- **Diagnostics & Logging:** Serilog (Console Sink, Daily Rolling File Sink)
- **API Documentation:** Swagger / OpenAPI UI

---

## 🏛️ System Architecture

The solution is architected following **Clean Architecture (Onion Architecture)** guidelines to enforce strict separation of concerns, testability, and independence from external frameworks and database mechanisms. Dependencies strictly point inward toward the core domain.

`mermaid
graph TD
    subgraph Presentation ["1. Presentation Layer (ECommerce.Host)"]
        Controllers[API Controllers]
        Middlewares[Exception & Logging Middlewares]
        Swagger[Swagger / OpenAPI Documentation]
    end

    subgraph Application ["2. Application Layer (ECommerceApp.Application)"]
        Services[Application Services]
        DTOs[Data Transfer Objects DTOs]
        Validation[FluentValidation Rules]
        Mapping[AutoMapper Profiles]
    end

    subgraph Infrastructure ["3. Infrastructure Layer (ECommerce.Infrastructure)"]
        DbContext[AppDbContext & Migrations]
        Repositories[Generic & Specialized Repositories]
        Identity[ASP.NET Identity & JWT Token Management]
        StripeClient[Stripe Checkout Integration]
        SerilogAdapter[Serilog Structured Logger Adapter]
    end

    subgraph Domain ["4. Domain Layer (ECommerceApp.Domain)"]
        Entities[Domain Entities]
        DomainInterfaces[Repository & Service Interfaces]
        Enums[Enums & Value Objects]
    end

    Presentation --> Application
    Infrastructure --> Application
    Infrastructure --> Domain
    Application --> Domain
`

### Layer Responsibilities:
1. **Domain:** The enterprise core containing domain models (Product, Category, Order, OrderItem, AppUser, RefreshToken, PaymentMethod, Achieve), domain enums (OrderStatus), and interface definitions. Contains zero external dependencies.
2. **Application:** Implements use cases and business workflows (OrderService, ProductService, AuthenticationService, CartService), input validation, and DTO projections.
3. **Infrastructure:** Manages database interaction, EF Core configuration, SQL Server error translation, Stripe payment processing, and logging adapters.
4. **Host (Presentation):** ASP.NET Core Web API host managing routing, controller endpoints, dependency injection bootstrapping, and middleware pipeline registration.

---

## 🗄️ Database Schema & Entity Relationship Diagram (ERD)

The database design uses normalized relational schemas managed via EF Core Code-First migrations with decimal precision for currency amounts (decimal(18,2)) and foreign key constraints.

`mermaid
erDiagram
    APP_USER ||--o{ REFRESH_TOKEN : "owns"
    APP_USER ||--o{ ORDER : "places"
    CATEGORY ||--o{ PRODUCT : "classifies"
    ORDER ||--|{ ORDER_ITEM : "contains"
    PRODUCT ||--o{ ORDER_ITEM : "ordered in"
    PAYMENT_METHOD ||--o{ ORDER : "paid via"

    APP_USER {
        string Id PK "Identity GUID"
        string FullName "User Full Name"
        string Email "Unique Email Address"
        string UserName "User Name"
    }

    REFRESH_TOKEN {
        Guid Id PK
        string Token "Cryptographic Base64 Token"
        string UserId FK "References AppUser"
    }

    CATEGORY {
        Guid Id PK
        string Name "Category Name"
    }

    PRODUCT {
        Guid Id PK
        string Name "Product Title"
        string Description "Detailed Description"
        decimal Price "Precision 18,2"
        string Image "Image URL/Path"
        int Quantity "Available Stock"
        Guid CategoryId FK "References Category"
    }

    ORDER {
        Guid Id PK
        string UserId FK "References AppUser"
        datetime OrderDate "UTC Timestamp"
        decimal TotalAmount "Server Verified Total"
        string ShippingAddress "Physical Delivery Destination"
        string PhoneNumber "Contact Telephone"
        string Status "Pending, Processing, Shipped, Delivered, Cancelled"
        Guid PaymentMethodId FK "References PaymentMethod"
    }

    ORDER_ITEM {
        Guid Id PK
        Guid OrderId FK "References Order"
        Guid ProductId FK "References Product"
        int Quantity "Units Purchased"
        decimal Price "Historical Unit Price Snapshot"
    }

    PAYMENT_METHOD {
        Guid Id PK
        string Name "CreditCard, etc."
    }

    ACHIEVE {
        Guid Id PK
        Guid ProductId "Product Reference"
        Guid UserId "Customer Reference"
        int Quantity "Quantity Purchased"
        datetime CreatedData "Audit Timestamp"
    }
`

---
