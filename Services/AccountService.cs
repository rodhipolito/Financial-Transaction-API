using FinancialTransactionAPI.Data;
using FinancialTransactionAPI.Models;
using Microsoft.EntityFrameworkCore;


namespace FinancialTransactionAPI.Services;

public class AccountService
{
    private readonly AppDbContext _db;

    public AccountService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<Account> CreateAsync(Guid userId, AccountType type, string currency)
    {
        var account = new Account
        {
            UserId = userId,
            Type = type,
            Currency = currency
        };

        _db.Accounts.Add(account);
        await _db.SaveChangesAsync();
        return account;
    }

    public async Task<List<Account>> GetByUserAsync(Guid userId)
    {
        return await _db.Accounts
            .Where(a => a.UserId == userId && a.IsActive)
            .ToListAsync();
    }

    public async Task<Account?> GetByIdAsync(Guid id, Guid userId)
    {
        return await _db.Accounts
            .FirstOrDefaultAsync(a => a.Id == id && a.UserId == userId && a.IsActive);
    }

    public async Task<List<AccountSearchResult>> SearchAccountsAsync(string query, Guid excludeUserId)
    {
        var accounts = await _db.Accounts
            .Include(a => a.User)
            .Where(a => a.IsActive &&
                        a.UserId != excludeUserId &&
                        (a.User.Name.Contains(query) || a.User.Email.Contains(query)))
            .ToListAsync();

        return accounts.Select(a => new AccountSearchResult
        {
            Id = a.Id,
            UserName = a.User.Name,
            Email = a.User.Email,
            Currency = a.Currency,
            Type = a.Type.ToString()
        }).ToList();
    }
}

public record AccountSearchResult(Guid Id, string UserName, string Email, string Currency, string Type);