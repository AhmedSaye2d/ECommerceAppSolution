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

## 🔐 Authentication & Refresh Token Rotation Flow

Authentication utilizes ASP.NET Core Identity combined with stateless JWT bearer tokens for API requests and persistent Refresh Tokens stored in SQL Server.

To protect against token theft and replay attacks, the platform employs **Single-Use Refresh Token Rotation**: every time a client refreshes their session, the current refresh token is invalidated and replaced with a newly minted cryptographic token.

`mermaid
sequenceDiagram
    autonumber
    actor Client as Web / Mobile Client
    participant AuthCtrl as AuthenticationController
    participant AuthSvc as AuthenticationService
    participant UserMgr as UserManager / Identity
    participant TokenMgr as TokenManagement
    participant DB as SQL Server Database

    %% Registration Flow
    rect rgb(240, 248, 255)
    Note over Client, DB: 1. User Registration & Role Assignment
    Client->>AuthCtrl: POST /api/Authentication/Create (User DTO)
    AuthCtrl->>AuthSvc: CreateUser(CreateUser DTO)
    AuthSvc->>UserMgr: CreateAsync(AppUser, Password)
    UserMgr->>DB: INSERT into AspNetUsers
    AuthSvc->>UserMgr: AddToRoleAsync (First User -> Admin, Subsequent -> User)
    alt Role Assignment Fails
        AuthSvc->>UserMgr: RemoveUserByEmail (Compensating Transaction)
        AuthSvc-->>Client: 400 Bad Request
    else Role Assignment Success
        AuthSvc-->>Client: 200 OK ("User created successfully")
    end
    end

    %% Login Flow
    rect rgb(245, 255, 245)
    Note over Client, DB: 2. Login & Token Issuance
    Client->>AuthCtrl: POST /api/Authentication/Login (Email, Password)
    AuthCtrl->>AuthSvc: LoginUser(credentials)
    AuthSvc->>UserMgr: CheckPasswordAsync(user, password)
    AuthSvc->>TokenMgr: GenerateToken(claims) -> Returns 2-hour JWT
    AuthSvc->>TokenMgr: GetRefreshToken() -> Cryptographic 64-byte RNG Token
    TokenMgr->>DB: UPSERT RefreshToken for User
    AuthSvc-->>Client: 200 OK (JWT Access Token + Refresh Token)
    end

    %% Refresh Token Rotation Flow
    rect rgb(255, 250, 240)
    Note over Client, DB: 3. Single-Use Refresh Token Rotation
    Client->>AuthCtrl: POST /api/Authentication/refresh-token (Current Refresh Token)
    AuthCtrl->>AuthSvc: ReviveToken(refreshToken)
    AuthSvc->>TokenMgr: ValidateRefreshToken(refreshToken)
    TokenMgr->>DB: Verify token exists and retrieve UserId
    AuthSvc->>TokenMgr: GenerateToken(claims) -> Fresh JWT
    AuthSvc->>TokenMgr: GetRefreshToken() -> New Cryptographic Token
    TokenMgr->>DB: UPDATE RefreshToken (Old token invalidated immediately)
    AuthSvc-->>Client: 200 OK (New JWT Token + New Refresh Token)
    end
`

---

## 💳 Checkout & Stripe Payment Processing Workflow

The checkout process guarantees transaction security and financial integrity:
1. **Server-Side Price Verification:** Client-submitted prices are discarded. The backend queries SQL Server for the actual product prices, preventing price tampering.
2. **Stripe Hosted Checkout:** Line items are formatted with description, name, and unit amount converted to cents (price * 100) and dispatched to Stripe Checkout API.
3. **Order Snapshotting:** When an order is saved, unit prices are permanently recorded in OrderItem records so future product price updates never alter historical invoices.

