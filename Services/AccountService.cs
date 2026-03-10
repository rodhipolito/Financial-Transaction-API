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
}