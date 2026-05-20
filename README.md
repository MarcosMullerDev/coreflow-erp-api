# 🚀 CoreFlow ERP API

Enterprise ERP backend built with ASP.NET Core 9, PostgreSQL and JWT Authentication.

A scalable multi-tenant ERP architecture focused on performance, security and real-world business scenarios.

---

# 📌 Features

## 🔐 Authentication & Security

* JWT Authentication
* BCrypt Password Hashing
* Role-based Authorization
* Protected Endpoints
* Current User Context

## 🏢 Multi-Tenant Architecture

* Company-based data isolation
* Users linked to companies
* Secure tenant filtering

## 📦 Product Management

* Product CRUD
* Stock Control
* Soft Delete
* Pagination
* SKU support

## 💰 Sales Module

* Sales creation
* Multiple sale items
* Automatic stock decrease
* Sales history
* Revenue calculation

## 📊 Dashboard Analytics

* Total Revenue
* Total Sales
* Total Products
* Low Stock Products

## 🧱 Architecture

* Clean Layered Architecture
* Repository Pattern
* Service Layer
* DTOs
* Dependency Injection
* Global Exception Middleware

---

# 🛠️ Tech Stack

* ASP.NET Core 9
* Entity Framework Core
* PostgreSQL
* JWT Bearer Authentication
* FluentValidation
* Swagger / OpenAPI
* BCrypt.Net
* C#

---

# 📂 Project Structure

```txt
src/
 ├── CoreFlow.Api
 ├── CoreFlow.Application
 ├── CoreFlow.Domain
 └── CoreFlow.Infrastructure
```

---

# 🔑 Authentication

This API uses JWT Bearer Authentication.

## Login Example

```http
POST /api/Auth/login
```

```json
{
  "email": "admin@coreflow.com",
  "password": "123456"
}
```

---

# 📦 Product Example

## Create Product

```http
POST /api/Products
```

```json
{
  "name": "Gaming Mouse",
  "description": "RGB Mouse",
  "sku": "MOUSE-001",
  "costPrice": 100,
  "salePrice": 180,
  "stockQuantity": 20
}
```

---

# 💵 Sale Example

## Create Sale

```http
POST /api/Sales
```

```json
{
  "items": [
    {
      "productId": "PRODUCT_GUID",
      "quantity": 2
    }
  ]
}
```

---

# 📊 Dashboard Endpoint

```http
GET /api/Dashboard/summary
```

---

# ⚙️ Running Locally

## Clone