`mermaid
sequenceDiagram
    autonumber
    actor Customer as Shopper
    participant CartCtrl as CartController
    participant CartSvc as CartService
    participant ProductRepo as ProductRepository
    participant StripeSvc as StripePaymentService
    participant StripeAPI as Stripe Checkout API
    participant OrderCtrl as OrdersController
    participant OrderSvc as OrderService
    participant OrderRepo as OrderRepository
    participant DB as SQL Server Database

    rect rgb(240, 240, 255)
    Note over Customer, StripeAPI: Phase 1: Cart Checkout & Stripe Session
    Customer->>CartCtrl: POST /api/Cart/checkout (Cart Items, PaymentMethodId)
    CartCtrl->>CartSvc: CheckOut(CheckOut DTO)
    CartSvc->>ProductRepo: Fetch product records from DB
    CartSvc->>CartSvc: Calculate true total amount (discard client prices)
    CartSvc->>StripeSvc: Pay(totalAmount, verifiedProducts, carts)
    StripeSvc->>StripeAPI: SessionService.CreateAsync(SessionCreateOptions)
    StripeAPI-->>StripeSvc: Return Stripe Session Object (with hosted session.Url)
    StripeSvc-->>CartCtrl: ServiceResponse(session.Url)
    CartCtrl-->>Customer: 200 OK with redirect URL
    Customer->>StripeAPI: User redirected to Stripe secure hosted payment page
    end

    rect rgb(255, 245, 245)
    Note over Customer, DB: Phase 2: Order Creation & Historical Price Snapshot
    Customer->>OrderCtrl: POST /api/Orders (CreateOrderDto + Auth Bearer Token)
    OrderCtrl->>OrderSvc: CreateOrderAsync(orderDto, authenticatedUserId)
    OrderSvc->>ProductRepo: Fetch product details
    OrderSvc->>OrderSvc: Create OrderItems with unit price snapshot
    OrderSvc->>OrderRepo: CreateOrderAsync(Order with OrderItems)
    OrderRepo->>DB: INSERT into Orders & OrderItems (Status = Pending)
    OrderSvc-->>OrderCtrl: ServiceResponse("Order placed successfully")
    OrderCtrl-->>Customer: 200 OK
    end
`

---

## ⚡ High-Performance Product Query Pipeline

To serve fast catalog browsing with thousands of SKU items, ProductRepository constructs a dynamic deferred IQueryable pipeline that executes completely inside SQL Server.

`mermaid
flowchart LR
    A[Client Request<br/>GET /api/Product?search=...&categoryId=...&minPrice=...&maxPrice=...&sortBy=...&pageNumber=1&pageSize=10] --> B[ProductController]
    B --> C[ProductService.GetProductsAsync]
    C --> D[ProductRepository.GetProductsAsync]
    
    subgraph Pipeline ["Server-Side LINQ Execution Pipeline"]
        D --> E[Start: IQueryable<Product><br/>.Include Category<br/>.AsNoTracking]
        E --> F{Category Filter?}
        F -- Yes --> F1[Where CategoryId == val]
        F -- No --> G{Price Range Filter?}
        F1 --> G
        G -- Yes --> G1[Where Price >= min & Price <= max]
        G -- No --> H{Search Term?}
        G1 --> H
        H -- Yes --> H1[Where Name.Contains or Description.Contains]
        H -- No --> I[CountAsync<br/>Fetch Total Filtered Count]
        H1 --> I
        I --> J[Dynamic Sorting<br/>Switch: Price Asc/Desc, Name Asc/Desc]
        J --> K[Skip page - 1 * pageSize<br/>Take pageSize<br/>ToListAsync]
    end

    K --> L[Return Tuple<br/>Items, TotalCount]
    L --> M[AutoMapper: Map to IEnumerable<GetProduct>]
    M --> N[Return PaginatedResult<GetProduct><br/>Items, TotalCount, PageNumber, PageSize]
    N --> O[HTTP 200 OK Response]
`

