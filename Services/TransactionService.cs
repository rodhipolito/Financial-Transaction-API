using FinancialTransactionAPI.Data;
using FinancialTransactionAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FinancialTransactionAPI.Services;

public class TransactionService
{
    private readonly AppDbContext _db;
    private readonly AuditLogService _auditLog;

    public TransactionService(AppDbContext db, AuditLogService auditLog)
    {
        _db = db;
        _auditLog = auditLog;
    }

    public async Task<Transaction?> DepositAsync(Guid accountId, Guid userId, decimal amount, string description)
    {
        var account = await _db.Accounts
            .FirstOrDefaultAsync(a => a.Id == accountId && a.UserId == userId && a.IsActive);

        if (account == null) return null;

        account.Balance += amount;

        var transaction = new Transaction
        {
            AccountId = accountId,
            Type = TransactionType.Deposit,
            Amount = amount,
            Description = description,
            Status = TransactionStatus.Completed,
            ProcessedAt = DateTime.UtcNow
        };

        _db.Transactions.Add(transaction);
        await _db.SaveChangesAsync();

        await _auditLog.LogAsync("Deposit", "Transaction", transaction.Id.ToString(),
            $"Amount: {amount} {account.Currency}", userId);

        return transaction;
    }

    public async Task<Transaction?> WithdrawAsync(Guid accountId, Guid userId, decimal amount, string description)
    {
        var account = await _db.Accounts
            .FirstOrDefaultAsync(a => a.Id == accountId && a.UserId == userId && a.IsActive);

        if (account == null || account.Balance < amount) return null;

        account.Balance -= amount;

        var transaction = new Transaction
        {
            AccountId = accountId,
            Type = TransactionType.Withdrawal,
            Amount = amount,
            Description = description,
            Status = TransactionStatus.Completed,
            ProcessedAt = DateTime.UtcNow
        };

        _db.Transactions.Add(transaction);
        await _db.SaveChangesAsync();

        await _auditLog.LogAsync("Withdrawal", "Transaction", transaction.Id.ToString(),
            $"Amount: {amount} {account.Currency}", userId);

        return transaction;
    }

    public async Task<Transaction?> TransferAsync(Guid fromAccountId, Guid toAccountId, Guid userId, decimal amount, string description)
    {
        var fromAccount = await _db.Accounts
            .FirstOrDefaultAsync(a => a.Id == fromAccountId && a.UserId == userId && a.IsActive);

        var toAccount = await _db.Accounts
            .FirstOrDefaultAsync(a => a.Id == toAccountId && a.IsActive);

        if (fromAccount == null || toAccount == null || fromAccount.Balance < amount) return null;

        fromAccount.Balance -= amount;
        toAccount.Balance += amount;

        var transaction = new Transaction
        {
            AccountId = fromAccountId,
            DestinationAccountId = toAccountId,
            Type = TransactionType.Transfer,
            Amount = amount,
            Description = description,
            Status = TransactionStatus.Completed,
            ProcessedAt = DateTime.UtcNow
        };

        _db.Transactions.Add(transaction);
        await _db.SaveChangesAsync();

        await _auditLog.LogAsync("Transfer", "Transaction", transaction.Id.ToString(),
            $"Amount: {amount} from {fromAccountId} to {toAccountId}", userId);

        return transaction;
    }

    public async Task<List<Transaction>> GetByAccountAsync(Guid accountId, Guid userId)
    {
        var account = await _db.Accounts
            .FirstOrDefaultAsync(a => a.Id == accountId && a.UserId == userId);

        if (account == null) return new List<Transaction>();

        return await _db.Transactions
            .Where(t => t.AccountId == accountId || t.DestinationAccountId == accountId)
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync();
    }
}