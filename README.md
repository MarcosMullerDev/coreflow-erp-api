🚀 CoreFlow ERP API

Enterprise ERP backend built with ASP.NET Core 9, PostgreSQL and JWT Authentication.

A scalable multi-tenant ERP architecture focused on performance, security and real-world business scenarios.

📌 Features
🔐 Authentication & Security
JWT Authentication
BCrypt Password Hashing
Role-based Authorization
Protected Endpoints
Current User Context
🏢 Multi-Tenant Architecture
Company-based data isolation
Users linked to companies
Secure tenant filtering
📦 Product Management
Product CRUD
Stock Control
Soft Delete
Pagination
SKU support
💰 Sales Module
Sales creation
Multiple sale items
Automatic stock decrease
Sales history
Revenue calculation
📊 Dashboard Analytics
Total Revenue
Total Sales
Total Products
Low Stock Products
🧱 Architecture
Clean Layered Architecture
Repository Pattern
Service Layer
DTOs
Dependency Injection
Global Exception Middleware
🛠️ Tech Stack
ASP.NET Core 9
Entity Framework Core
PostgreSQL
JWT Bearer Authentication
FluentValidation
Swagger / OpenAPI
BCrypt.Net
C#
📂 Project Structure
src/
 ├── CoreFlow.Api
 ├── CoreFlow.Application
 ├── CoreFlow.Domain
 └── CoreFlow.Infrastructure
🏗️ Architecture Layers
CoreFlow.Api

Responsible for:

Controllers
Middlewares
Authentication configuration
Swagger
API entry point
CoreFlow.Application

Responsible for:

Services
DTOs
Interfaces
Business rules
Validation
CoreFlow.Domain

Responsible for:

Entities
Enums
BaseEntity
Core domain rules
CoreFlow.Infrastructure

Responsible for:

EF Core
PostgreSQL
Repositories
JWT generation
Database access
🔑 Authentication

This API uses JWT Bearer Authentication.

Login Example
POST /api/Auth/login
{
  "email": "admin@coreflow.com",
  "password": "123456"
}
🧪 Swagger

Swagger available at:

/swagger

Use the Authorize button with:

Bearer YOUR_TOKEN
📦 Product Example
Create Product
POST /api/Products
{
  "name": "Gaming Mouse",
  "description": "RGB Mouse",
  "sku": "MOUSE-001",
  "costPrice": 100,
  "salePrice": 180,
  "stockQuantity": 20
}
💵 Sale Example
Create Sale
POST /api/Sales
{
  "items": [
    {
      "productId": "PRODUCT_GUID",
      "quantity": 2
    }
  ]
}
📊 Dashboard Endpoint
GET /api/Dashboard/summary

Example response:

{
  "totalRevenue": 25000,
  "totalSales": 143,
  "totalProducts": 87,
  "lowStockProducts": 5
}
⚙️ Running Locally
Clone repository
git clone https://github.com/YOUR_USERNAME/coreflow-erp-api.git
Restore packages
dotnet restore
Run migrations
dotnet ef database update \
--project src/CoreFlow.Infrastructure \
--startup-project src/CoreFlow.Api
Start API
dotnet run --project src/CoreFlow.Api
🧭 Roadmap
 JWT Authentication
 Multi-Tenant Architecture
 Product Module
 Sales Module
 Dashboard Analytics
 Soft Delete
 Pagination
 Docker Support
 Redis Cache
 Refresh Tokens
 Unit Tests
 Next.js Frontend
 CI/CD Pipeline
📈 Future Improvements
Docker Compose
Redis Cache
RabbitMQ
Financial Module
Reports & BI
Real-time Notifications
AI Analytics
SaaS Billing
👨‍💻 Author

Marcos Müller

Backend Developer focused on scalable enterprise applications.

ASP.NET Core
PostgreSQL
Node.js
React / Next.js
REST APIs
⭐ Project Status

🚧 In active development.

This project is continuously evolving into a complete enterprise ERP platform.
