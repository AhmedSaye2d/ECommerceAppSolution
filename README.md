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