### Key Optimizations:
- **AsNoTracking():** Disables EF Core Change Tracker allocations for read-only queries.
- **Single Roundtrip:** Total count and paginated items are queried efficiently without pulling full dataset into memory.
- **SQL Case-Insensitive Search:** LINQ .Contains() translates to SQL LIKE '%term%' handled by the database engine.

---

## 📦 Core Business Modules & Capabilities

### 1. Identity & Access Management (IAM)
- **User Registration:** Validated via FluentValidation for strong password policies (min 8 chars, uppercase, lowercase, digit, special symbol).
- **Self-Bootstrapping Admin:** First registered user automatically acquires the Admin role; subsequent users receive User.
- **Compensating Rollback:** If role assignment fails during signup, the user entity is deleted automatically to prevent orphan accounts.
- **JWT Authentication:** Issues signed HMAC-SHA256 JWT tokens containing FullName, Email, NameIdentifier, and Role claims.
- **Refresh Token Rotation:** Employs single-use refresh tokens stored in SQL Server with cryptographic RNG generation.

### 2. Catalog & Category Management
- **Category CRUD:** Create, read, update, and delete product categories.
- **Product Management:** Complete product information handling (Title, Description, Image URL, Stock Quantity, Price).
- **Search & Multi-Filter:** Filter by category, minimum price, maximum price, text search across titles and descriptions.
- **Dynamic Pagination:** Server-side sorting (price_asc, price_desc, 
ame_asc, 
ame_desc) with custom page sizes.

### 3. Cart, Checkout & Stripe Payment
- **Price Verification:** Recalculates cart amounts strictly on the server to prevent front-end client price tampering.
- **Stripe Hosted Checkout:** Formats line items with cents conversion, sets redirect callback URLs, and returns Stripe session links.
- **Checkout History:** Logs checkout history into Achieve entity for auditing and cart recovery analysis.

### 4. Order Lifecycle Management
- **Order Placement:** Converts cart items into permanent order entities and captures unit prices at the time of purchase.
- **Ownership Verification:** Users can only view and cancel their own orders; Admins can view all orders.
- **Status State Machine:** Orders transition across Pending ➔ Processing ➔ Shipped ➔ Delivered (or Cancelled).
- **Cancellation Constraints:** Customers can only cancel orders while status is Pending.

---

## 🧩 Key Engineering Challenges & Technical Solutions

| # | Engineering Challenge | Problem & Risk | Technical Approach & Solution | Key Benefit |
|---|----------------------|----------------|--------------------------------|-------------|
| **1** | **Cart Price Tampering** | Malicious clients altering product prices in JSON payloads before payment. | Server-side price recalculation in OrderService & CartService fetching prices directly from DB. | 100% financial accuracy; zero reliance on untrusted client inputs. |
| **2** | **Session Hijacking & Replay Attacks** | Stolen static refresh tokens allowing permanent unauthorized access. | Single-use Refresh Token Rotation with 64-byte RandomNumberGenerator cryptographic tokens. | Stolen tokens are immediately rendered useless upon rotation. |
| **3** | **Memory Exhaustion on Catalog Browsing** | Loading large product tables into web server RAM for filtering and pagination. | Deferred IQueryable pipeline with AsNoTracking() executing Skip() and Take() at database level. | Minimal RAM usage and fast, scalable response times under high traffic. |
| **4** | **Database Exception Security Leaks** | Exposing raw SQL Server errors (PK, FK, NULL violations) to public API consumers. | Custom ExceptionHandlingMiddleware intercepting SqlException error numbers (2627, 515, 547). | Sanitized, descriptive HTTP status codes (409 Conflict, 400 Bad Request) without data leakage. |
| **5** | **Inconsistent State on Registration Failure** | Users created in Identity tables without assigned roles when role mapping fails. | Compensating transaction logic in AuthenticationService rolling back user creation if role assignment fails. | Guarantees database consistency and eliminates orphaned user records. |

---

## 🔌 API Endpoints Catalog (22 Verified Endpoints)

