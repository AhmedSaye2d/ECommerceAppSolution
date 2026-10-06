# E-Commerce Backend Application

A scalable E-Commerce Backend built with C# and ASP.NET Core, following Clean Architecture principles and Repository Pattern for maintainability, testability, and separation of concerns.

## Technologies & Tools

- **C# & ASP.NET Core**
- **Entity Framework Core (EF Core)**
- **LINQ** for efficient data querying
- **Clean Architecture**
- **Repository Pattern**
- **JWT Authentication & Refresh Tokens**
- **Stripe Payment Gateway Integration**
- **Custom Middleware** for centralized error handling
- **Serilog** for structured logging and monitoring

## Key Features

### 1. Authentication & Authorization
- User registration
- Login with JWT token
- Refresh token support
- Role-based authorization (Admin/User)

### 2. Product Management
- Get all products
- Search and filter products with query parameters
- Get single product by ID
- Add new product
- Update product
- Delete product

### 3. Category Management
- Get all categories
- Get single category
- Add new category
- Update category
- Delete category

### 4. Order Management
- Create new order
- Get user orders
- Get order by ID
- Cancel order
- Update order status (Admin only)

### 5. Cart & Checkout
- Checkout process
- Save checkout history
- View available payment methods (Stripe Integration)

### 6. Infrastructure
- Centralized error handling via Custom Middleware
- Structured logging with Serilog
- Clean layer separation (Domain, Application, Infrastructure, Host)

## Project Structure

```
ECommerceAppSolution/
├── ECommerce.Host/           # Web API Layer
│   ├── Controllers/          # API Controllers
│   └── Program.cs            # Application Entry Point
├── ECommerceApp.Application/ # Business Logic Layer
│   ├── Services/             # Service Implementations
│   ├── Dto/                  # Data Transfer Objects
│   └── Mapping/              # Object Mapping
├── ECommerceApp.Domain/      # Domain Layer
│   ├── Entities/             # Domain Entities
│   └── Interface/            # Repository Interfaces
└── ECommerce.Infrastructure/ # Infrastructure Layer
    ├── Repository/           # Repository Implementations
    ├── Service/              # External Service Integrations
    ├── MiddleWare/           # Custom Middleware
    └── Data/                 # Database Context
```

## Objective

Build a real-world E-Commerce backend applying best practices in backend development and software architecture, simulating production-level systems.