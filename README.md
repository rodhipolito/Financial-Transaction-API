# Financial Transaction API

A production-ready REST API simulating real-world financial operations, built with ASP.NET Core 8.

🚀 **Live Demo:** https://financial-transaction-api-production-3651.up.railway.app/swagger

## Features

- JWT Authentication with role-based access (User / Admin)
- Account management (Checking, Savings, Investment)
- Financial transactions (Deposit, Withdrawal, Transfer)
- Full audit logging of all operations
- Swagger UI documentation

## Tech Stack

- **Backend:** ASP.NET Core 8
- **Database:** PostgreSQL (Supabase)
- **ORM:** Entity Framework Core 8
- **Auth:** JWT Bearer Tokens
- **Docs:** Swagger / OpenAPI
- **Deploy:** Railway

## Endpoints

### Auth
| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | /api/Auth/register | Register new user |
| POST | /api/Auth/login | Login and get JWT token |

### Accounts
| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | /api/Accounts | Create account |
| GET | /api/Accounts | List user accounts |
| GET | /api/Accounts/{id} | Get account by ID |

### Transactions
| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | /api/Transactions/deposit | Deposit funds |
| POST | /api/Transactions/withdraw | Withdraw funds |
| POST | /api/Transactions/transfer | Transfer between accounts |
| GET | /api/Transactions/{accountId} | Get transaction history |

### Audit Logs (Admin only)
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | /api/AuditLogs | Get all audit logs |

## Getting Started

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
       "Key": "your_secret_key",
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

## Author

**Rodrigo Hipolito** — [GitHub](https://github.com/rodhipolito)