### 🔑 Authentication (/api/Authentication)
| Method | Endpoint | Authorization | Description |
|--------|----------|---------------|-------------|
| POST | /api/Authentication/Create | Public | Register new user (First user becomes Admin) |
| POST | /api/Authentication/Login | Public | Authenticate user and receive JWT + Refresh Token |
| POST | /api/Authentication/refresh-token | Public | Refresh expired JWT using valid Refresh Token |

### 📦 Products (/api/Product)
| Method | Endpoint | Authorization | Description |
|--------|----------|---------------|-------------|
| GET | /api/Product/all | Public | Retrieve list of all products |
| GET | /api/Product | Public | Search, filter by category/price, sort, and paginate |
| GET | /api/Product/Single/{id} | Public | Retrieve single product details by GUID |
| POST | /api/Product/Add | Admin | Add new product to catalog |
| PUT | /api/Product/Update | Admin | Update existing product |
| DELETE | /api/Product/Delete/{id} | Admin | Delete product from catalog |

### 🏷️ Categories (/api/Category)
| Method | Endpoint | Authorization | Description |
|--------|----------|---------------|-------------|
| GET | /api/Category/all | Public | Retrieve all product categories |
| GET | /api/Category/Single/{id} | Public | Retrieve category by GUID |
| POST | /api/Category/Add | Admin | Create a new category |
| PUT | /api/Category/Update | Admin | Update existing category |
| DELETE | /api/Category/Delete/{id} | Admin | Delete category |

### 🛒 Cart & Checkout (/api/Cart & /api/Payment)
| Method | Endpoint | Authorization | Description |
|--------|----------|---------------|-------------|
| POST | /api/Cart/checkout | Authenticated | Validate cart prices & generate Stripe session URL |
| POST | /api/Cart/Save-checkout | Authenticated | Archive cart items into checkout history (Achieve) |
| GET | /api/Payment | Public | Retrieve supported payment methods |

### 📋 Orders (/api/Orders)
| Method | Endpoint | Authorization | Description |
|--------|----------|---------------|-------------|
| POST | /api/Orders | Authenticated | Place a new order from cart with address & phone |
| GET | /api/Orders | Authenticated | Retrieve order history for current authenticated user |
| GET | /api/Orders/{id} | User/Admin | Get order by ID (Enforces ownership verification) |
| PUT | /api/Orders/{id}/cancel | User/Admin | Cancel order (Only permitted if status is Pending) |
| PUT | /api/Orders/{id}/status | Admin | Update order status (Pending ➔ Processing ➔ Shipped ➔ Delivered) |

---

## 🚀 Getting Started & Local Setup

### Prerequisites
- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [SQL Server](https://www.microsoft.com/sql-server) (LocalDB, Express, or Full)
- [Stripe Account](https://stripe.com) (for test API Secret Key)

### 1. Clone the Repository
`ash
git clone https://github.com/AhmedSaye2d/ECommerceAppSolution.git
cd ECommerceAppSolution
`

### 2. Configure ppsettings.json
Update ECommerce.Host/appsettings.json with your SQL Server connection string and credentials:

`json
{
  "ConnectionStrings": {
    "Default": "Server=.;Database=ECommerceDb;Trusted_Connection=True;TrustServerCertificate=True;"
  },
  "Jwt": {
    "Key": "YOUR_SUPER_SECRET_SECURITY_KEY_AT_LEAST_32_CHARS_LONG",
    "Issuer": "https://localhost:44353",
    "Audience": "https://localhost:44353"
  },
  "Stripe": {
    "Secretkey": "sk_test_YOUR_STRIPE_SECRET_KEY"
  }
}
`

### 3. Apply EF Core Migrations
Execute the migrations to generate database schema and initial seed data:
`ash
dotnet ef database update --project ECommerce.Infrastructure --startup-project ECommerce.Host
`

### 4. Run the Application
`ash
dotnet run --project ECommerce.Host
`
Once launched, navigate to the interactive Swagger documentation:
👉 https://localhost:44353/swagger

---
