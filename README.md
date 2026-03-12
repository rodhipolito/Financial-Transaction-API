# 🏦 Financial Transaction API

A production-ready REST API simulating real-world financial operations, built with **ASP.NET Core 8**.

🚀 **Live Demo:** https://financial-transaction-api-production-3651.up.railway.app/swagger

---

## ✨ Features

- 🔐 JWT Authentication with role-based access (User / Admin)
- 🏦 Account management (Checking, Savings, Investment)
- 💸 Financial transactions (Deposit, Withdrawal, Transfer)
- 📋 Full audit logging of all operations
- 📖 Interactive Swagger UI documentation

---

## 🛠 Tech Stack

| Layer | Technology |
|-------|-----------|
| Backend | ASP.NET Core 8 |
| Database | PostgreSQL (Supabase) |
| ORM | Entity Framework Core 8 |
| Auth | JWT Bearer Tokens |
| Docs | Swagger / OpenAPI |
| Deploy | Railway |

---

## 🧪 How to Test the API (Step by Step)

> 💡 You can test directly on the **[Live Demo](https://financial-transaction-api-production-3651.up.railway.app/swagger)** — no setup required!

### Step 1 — Register a user
`POST /api/Auth/register`
```json
{
  "name": "Your Name",
  "email": "your@email.com",
  "password": "YourPassword123!"
}
```
✅ Copy the **token** from the response.

---

### Step 2 — Authorize
Click the **🔒 Authorize** button at the top of the Swagger page.
Enter exactly: `Bearer YOUR_TOKEN_HERE` and click **Authorize**.

---

### Step 3 — Create an account
`POST /api/Accounts`
```json
{
  "type": 0,
  "currency": "EUR"
}
```
✅ Copy the **id** from the response — this is your `accountId`.

> Account types: `0` = Checking · `1` = Savings · `2` = Investment

---

### Step 4 — Make a deposit
`POST /api/Transactions/deposit`
```json
{
  "accountId": "YOUR_ACCOUNT_ID",
  "amount": 1000,
  "description": "Initial deposit"
}
```

---

### Step 5 — Explore!
| Action | Endpoint |
|--------|----------|
| Withdraw funds | `POST /api/Transactions/withdraw` |
| Transfer between accounts | `POST /api/Transactions/transfer` |
| Check transaction history | `GET /api/Transactions/{accountId}` |
| View all your accounts | `GET /api/Accounts` |

---

## 📡 API Endpoints

### 🔐 Auth
| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | /api/Auth/register | Register new user and get token |
| POST | /api/Auth/login | Login and get JWT token |

### 🏦 Accounts
| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | /api/Accounts | Create a new account |
| GET | /api/Accounts | List all user accounts |
| GET | /api/Accounts/{id} | Get account by ID |

### 💸 Transactions
| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | /api/Transactions/deposit | Deposit funds |
| POST | /api/Transactions/withdraw | Withdraw funds |
| POST | /api/Transactions/transfer | Transfer between accounts |
| GET | /api/Transactions/{accountId} | Get transaction history |

### 📋 Audit Logs *(Admin only)*
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | /api/AuditLogs | Get all system audit logs |

---

## 🚀 Getting Started (Local Setup)

### Prerequisites
- .NET 8 SDK
- PostgreSQL database

### Setup

1. Clone the repository
```bash
git clone https://github.com/rodhipolito/Financial-Transaction-API.git
cd Financial-Transaction-API
```

2. Configure `appsettings.json`
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "your_postgresql_connection_string"
  },
  "Jwt": {
    "Key": "your_secret_key_min_32_characters",
    "Issuer": "FinancialTransactionAPI",
    "Audience": "FinancialTransactionAPIUsers",
    "ExpiresInHours": 24
  }
}
```

3. Run migrations
```bash
dotnet ef database update
```

4. Start the API
```bash
dotnet run
```

5. Open Swagger UI at `http://localhost:5161/swagger`

---

## 👨‍💻 Author

**Rodrigo Hipolito** — [GitHub](https://github.com/rodhipolito/Financial-Transaction-API) · [LinkedIn](https://www.linkedin.com/in/rodrigo-hipolito)